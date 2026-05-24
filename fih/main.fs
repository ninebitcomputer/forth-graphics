require ../buffer.fs
require ../raylib.fs
require ./anim.fs

800 constant swidth
450 constant sheight

60 constant fps

variable addr-head 67e 67e >Vector2 addr-head !
create tempv Vector2 allot

: head ( -- addr ) addr-head @ ;

: main

  swidth sheight s" fih" drop InitWindow
  fps SetTargetFPS

  begin
	WindowShouldClose 0=
  while
	BeginDrawing
	  WHITE ClearBackground

	  GetMouseX s>f GetMouseY s>f tempv Vector2!

	  head tempv RED DrawLineV

	  tempv Vector2@ head Vector2@ vec2- tempv Vector2!

	  head 20 RED DrawCircleV

	EndDrawing
  repeat

  CloseWindow ;

main
