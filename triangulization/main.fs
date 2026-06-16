require ../raylib.fs
require ./pfill.fs

128 constant NPOINTS
create pbuf NPOINTS Vector2 * allot

create tempv Vector2 allot
create mouse-pos Vector2 allot

800 constant swidth
450 constant sheight
60 constant fps

: a-point ( idx -- addr ) Vector2 * pbuf + ;
: point ( idx -- x y ) a-point Vector2@ ;

: add-point ( px py -- )
	total-points @ NPOINTS < 0= if 2drop exit then 
	total-points @ a-point Vector2!
	total-points @ 1 + total-points ! ;

: undo-point ( -- )
	total-points @ 1 -
	dup 0 < if drop exit then
	total-points ! ;

: main
	swidth sheight s" triangle" drop InitWindow
	fps SetTargetFPS

	begin
		WindowShouldClose 0=
	while
		GetMouseX s>f GetMouseY s>f mouse-pos Vector2!

		KEY_X IsKeyPressed if mouse-pos Vector2@ add-point then
		KEY_Z IsKeyPressed if undo-point then

		BeginDrawing
			WHITE ClearBackground

			mouse-pos 3 BLUE DrawCircleV

			total-points @ pbuf set-pbuf
			draw-points
			GREEN fill-shape

		EndDrawing
	repeat

	CloseWindow ;

main
