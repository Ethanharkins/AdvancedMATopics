using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircularTerrain : MonoBehaviour
{
    public int segments = 200;
    public int rings = 200;
    public float radius = 50f;
    public float heightMultiplier = 3f;
    public float noiseScale = 0.05f;

    void Start()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(segments + 1) * (rings + 1)];
        int[] triangles = new int[segments * rings * 6];
        Color[] colors = new Color[vertices.Length];

        int vertIndex = 0;
        for (int r = 0; r <= rings; r++)
        {
            float rPercent = (float)r / rings;
            float rDist = rPercent * radius;

            for (int s = 0; s <= segments; s++)
            {
                float angle = 2 * Mathf.PI * s / segments;
                float x = Mathf.Cos(angle) * rDist;
                float z = Mathf.Sin(angle) * rDist;

                // Fractal noise (multiple octaves)
                float y = 0f;
                float amplitude = 1f;
                float frequency = 1f;
                for (int o = 0; o < 4; o++) // 4 octaves
                {
                    float nx = x * noiseScale * frequency;
                    float nz = z * noiseScale * frequency;
                    y += Mathf.PerlinNoise(nx, nz) * amplitude;

                    amplitude *= 0.5f;
                    frequency *= 2f;
                }
                y *= heightMultiplier;

                vertices[vertIndex] = new Vector3(x, y, z);

                // Smooth terrain color
                float t = Mathf.InverseLerp(0, heightMultiplier * 1.5f, y); // scale max height slightly
                Color color;
                if (t < 0.3f)
                    color = Color.Lerp(new Color(0, 0.3f, 1f), Color.green, t / 0.3f); // blue to green
                else if (t < 0.7f)
                    color = Color.Lerp(Color.green, new Color(0.8f, 0.8f, 0.8f), (t - 0.3f) / 0.4f); // green to gray
                else
                    color = Color.Lerp(new Color(0.8f, 0.8f, 0.8f), Color.white, (t - 0.7f) / 0.3f); // gray to white

                colors[vertIndex] = color;
                vertIndex++;
            }
        }

        int triIndex = 0;
        for (int r = 0; r < rings; r++)
        {
            for (int s = 0; s < segments; s++)
            {
                int i0 = r * (segments + 1) + s;
                int i1 = i0 + 1;
                int i2 = i0 + segments + 1;
                int i3 = i2 + 1;

                triangles[triIndex++] = i0;
                triangles[triIndex++] = i2;
                triangles[triIndex++] = i1;

                triangles[triIndex++] = i1;
                triangles[triIndex++] = i2;
                triangles[triIndex++] = i3;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
