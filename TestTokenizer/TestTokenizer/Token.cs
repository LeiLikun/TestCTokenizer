﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTokenizer
{
    class Token
    {
        private TokenType type;
        private TokenValue value;
        private string strValue;
        public enum TokenType
        {
            Keyword,  
            Data,    //int, float
            Identifier,  //a, b
            Number, 
            String,
            Char,
            Operator,  //+, -, *
            Delimiter, //,,  , ;  
            Comment,
            EOF,
            Unknown
        }

        public enum TokenValue
        {
            //Keyword
            IF,
            FOR,
            WHILE,
            AUTO,
            STRUCT,
            ELSE,
            BREAK,
            SWITCH,
            CASE,
            ENUM,
            REGISTER,
            TYPEDEF,
            EXTERN,
            RETURN,
            UNION,
            CONST,
            UNSIGNED,
            SIGNED,
            CONTINUE,
            DEFAULT,
            GOTO,
            SIZEOF,
            VOLATILE,
            DO,
            STATIC,
            
            //Data type
            SHORT,
            INT,
            FLOAT,
            DOUBLE,
            LONG,
            CHAR,
            STRING,
            VOID,

            //Operator
            ADD,
            MINUS,
            PLUS,
            DIV,
            EQUAL,
            LEFT_BRACKET,
            RIGHT_BRACKET,
            BIGGER_THAN, //Leave >=, <= to parser
            LESS_THAN,

            //Delimiter
            COMMA,
            SEMICOLON,
            SPACE,
            END_OF_LINE,

            NUMBER_VALUE,

            IDENTIFIER_NAME,

            COMMENT,  //such as this

            EOF,

            UNKNOWN
        }

        public Token()
        { }

        public Token(TokenType type, TokenValue value, string strValue)
        {
            this.type = type;
            this.value = value;
            this.strValue = strValue;
        }

        public TokenType getType()
        {
            return this.type;
        }

        public TokenValue getValue()
        {
            return this.value;
        }
    }
}