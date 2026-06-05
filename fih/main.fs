require ../buffer.fs
require ../raylib.fs
require ./anim.fs

800 constant swidth
450 constant sheight

5e fconstant follow-speed

60 constant fps

variable addr-head 420e 200e >Vector2 addr-head !
create tempv Vector2 allot

: head ( -- addr ) addr-head @ ;

: main

  swidth sheight s" fih" drop InitWindow
  fps SetTargetFPS

  


  10e 10e 10e 10e 10e 10e 10e 12e 14e 16e 18e 20e 25e 15e 14 init-sizes
  15e 15e 15e 15e 15e 15e 15e 15e 15e 15e 20e 20e 20e 20e 14 init-links
  head Vector2@ move-head
  14 init-propogate

  begin
	WindowShouldClose 0=
  while
	BeginDrawing
	  WHITE ClearBackground

	  GetMouseX s>f GetMouseY s>f tempv Vector2!

	  tempv 10 BLUE DrawCircleV

	  head Vector2@
	  tempv Vector2@
	  head Vector2@

	  vec2-
	  fover fover
	  vec2-mag fdup 5e f< if
		fdrop fdrop fdrop 0e 0e
	  else
		vec2s/ follow-speed vec2s*
	  then

	  vec2+

	  tempv Vector2!

	  tempv Vector2@ head Vector2!
	  head Vector2@ move-head

	  14 propogate
	  14 draw-nodes

	EndDrawing
  repeat

  CloseWindow ;

main
