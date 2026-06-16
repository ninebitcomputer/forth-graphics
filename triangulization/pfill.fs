require ../raylib.fs

1024 constant NPOINTS
create d-pbuf NPOINTS Vector2 * allot
create l-next NPOINTS cells allot
create l-prev NPOINTS cells allot

variable total-points 0 total-points !
variable current-pbuf d-pbuf current-pbuf !

: pbuf current-pbuf @ ;
: set-pbuf ( len pbuf -- ) current-pbuf ! total-points ! ;
: reset-pbuf ( -- ) 0 d-pbuf set-pbuf ;

: a-point ( idx -- addr ) Vector2 * pbuf + ;
: point ( idx -- x y ) a-point Vector2@ ;


: cdll-init ( -- )
	total-points @
	dup 3 < if drop exit then
	dup 0 do
		r@ 1 + l-next r@ cells + !
		r@ 1 - l-prev r@ cells + !
	loop
	1 - dup cells l-next + 0 swap !
	l-prev ! ;

: l-next-a ( node -- addr ) cells l-next + ;
: l-prev-a ( node -- addr ) cells l-prev + ;

: >cdll-next ( val node -- ) l-next-a ! ;
: >cdll-prev ( val node -- ) l-prev-a ! ;

: cdll-next ( node -- node ) l-next-a @ ;
: cdll-prev ( node -- node ) l-prev-a @ ;

: cdll-del ( node -- ) 
	dup cdll-next >r cdll-prev 
	dup r@ swap >cdll-next
	r> >cdll-prev ;

: polygon-filled? ( head -- flag ) dup cdll-next cdll-next = ;

: ta ( tidx -- tidx ) ;
: tb ( tidx -- tidx ) cdll-next ;
: tc ( tidx -- tidx ) cdll-next cdll-next ;

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
	in-bary? ;

: _t-cond ( end cur -- end cur flag )
	2dup <> if 2dup pit? 0= else false then ;

: good-triangle? ( tidx -- flag )
	dup cdll-prev swap cdll-next cdll-next
	( end cur )
	begin
		_t-cond
	while
			cdll-next
	repeat
	= ;

: calculate-orientation ( pidx -- det )
	dup cdll-prev point dup point vec2-
	dup cdll-next point point vec2-
	vec2-det ;

: convex? ( pidx -- flag ) calculate-orientation 0e f< ;

: draw-triangle ( tidx color -- )
	>r dup cdll-next a-point swap
	dup a-point swap
	cdll-prev a-point r> DrawTriangle ;

: fill-shape ( color -- )
	total-points @ 3 < if drop exit then
	cdll-init 
	>r

	0 0 0
	begin ( enc last cur )
		2dup = if rot 1+ >r r@ -rot r> 2 < else true then
		over polygon-filled? 0=
		and
	while
		dup convex? if 
			dup good-triangle? if 

				dup r@ draw-triangle
				dup cdll-del
				swap drop swap drop 0 swap dup cdll-next swap

		then then
		cdll-next
	repeat 2drop drop rdrop ;

: begin-shape ( -- ) 0 total-points ! ;

: add-point ( px py -- )
	total-points @ NPOINTS < 0= if 2drop exit then 
	total-points @ a-point Vector2!
	total-points @ 1 + total-points ! ;

: set-buffer ( len addr -- ) 2drop ;
: use-default-buffer ( -- ) 0 pbuf set-buffer ;


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
