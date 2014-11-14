using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTokenizer
{
    class Dictionary
    {
        
        private Dictionary<String, Token.TokenValue> dataTypeDictionary;
        private Dictionary<String, Token.TokenValue> keyWordDictionary;
        private Dictionary<Char, Token.TokenValue> operatorValueDictionary;
        private Dictionary<Char, Token.TokenValue> delimiterValueDictionary;

        public Dictionary()
        {
            dataTypeDictionary = new Dictionary<string,Token.TokenValue>();
            operatorValueDictionary = new Dictionary<char,Token.TokenValue>();
            delimiterValueDictionary = new Dictionary<char, Token.TokenValue>();
            keyWordDictionary = new Dictionary<string, Token.TokenValue>();
            addDataType();
            addOperator();
            addDelimiter();
        }

        private void addKeyWord()
        {
            keyWordDictionary.Add("if",Token.TokenValue.IF);
            keyWordDictionary.Add("for", Token.TokenValue.FOR);
            keyWordDictionary.Add("while", Token.TokenValue.WHILE);
            keyWordDictionary.Add("auto", Token.TokenValue.AUTO);
            keyWordDictionary.Add("struct", Token.TokenValue.STRUCT);
            keyWordDictionary.Add("else", Token.TokenValue.ELSE);
            keyWordDictionary.Add("break", Token.TokenValue.BREAK);
            keyWordDictionary.Add("switch", Token.TokenValue.SWITCH);
            keyWordDictionary.Add("case", Token.TokenValue.CASE);
            keyWordDictionary.Add("enum", Token.TokenValue.ENUM);
            keyWordDictionary.Add("register", Token.TokenValue.REGISTER);
            keyWordDictionary.Add("typedef", Token.TokenValue.TYPEDEF);
            keyWordDictionary.Add("extern", Token.TokenValue.EXTERN);
            keyWordDictionary.Add("return", Token.TokenValue.RETURN);
            keyWordDictionary.Add("union", Token.TokenValue.UNION);
            keyWordDictionary.Add("const", Token.TokenValue.CONST);
            keyWordDictionary.Add("unsigned", Token.TokenValue.UNSIGNED);
            keyWordDictionary.Add("signed", Token.TokenValue.SIGNED);
            keyWordDictionary.Add("continue", Token.TokenValue.CONTINUE);
            keyWordDictionary.Add("default", Token.TokenValue.DEFAULT);
            keyWordDictionary.Add("goto", Token.TokenValue.GOTO);
            keyWordDictionary.Add("sizeof", Token.TokenValue.SIZEOF);
            keyWordDictionary.Add("volatile", Token.TokenValue.VOLATILE);
            keyWordDictionary.Add("do", Token.TokenValue.DO);
            keyWordDictionary.Add("static", Token.TokenValue.STATIC);
        }

        private void addDataType()
        {
            dataTypeDictionary.Add("int", Token.TokenValue.INT);
            dataTypeDictionary.Add("double", Token.TokenValue.DOUBLE);
            dataTypeDictionary.Add("float", Token.TokenValue.FLOAT);
            dataTypeDictionary.Add("long", Token.TokenValue.LONG);
            dataTypeDictionary.Add("char", Token.TokenValue.CHAR);
            dataTypeDictionary.Add("string", Token.TokenValue.STRING);
            dataTypeDictionary.Add("void", Token.TokenValue.VOID);
        }

        private void addOperator()
        {
            operatorValueDictionary.Add('+', Token.TokenValue.ADD);
            operatorValueDictionary.Add('/', Token.TokenValue.DIV);
            operatorValueDictionary.Add('=', Token.TokenValue.EQUAL);
            operatorValueDictionary.Add('(', Token.TokenValue.LEFT_BRACKET);
            operatorValueDictionary.Add(')', Token.TokenValue.RIGHT_BRACKET);
            operatorValueDictionary.Add('*', Token.TokenValue.PLUS);
            operatorValueDictionary.Add('-', Token.TokenValue.MINUS);
            operatorValueDictionary.Add('>', Token.TokenValue.BIGGER_THAN);
            operatorValueDictionary.Add('<', Token.TokenValue.LESS_THAN);
        }

        private void addDelimiter()
        {
            delimiterValueDictionary.Add(';', Token.TokenValue.SEMICOLON);
            delimiterValueDictionary.Add(' ', Token.TokenValue.SPACE);
            delimiterValueDictionary.Add(',', Token.TokenValue.COMMA);
            delimiterValueDictionary.Add('\r', Token.TokenValue.END_OF_LINE_R);
            delimiterValueDictionary.Add('\n', Token.TokenValue.END_OF_LINE_N);
            //delimiterValueDictionary.Add((char)(-1), Token.TokenValue.EOF);
        }
        public bool isKeyWord(string str)
        {
            return keyWordDictionary.ContainsKey(str);
        }
        public bool isDataType(string str)
        {
            return dataTypeDictionary.ContainsKey(str);
        }

        public bool isOperator(char ch)
        {
            return operatorValueDictionary.ContainsKey(ch);
        }

        public bool isDelimiter(char ch)
        {
            return delimiterValueDictionary.ContainsKey(ch);
        }

        public Token.TokenValue getKeyWordValue(string key)
        {
            Token.TokenValue value;
            keyWordDictionary.TryGetValue(key, out value);
            return value;
        }
        public Token.TokenValue getDataTypeValue(string key)
        {
            Token.TokenValue value;
            dataTypeDictionary.TryGetValue(key, out value);
            return value;
        }
        public Token.TokenValue getOperatorValue(char ch)
        {
            Token.TokenValue value;
            operatorValueDictionary.TryGetValue(ch, out value);
            return value;
        }
        public Token.TokenValue getDelimiterValue(char ch)
        {
            Token.TokenValue value;
            delimiterValueDictionary.TryGetValue(ch, out value);
            return value;
        }
    }
}
