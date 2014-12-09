using System;
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
            LEFT_BRACE,
            RIGHT_BRACE,
            COMMA,
            SEMICOLON,
            SPACE,
            END_OF_LINE_R,
            END_OF_LINE_N, //which means it's running on Windows
            TAB,
            EOF,

            NUMBER_VALUE,

            IDENTIFIER_NAME,

            COMMENT,  //such as this

            STRING_CONTENT,
            CHAR_CONTENT,

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

        public string getStrValue()
        {
            return this.strValue;
        }

        public string outPutToken()
        {
            return "Token type: " + this.type + "\t Token value: " + this.value + "\t Token content: " + this.strValue;
        }
    }
}
