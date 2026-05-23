require ./raylib.fs

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

  800 450 s" delta time" drop InitWindow

  begin
	WindowShouldClose 0=
  while ( pos1 pos2 )
	over update-delta

	BeginDrawing
	  WHITE ClearBackground

	  dup 20 GREEN DrawCircleV
	  over 20 RED DrawCircleV
	
	EndDrawing
  repeat

  CloseWindow 
  free free ;

main
