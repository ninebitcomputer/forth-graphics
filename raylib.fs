c-library raylib

  begin-structure Color
    cfield: Color-b
	cfield: Color-r
	cfield: Color-g
  	cfield: Color-a
  end-structure

  : >Color ( r g b a -- addr )
	Color allocate throw >R
	R@ Color-a c!
	R@ Color-b c!
	R@ Color-g c!
	R@ Color-r c!
	R> ;

  0 0 0 255 			>Color Constant BLACK
  255 255 255 255		>Color Constant WHITE

  s" raylib" add-lib
  \c #include <raylib.h>

  c-function InitWindow InitWindow n n a -- void
  c-function WindowShouldClose WindowShouldClose -- n
  c-function CloseWindow CloseWindow -- void

  c-function SetTargetFPS SetTargetFPS n -- void
  c-function ClearBackground ClearBackground a{*(Color *)} -- void

  c-function BeginDrawing BeginDrawing -- void
  c-function EndDrawing EndDrawing -- void

  c-function DrawText DrawText a n n n a{*(Color*)} -- void

end-c-library
