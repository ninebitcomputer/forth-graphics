require ./raylib.fs

: main

  800 800 s" Hello forth" drop InitWindow

  begin
    WindowShouldClose 0=
  while
    BeginDrawing
	  WHITE ClearBackground
      s" hello from gforth" drop 190 200 20 BLACK DrawText
    EndDrawing
  repeat

  CloseWindow ;

main

bye
