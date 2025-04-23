using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GenerationManager : MonoBehaviour
{
    public int seed = 0;
    public int roomAmount = 6;
    public RoomScript[] spawnRooms;
    public RoomScript[] rooms;

    [Header("Dangers")]
    public int dangerRoomAmount = 3;
    public RoomScript[] dangerRooms;

    [Header("Closing Exits")]
    public WallScript[] walls;
    public float exitProximityTolerance = 0.1f;

    [Header("Progression Door & Key")]
    public GameObject progressionDoorPrefab;
    public GameObject progressionKeyPrefab;

    [Header("Final Door & Key")]
    public GameObject finalDoorPrefab;
    public GameObject finalKeyPrefab;

    private List<RoomScript> spawnedRooms;
    private List<RoomScript.ExitClass> openExits;
    private List<RoomScript.ExitClass> connectedExits;

    void Start()
    {
        spawnedRooms = new List<RoomScript>();
        openExits = new List<RoomScript.ExitClass>(); // Ensures it's initialized
        connectedExits = new List<RoomScript.ExitClass>();

        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        Random.InitState(seed);
        spawnedRooms.Clear();
        openExits.Clear();
        connectedExits.Clear();

        Deck<RoomScript> roomsToBeSpawned = new Deck<RoomScript>();
        Deck<RoomScript> secondHalf = new Deck<RoomScript>();

        int randRoom = Random.Range(0, spawnRooms.Length);
        SpawnRoom(spawnRooms[randRoom], true);

        for (int n = 1; n < roomAmount - dangerRoomAmount; n++)
        {
            randRoom = Random.Range(0, rooms.Length);
            if (n < (roomAmount - dangerRoomAmount) / 2)
            {
                roomsToBeSpawned.Add(rooms[randRoom]);
            }
            else
            {
                secondHalf.Add(rooms[randRoom]);
            }
        }

        for (int i = 0; i < dangerRoomAmount; i++)
        {
            randRoom = Random.Range(0, dangerRooms.Length);
            secondHalf.Add(dangerRooms[randRoom]);
        }

        roomsToBeSpawned.Shuffle();
        secondHalf.Shuffle();

        for (int r = 0; r < roomsToBeSpawned.Count; r++)
        {
            SpawnRoom(roomsToBeSpawned[r]);
        }

        for (int r2 = 0; r2 < secondHalf.Count; r2++)
        {
            SpawnRoom(secondHalf[r2]);
        }

        ConnectExits();
        CloseOffExits();

        Debug.Log($"Connected Exits Count: {connectedExits.Count}");

        // **New method: Spawn the progression door at an exit instead of replacing a wall**
        SpawnSingleProgressionDoor();

        // **Spawn final key and door**
        ReplaceWallWithFinalDoorAndKey();
    }
    private void ConnectExits()
    {
        if (openExits == null)
        {
            Debug.LogError("Error: `openExits` is null. Initializing a new list.");
            openExits = new List<RoomScript.ExitClass>();
        }

        if (openExits.Count < 2)
        {
            Debug.LogWarning("Not enough open exits to connect.");
            return;
        }

        List<RoomScript.ExitClass> exitsToProcess = new List<RoomScript.ExitClass>(openExits);

        for (int i = exitsToProcess.Count - 1; i >= 0; i--)
        {
            for (int other = 0; other < i; other++)
            {
                if (!exitsToProcess[i].CanConnect(exitsToProcess[other]))
                    continue;

                if (Vector3.Distance(
                        exitsToProcess[i].alignmentObject.position,
                        exitsToProcess[other].alignmentObject.position) <= exitProximityTolerance)
                {
                    Debug.Log($"Connecting {exitsToProcess[i].name} with {exitsToProcess[other].name}");
                    connectedExits.Add(exitsToProcess[i]);
                    connectedExits.Add(exitsToProcess[other]);

                    openExits.Remove(exitsToProcess[i]);
                    openExits.Remove(exitsToProcess[other]);
                    exitsToProcess.RemoveAt(i);
                    exitsToProcess.RemoveAt(other);
                    i--;
                    break;
                }
            }
        }

        Debug.Log($"Connected Exits: {connectedExits.Count}, Remaining Open Exits: {openExits.Count}");
    }



    private void SpawnRoom(RoomScript room, bool ignoreConnections = false)
    {
        RoomScript newRoom = Instantiate(room, Vector3.zero, Quaternion.identity, transform);
        spawnedRooms.Add(newRoom);
        Debug.Log("Spawned Room: " + newRoom.name);

        foreach (var exit in newRoom.exits)
        {
            foreach (string keyword in exit.keywords)
            {
                if (ignoreConnections)
                {
                    RegisterExitsInRoom(newRoom);
                    return;
                }

                if (openExits.Any(e => e.keywords.Contains(keyword)))
                {
                    foreach (var otherExit in openExits)
                    {
                        if (!newRoom.TryConnect(exit, otherExit))
                        {
                            continue;
                        }

                        RegisterExitsInRoom(newRoom, exit);
                        connectedExits.Add(exit);
                        connectedExits.Add(otherExit);
                        openExits.Remove(otherExit);
                        return;
                    }
                }
            }
        }
    }

    private void RegisterExitsInRoom(RoomScript room, RoomScript.ExitClass exceptForExit = null)
    {
        foreach (var newExit in room.exits)
        {
            if (newExit == exceptForExit)
                continue;

            openExits.Add(newExit);
        }
    }

    private void CloseOffExits()
    {
        if (openExits.Count == 0)
        {
            Debug.LogWarning("No open exits left to close.");
            return;
        }

        foreach (var exit in openExits.ToList()) // Use ToList() to avoid modifying during iteration
        {
            WallScript newWall = Instantiate(walls[Random.Range(0, walls.Length)]);
            if (newWall.TryConnect(exit))
            {
                Debug.Log($"Wall placed at: {exit.alignmentObject.position}");
                openExits.Remove(exit);
            }
        }
    }


    private void SpawnSingleProgressionDoor()
    {
        if (connectedExits.Count == 0)
        {
            Debug.LogError("No connected exits available for a progression door.");
            return;
        }

        RoomScript.ExitClass chosenExit = connectedExits[Random.Range(0, connectedExits.Count)];

        Vector3 doorPosition = chosenExit.alignmentObject.position;
        Quaternion doorRotation = chosenExit.alignmentObject.rotation;

        Instantiate(progressionDoorPrefab, doorPosition, doorRotation);
        Debug.Log($"Progression Door placed at exit: {chosenExit.name} at {doorPosition}");

        RoomScript keyRoom = GetAccessibleRoomWithoutDoors();
        if (keyRoom != null)
        {
            Vector3 keyPosition = keyRoom.transform.position + Vector3.up * 0.25f;
            Instantiate(progressionKeyPrefab, keyPosition, Quaternion.identity);
            Debug.Log($"Progression Key placed in: {keyRoom.name} at {keyPosition}");
        }
    }

    private void ReplaceWallWithFinalDoorAndKey()
    {
        if (connectedExits.Count == 0)
        {
            Debug.LogError("No exits available for the final door.");
            return;
        }

        RoomScript.ExitClass chosenExit = connectedExits[Random.Range(0, connectedExits.Count)];

        Vector3 doorPosition = chosenExit.alignmentObject.position;
        Quaternion doorRotation = chosenExit.alignmentObject.rotation;

        Instantiate(finalDoorPrefab, doorPosition, doorRotation);
        Debug.Log($"Final Door placed at exit: {chosenExit.name} at {doorPosition}");

        RoomScript farthestRoom = FindFarthestRoom(doorPosition);
        Instantiate(finalKeyPrefab, farthestRoom.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Debug.Log($"Final Key placed in farthest room: {farthestRoom.name}");
    }

    private RoomScript GetAccessibleRoomWithoutDoors()
    {
        if (spawnedRooms.Count == 0)
        {
            Debug.LogError("No rooms available to place a key.");
            return null;
        }

        return spawnedRooms[Random.Range(0, spawnedRooms.Count)];
    }

    private RoomScript FindFarthestRoom(Vector3 targetPosition)
    {
        return spawnedRooms.OrderByDescending(room => Vector3.Distance(room.transform.position, targetPosition)).FirstOrDefault();
    }
}
