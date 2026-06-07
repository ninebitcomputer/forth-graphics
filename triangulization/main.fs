require ../buffer.fs
require ../raylib.fs

128 constant NPOINTS
create pbuf NPOINTS Vector2 * allot
create l-next NPOINTS cells allot
create l-prev NPOINTS cells allot
variable cdll-head

variable total-points 0 total-points !
variable draw-lines? false draw-lines? !

create tempv Vector2 allot
create mouse-pos Vector2 allot

800 constant swidth
450 constant sheight
60 constant fps

: cdll-init ( -- )
	total-points @
	dup 3 < if drop exit then
	0 cdll-head !
	dup 0 do
		r@ 1 + l-next r@ cells + !
		r@ 1 - l-prev r@ cells + !
	loop
	dup 1 - cells l-next + 0 swap !
	l-prev ! ;

: a-point ( idx -- addr ) Vector2 * pbuf + ;
: point ( idx -- x y ) a-point Vector2@ ;

: cdll-next ( node -- node ) cells l-next + @ ;
: cdll-prev ( node -- node ) cells l-prev + @ ;
: polygon-filled? ( -- flag ) cdll-head @ dup cdll-next cdll-next = ;

: calculate-orientation ( pidx -- det )
	dup cdll-prev point dup point vec2-
	dup cdll-next point point vec2-
	vec2-det ;

: fill-polygon ( -- )
	total-points @ 3 < if exit then
	cdll-init 

	cdll-head @
	begin ( cur )
		dup a-point
		over calculate-orientation 0e f< if GREEN else BLUE then
		2
		swap
		DrawCircleV


		cdll-next
	dup cdll-head @ <> while repeat drop
	;


: add-point ( px py -- )
	total-points @ NPOINTS < 0= if 2drop exit then 
	total-points @ a-point Vector2!
	total-points @ 1 + total-points ! ;

: draw-points ( -- )
	total-points @ 0= if exit then

	pbuf
	total-points @ 0 do
		dup 3 RED DrawCircleV
		Vector2 +
	loop drop ;

: draw-lines ( -- )
	total-points @ 3 < if exit then 
	pbuf dup Vector2 +
	total-points @ 1 - 0 do
		2dup BLACK DrawLineV
		swap drop dup Vector2 +
	loop
	drop pbuf BLACK DrawLineV ;

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

		KEY_L	IsKeyPressed if draw-lines? @ invert draw-lines? ! then
		KEY_X IsKeyPressed if mouse-pos Vector2@ add-point then
		KEY_Z IsKeyPressed if undo-point then

		BeginDrawing
			WHITE ClearBackground

			mouse-pos 3 BLUE DrawCircleV
			draw-lines? if draw-lines then
			draw-points
			fill-polygon

		EndDrawing
	repeat

	CloseWindow ;

main
