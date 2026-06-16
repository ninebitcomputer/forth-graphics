require ../raylib.fs

800 constant swidth
450 constant sheight
60 constant fps

create points 4 Vector2 * allot
create mouse-pos Vector2 allot

create sv1 Vector2 allot
create sv2 Vector2 allot
create sv3 Vector2 allot

: a-point ( pidx -- a ) Vector2 * points + ;
: draw-point ( pidx -- ) a-point 3 RED DrawCircleV ;


: point ( pidx -- x y ) a-point Vector2@ ;
: pnext ( pidx -- pidx ) 1 + 3 mod ;
: pprev ( pidx -- pidx ) 1 - 3 mod ;

: ta ( tidx -- tidx ) ;
: tb ( tidx -- tidx ) pnext ;
: tc ( tidx -- tidx ) pnext pnext ;

: vab ( tidx -- vec ) dup tb point ta point vec2- ;
: vac ( tidx -- vec ) dup tc point ta point vec2- ;
: vap ( tidx pidx -- vec ) point ta point vec2- ;

: bary-range? ( val area -- flag )
	fover 0.0e f< if fdrop fdrop false exit then
	f< ;


: in-bary? ( abp apc abc -- flag )
	f>r
	fover fover f+ fr@ fswap f-

	fr@ bary-range?
	fr@ bary-range?
	fr> bary-range?
	and and ;

: pit? ( tidx pidx -- flag ) 
	over vab over vac vec2-det
	over vab 2dup vap vec2-det
	2dup vap drop vac vec2-det
	frot
	f.s cr
	in-bary? ;

: main
	swidth sheight s" triangle" drop InitWindow
	fps SetTargetFPS

	300e 250e 2 a-point vector2!
	550e 100e 1 a-point vector2!
	375e 35e 0 a-point vector2!

	begin
		WindowShouldClose 0=
	while
		GetMouseX s>f GetMouseY s>f mouse-pos Vector2!

		mouse-pos Vector2@ 3 a-point Vector2!

		BeginDrawing
			WHITE ClearBackground

			mouse-pos 3 
				0 3 pit? if RED else BLUE then
			DrawCircleV

			0 draw-point 1 draw-point 2 draw-point

		EndDrawing
	repeat

	CloseWindow ;

main
