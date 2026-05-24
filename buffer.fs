256 constant SBUF-CAP

variable sbuf-used 0 sbuf-used !
variable sbuf-work-off 0 sbuf-work-off !

create sbuf SBUF-CAP allot

: sbuf-reset ( -- )
  0 sbuf-used !
  0 sbuf-used !
  0 sbuf-work-off ! ;

: sbuf-free ( -- count ) SBUF-CAP sbuf-used @ - ;

: sbuf-a-cur ( -- addr ) sbuf sbuf-used @ + ;
: sbuf-a-work ( -- addr ) sbuf sbuf-work-off @ + ;
: sbuf-alloc  ( count -- addr ) sbuf-a-cur swap sbuf-used @ + sbuf-used ! ;

: sbuf-work-length ( -- length ) sbuf-used @ sbuf-work-off @ - ;

: trim-space ( addr len -- addr len ) 
  BEGIN
	over c@ bl = 
	over 0 >
	and
  WHILE
	swap 1 + swap
	1 -
  REPEAT ;


: sbuf-putc ( char -- ) sbuf-a-cur c! sbuf-used dup @ 1 + swap ! ;
: sbuf-puts ( addr u2 -- ) tuck sbuf-a-cur swap cmove sbuf-used @ + sbuf-used ! ;
: sbuf-putd ( n -- ) dup s>d dabs <# #s sign #> sbuf-puts ;
: sbuf-putf ( prec f -- ) 18 swap 0 f>str-rdp trim-space sbuf-puts ;

: sbuf-save-str ( -- addr length )
  sbuf-a-work sbuf-work-length
  sbuf-used @ sbuf-work-off ! ;

: sbuf-save-str0 ( -- addr length)
  0 sbuf-putc
  sbuf-save-str ;


: %c sbuf-putc ;
: %s sbuf-puts ;
: %d sbuf-putd ;
: %f sbuf-putf ;

: test
  sbuf-reset
  'p' %c
  'o' %c
  'o' %c
  'p' %c
  sbuf-save-str0
  type

  s" hello" %s
  bl %c
  s" world" %s
  sbuf-save-str0
  type ;
