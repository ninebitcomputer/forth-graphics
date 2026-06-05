require ./util.fs

c-library raylib

  begin-structure Color
    cfield: Color-r
	cfield: Color-g
	cfield: Color-b
  	cfield: Color-a
  end-structure


  : >Color ( r g b a -- addr )
	Color allocate throw >R
	R@ Color-a c!
	R@ Color-b c!
	R@ Color-g c!
	R@ Color-r c!
	R> ;

  begin-structure Vector2
	sffield: Vector2-x
	sffield: Vector2-y
  end-structure


  : Vector2! ( x y addr -- )
	dup Vector2-y sf!
	Vector2-x sf! ;

  : Vector2@ ( addr -- x y)
	dup Vector2-x sf@
	Vector2-y sf@ ;

  : >Vector2 ( x y -- addr)
	Vector2 allocate throw
	dup Vector2! ;


  : vec2-piecewise ( x1 y1 x2 y2 xt -- x y)
	frot fswap dup execute
	frot frot execute fswap ;

  : vec2-scalar ( x y s xt -- x y )
	ftuck dup execute
	frot frot execute fswap ;

  : vec2-apply ( x1 y1 xt -- x y )
	fswap dup execute
	fswap execute ;

  : vec2- ( x1 y1 x2 y2 -- x y ) ['] f- vec2-piecewise ;
  : vec2+ ( x1 y1 x2 y2 -- x y ) ['] f+ vec2-piecewise ;

  : vec2s- ( x y s -- x y ) ['] f- vec2-scalar ;
  : vec2s+ ( x y s -- x y ) ['] f+ vec2-scalar ;
  : vec2s* ( x y s -- x y ) ['] f* vec2-scalar ;
  : vec2s/ ( x y s -- x y ) ['] f/ vec2-scalar ;

  : vec2-msr ( x y -- msr ) ['] fsq vec2-apply f+ ;
  : vec2-mag ( x y -- mag ) vec2-msr fsqrt ;

  : vec2-norm ( x y -- x' y' )
	fover fover vec2-mag vec2s/ ;

  0 0 0 255 			>Color Constant BLACK
  255 0 0 255			>Color Constant RED
  0 255 0 255 			>Color Constant GREEN
  0 0 255 255			>Color Constant BLUE
  255 255 255 255		>Color Constant WHITE

	76	Constant KEY_L
	88	Constant KEY_X
	90 	Constant KEY_Z


  s" raylib" add-lib
  \c #include <raylib.h>

  c-function InitWindow InitWindow n n a -- void
  c-function WindowShouldClose WindowShouldClose -- n
  c-function CloseWindow CloseWindow -- void

  c-function SetTargetFPS SetTargetFPS n -- void
  c-function GetFPS GetFPS -- n

  c-function IsKeyPressed IsKeyPressed n -- n

  c-function ClearBackground ClearBackground a{*(Color *)} -- void

  c-function BeginDrawing BeginDrawing -- void
  c-function EndDrawing EndDrawing -- void

  c-function DrawText DrawText a n n n a{*(Color*)} -- void
  c-function DrawCircleV DrawCircleV a{*(Vector2 *)} n a{*(Color *)} -- void
  c-function DrawCircleLinesV DrawCircleLinesV a{*(Vector2 *)} n a{*(Color *)} -- void
  c-function DrawLineV DrawLineV a{*(Vector2 *)} a{*(Vector2 *)} a{*(Color *)} -- void

  c-function GetFrameTime GetFrameTime -- r
  c-function GetMouseWheelMove GetMouseWheelMove -- r

  c-function GetMouseX GetMouseX -- n
  c-function GetMouseY GetMouseY -- n


end-c-library
