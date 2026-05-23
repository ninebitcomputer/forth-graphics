\ https://github.com/jkotlinski/forth-strfmt
\ Copyright (c) 2017 Johan Kotlinski
\ 
\ Permission is hereby granted, free of charge, to any person obtaining a copy
\ of this software and associated documentation files (the "Software"), to deal
\ in the Software without restriction, including without limitation the rights
\ to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
\ copies of the Software, and to permit persons to whom the Software is
\ furnished to do so, subject to the following conditions:
\ 
\ The above copyright notice and this permission notice shall be included in all
\ copies or substantial portions of the Software.
\ 
\ THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
\ IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
\ FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
\ AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
\ LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
\ OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
\ SOFTWARE.


variable min-field-width
create left-justify 1 allot
create pad-char 1 allot
create charbuf 1 allot

: pad-left ( dst c-addr u -- dst c-addr u )
left-justify c@ if exit then dup min-field-width @ < if
min-field-width @ over - >r rot dup r@ pad-char c@ fill r> + -rot then ;
: pad-right ( dst u -- dst )
tuck + swap left-justify c@ if min-field-width @ swap - >r r@ 0> if
dup r@ bl fill r@ + then rdrop else drop then ;
: add-field ( dst c-addr u -- dst ) pad-left >r over r@ move r> pad-right ;
: parse-min-field-width ( src srcend -- src srcend )
over c@ '-' = dup if rot 1+ -rot then left-justify c!
over c@ '0' = if swap 1+ swap '0' else bl then pad-char c!
base @ >r decimal over - 0 -rot 0 -rot >number
rot drop rot min-field-width ! over + r> base ! ;

: parse-cmdspec ( dst src srcend -- dst src srcend )
swap 1+ swap parse-min-field-width >r dup >r c@ case
'%' of s" %" add-field endof
'c' of swap charbuf c! charbuf 1 add-field endof
'n' of swap dup s>d dabs <# #s rot sign #> add-field endof
'u' of swap 0 <# #s #> add-field endof
's' of -rot add-field endof
'd' of r> 1+ dup >r c@ case
    'n' of -rot tuck dabs <# #s rot sign #> add-field endof
    'u' of -rot <# #s #> add-field endof
endcase endof endcase r> r> ;

\ Prints n*x into buffer c-addr2 using the format string at c-addr1 u.
\ caddr2 u3 is the resulting string.
: sprintf ( n*x c-addr1 u1 c-addr2 -- caddr2 u3 )
dup >r -rot over + begin 2dup < while over c@ '%' = if parse-cmdspec else
-rot 2dup c@ swap c! -rot 1+ -rot then swap 1+ swap repeat 2drop r> tuck - ;

\ Prints n*x using the format string at c-addr u.
: printf ( n*x c-addr u -- ) pad sprintf type ;
