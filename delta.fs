require ./raylib.fs

: main
  0e 0e >Vector2

  800 450 s" delta time" drop InitWindow

  begin
	WindowShouldClose 0=
  while
	BeginDrawing
	  WHITE ClearBackground

	  dup 45 BLACK DrawCircleV
	
	EndDrawing
  repeat

  CloseWindow ;

main
