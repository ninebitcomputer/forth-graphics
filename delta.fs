require ./raylib.fs
require ./buffer.fs

42.0e fconstant speed
800 constant swidth
450 constant sheight

: update-delta ( pos -- )
  dup Vector2@
  fswap
	GetFrameTime 6.0e speed f* f* f+
	fdup swidth s>f f> if fdrop 0e then
  fswap
  Vector2! ;
  

: main
  32e 100e >Vector2
  32e 200e >Vector2

  swidth sheight s" delta time" drop InitWindow

  begin
	WindowShouldClose 0=
  while ( pos1 pos2 )
	over update-delta

	sbuf-reset
	BeginDrawing
	  WHITE ClearBackground

	  dup 20 GREEN DrawCircleV
	  over 20 RED DrawCircleV

	  s" dynamically generated string test" %s sbuf-save-str0 drop
	  10 10 20 BLACK DrawText

	
	EndDrawing

  repeat

  CloseWindow 
  free free ;

main
