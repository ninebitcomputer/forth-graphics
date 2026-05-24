require ./raylib.fs
require ./buffer.fs

42.0e fconstant speed
800 constant swidth
450 constant sheight

variable fps 60 fps !

: update-x ( x pos -- )
  dup Vector2-x sf@
  f+
  fdup swidth s>f f> if fdrop 0e then
  Vector2-x sf! ;

: update-delta ( pos -- )
  GetFrameTime 6.0e speed f* f*
  update-x ;
  

: main
  32e 100e >Vector2
  32e 200e >Vector2

  swidth sheight s" delta time" drop InitWindow
  fps @ SetTargetFPS

  begin
	WindowShouldClose 0=
  while ( pos1 pos2 )
	GetMouseWheelMove
	fdup 0e f<> if
	  f>d d>s
	  fps @ +
	  dup 0 < if drop 0 then
	  dup fps ! SetTargetFPS
	else 
	  fdrop
	then

	over GetFrameTime 6.0e speed f* f* update-x
	dup 0.1e speed f* update-x

	sbuf-reset
	BeginDrawing
	  WHITE ClearBackground

	  dup 20 BLUE DrawCircleV
	  over 20 RED DrawCircleV

	  s" FPS: " %s
	  GetFPS %d 
	  s"  (target: " %s
	  fps @ %d
	  s" )" %s
	  sbuf-save-str0 drop
	  10 10 20 BLACK DrawText
	
	EndDrawing

  repeat

  CloseWindow 
  free free ;

main
