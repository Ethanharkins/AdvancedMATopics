A: Hey, do you play soccer?
B: Yeah, sometimes. Why?

A: We’re getting a game going at the park. Want to join?

* Sure, sounds fun.
    B: Sweet. We could use another striker.
    -> join_game
* Not today, I’m busy.
    B: All good, maybe next time.
    -> skip_game

== join_game ==
A: alright, we’ll meet at the south field in 10.
-> END

== skip_game ==
A: No worries. We’ll be out there for a while if you change your mind.
-> END
