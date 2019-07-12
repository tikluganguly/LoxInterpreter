using System;
using System.Collections.Generic;
using System.Text;

namespace LoxInterpreter.Interpreters
{
    public enum TokenType
    {
        //single char token
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,

        //one or two char token
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GRETER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        //literals
        IDENTIFIER, STRING, NUMBER,

        //keywords
        AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
        PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

        EOF

    }
}
