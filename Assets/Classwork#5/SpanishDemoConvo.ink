A: Oye, ¿juegas fútbol?
B: Sí, a veces. ¿Por qué?

A: Vamos a armar una partida en el parque. ¿Te unes?

* Claro, suena divertido.
    B: Genial. Nos vendría bien otro delantero.
    -> join_game
* No hoy, estoy ocupado.
    B: Está bien, tal vez la próxima.
    -> skip_game

== join_game ==
A: Perfecto, nos vemos en el campo sur en 10 minutos.
-> END

== skip_game ==
A: No hay problema. Estaremos allí un buen rato por si cambias de opinión.
-> END
