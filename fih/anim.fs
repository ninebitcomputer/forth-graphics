require ../raylib.fs

128 constant NODE-CAP

create node-table-pos NODE-CAP Vector2 * allot
create node-table-link NODE-CAP floats allot
create node-table-size NODE-CAP floats allot

: >v2x ( x addr -- ) Vector2-x sf! ;
: v2x> ( addr -- x ) Vector2-x sf@ ;

: >v2y ( y addr -- ) Vector2-y sf! ;
: v2y> ( addr -- y ) Vector2-y sf@ ;

: node ( idx -- nidx ) ; \ TODO: bounds checking

: pos-addr ( nidx -- addr ) Vector2 * node-table-pos + ;
: link-addr ( nidx -- addr ) floats node-table-link + ;
: size-addr ( nidx -- addr ) floats node-table-size + ;

: pos> ( nidx -- x y ) pos-addr Vector2@ ;
: >pos ( x y nidx -- ) pos-addr Vector2! ;

: link> ( nidx -- f ) link-addr sf@ ;
: >link ( f nidx -- ) link-addr sf! ;

: size> ( nidx -- f) size-addr sf@ ;
: >size ( f nidx -- ) size-addr sf! ;

: move-head ( x y -- ) 0 node >pos ;

: init-sizes ( ... +n -- ) 0 do r@ node >size loop ;
: init-links ( ... +n -- ) 0 do r@ node >link loop ;

: init-propogate ( n -- )
  dup 1 < if drop exit then
  1 do 
	r@ 1 - node dup
	  link>
	  pos>
	  frot
	  0e
	  vec2+
	r@ node >pos
  loop ;

: propogate ( n -- )
  1 do
	r@ node pos>
	r@ 1 - node pos>
	vec2- vec2-norm
	r@ node link> vec2s*

	r@ 1 - node pos> vec2+
	r@ node >pos
  loop ;

: draw-nodes ( n -- )
  0 do
	r@ node pos-addr
	r@ node size> f>s
	r@ 0 = if RED else BLUE then

	DrawCircleLinesV
  loop ;

