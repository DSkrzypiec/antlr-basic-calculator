grammar Calc;

prog: expr ;

expr returns [int exprValue]
    :   expr MUL expr   # Mul
    |   expr ADD expr   # Add
    |   INT             # Int
    ;

INT : [0-9]+ ;

MUL : '*' ;
ADD : '+' ;

NEWLINE : '\r'? '\n' ;
WS : [ \t]+ -> skip ;
