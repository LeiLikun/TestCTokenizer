using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestTokenizer
{
    class Scanner
    {
        private enum State
        { 
            NORMAL, // In this state, buffer is clear and currentChar is going to be ifed
            //NUMBER State handled by handleNumberState
            STRING,  //Including Char
            IDENTIFIER,  //Including KeyWord, DataType and Identifier
            OPERATOR,
            COMMENT,
            WRONG
        }

        public Scanner(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                this.source = sr;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File can't found!");
            }
            finally
            {
                dictionary = new Dictionary();
                buffer = String.Empty;
                state = State.NORMAL;
                error = String.Empty;
                allToken = new List<Token>();
            }
        }

        private List<Token> allToken;
        private Dictionary dictionary;
        private StreamReader source;
        private char currentChar;
        private string buffer;
        private State state;
        private string error;

        private void readNextChar()
        {
            buffer += currentChar;
            currentChar = (char)source.Read();
        }
        private char peekChar()
        {
            return (char)source.Peek();
        }
        private void clearBuffer()
        {
            buffer = String.Empty;
        }
        private void reduceBuffer(int reduceNum)
        {
            try
            {
                buffer = buffer.Substring(0, buffer.Length - reduceNum);
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public List<Token> getAllToken()
        {
            return this.allToken;
        }
        public void StateChange()
        {
            while (!source.EndOfStream)
            {
                switch (state)
                {
                    case State.NORMAL:
                        if (Char.IsDigit(currentChar))
                        {
                            handleNumberState();
                            Token numberToken = new Token(Token.TokenType.Number, Token.TokenValue.NUMBER_VALUE, buffer);
                            allToken.Add(numberToken);
                            clearBuffer();
                        }
                        else if (Char.IsLetter(currentChar))
                        {
                            state = State.IDENTIFIER;
                            //readNextChar();
                        }
                        else if (currentChar == '\"' || currentChar == '\'')
                        {
                            state = State.STRING;
                            //readNextChar();
                        }
                        else if (currentChar == '\\' && peekChar() == '\\')
                        {
                            state = State.COMMENT;
                            readNextChar();
                        }
                        else if (dictionary.isOperator(currentChar) || dictionary.isDelimiter(currentChar))
                        {
                            state = State.OPERATOR;
                            //readNextChar();
                        }
                        else
                            state = State.WRONG;
                        break;
                    case State.COMMENT:
                        readNextChar(); //read the second /
                        reduceBuffer(2); // reduce //
                        while (currentChar != '\n')
                            readNextChar();
                        //readNextChar(); //leave /n to NORMAL state
                        Token commentToken = new Token(Token.TokenType.Comment, Token.TokenValue.COMMENT, buffer);
                        allToken.Add(commentToken);
                        clearBuffer();
                        state = State.NORMAL;
                        break;
                    case State.IDENTIFIER:
                        while (!dictionary.isDelimiter(currentChar) || Char.IsLetterOrDigit(currentChar)) 
                        //TODO: bug:可能读到文件尾
                        {
                            readNextChar();
                        }
                        if (dictionary.isDataType(buffer))
                        {
                            Token.TokenValue dataTypeValue = dictionary.getDataTypeValue(buffer);
                            Token dataTypeToken = new Token(Token.TokenType.Data, dataTypeValue, buffer);
                            allToken.Add(dataTypeToken);
                            clearBuffer();
                        }
                        else if (dictionary.isKeyWord(buffer))
                        {
                            Token.TokenValue keyWordValue = dictionary.getKeyWordValue(buffer);
                            Token keyWordToken = new Token(Token.TokenType.Keyword, keyWordValue, buffer);
                            allToken.Add(keyWordToken);
                            clearBuffer();
                        }
                        else
                        {
                            Token identifierToken = new Token(Token.TokenType.Identifier, Token.TokenValue.IDENTIFIER_NAME, buffer);
                            allToken.Add(identifierToken);
                            clearBuffer();
                        }
                        //readNextChar(); //leave the delimiter to NORMAL state
                        //clearBuffer();
                        state = State.NORMAL;
                        break;
                    case State.OPERATOR:
                        if (dictionary.isOperator(currentChar))
                        {
                            Token.TokenValue operatorValue = dictionary.getOperatorValue(currentChar);
                            Token operatorToken = new Token(Token.TokenType.Operator, operatorValue, currentChar.ToString());
                            allToken.Add(operatorToken);
                            
                        }
                        else if (dictionary.isDelimiter(currentChar))
                        {
                            Token.TokenValue delimiterValue = dictionary.getDelimiterValue(currentChar);
                            Token delimiterToken = new Token(Token.TokenType.Delimiter, delimiterValue, currentChar.ToString());
                            allToken.Add(delimiterToken);
                        }
                        readNextChar(); //read the single operator/delimiter
                        clearBuffer();
                        state = State.NORMAL;
                        break;
                    case State.STRING:
                        if (currentChar == '\'')
                        {
                            readNextChar();
                            while (currentChar != '\'')
                            {
                                readNextChar();
                            }
                            //todo

                        }
                        else if (currentChar == '\"')
                        { 
                            
                        }
                        break;
                    case State.WRONG:
                        break;
                }

                currentChar = (char)source.Read();
            }
        }

        enum NumberState //Seeing Compiler principles and techniques P95
        { 
            STATE1,
            STATE2,
            STATE4,
            STATE5,
            STATE6,
            WRONG
        }
        private void handleNumberState()
        {
            bool matched = false;
            NumberState numberState = NumberState.STATE1;
            while (!matched && !source.EndOfStream)
            {
                switch (numberState)
                { 
                    case NumberState.STATE1:
                        while (Char.IsDigit(currentChar))
                        {
                            readNextChar();
                        }
                        if (currentChar == '.')
                        {
                            numberState = NumberState.STATE2;
                            readNextChar();
                        }
                        else if (currentChar == 'E' || currentChar == 'e')
                        {
                            numberState = NumberState.STATE4;
                            readNextChar();
                        }
                        else //Leave some error conditions to parser
                        {
                            matched = true;
                        }
                        break;
                    case NumberState.STATE2:
                        while (Char.IsDigit(currentChar))
                        {
                            readNextChar();
                        }
                        if (currentChar == 'E' || currentChar == 'e')
                        {
                            numberState = NumberState.STATE4;
                            readNextChar();
                        }
                        else
                            matched = true;
                        break;
                    case NumberState.STATE4:
                        if (currentChar == '+' || currentChar == '-')
                        {
                            numberState = NumberState.STATE5;
                            readNextChar();
                        }
                        else if (Char.IsDigit(currentChar))
                        {
                            numberState = NumberState.STATE6;
                            readNextChar();
                        }
                        else
                        {
                            numberState = NumberState.WRONG;
                            reduceBuffer(1);
                        }
                        break;
                    case NumberState.STATE5:
                        if (Char.IsDigit(currentChar))
                        {
                            numberState = NumberState.STATE6;
                            readNextChar();
                        }
                        else
                        {
                            numberState = NumberState.WRONG;
                            reduceBuffer(2);
                        }
                        break;
                    case NumberState.STATE6:
                        while (Char.IsDigit(currentChar))
                        {
                            readNextChar();
                        }
                        matched = true;
                        break;
                    case NumberState.WRONG:
                        error = "Illegal number form";
                        break;
                }
            }
            if (numberState != NumberState.WRONG)
                state = State.NORMAL;
            else
                state = State.WRONG;
        }

    }
}
