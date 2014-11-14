using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTokenizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary dic = new Dictionary();
            List<Token> all_tokens = new List<Token>();

            String dest = "int a = 98 , b = 560;";
            String[] lines = dest.Split(';');

            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Equals(""))
                {
                    String[] tokens = lines[i].Split(' ');
                    for (int j = 0; j < tokens.Length; j++)
                    {
                        String token = tokens[j];

                        char first_char = token[0];
                        if (Char.IsDigit(first_char))
                        {
                            all_tokens.Add(new Token(Token.TokenType.Number, Token.TokenValue.NUMBER));
                        }
                        else
                        {
                            if (Char.IsLetter(first_char))
                            {
                                if (dic.isDataType(token))
                                {
                                    Token.TokenValue value = dic.getDataTypeValue(token);
                                    all_tokens.Add(new Token(Token.TokenType.Data, value));
                                }
                                else
                                {
                                    all_tokens.Add(new Token(Token.TokenType.Identifier, Token.TokenValue.IDENTIFIER_NAME));
                                }
                            }
                            else if (token.Length == 1)
                            {
                                if (dic.isOperator(token))
                                {
                                    Token.TokenValue value = dic.getOperatorValue(token);
                                    all_tokens.Add(new Token(Token.TokenType.Operator, value));
                                }
                                else
                                    all_tokens.Add(new Token(Token.TokenType.Unknown, Token.TokenValue.UNKNOWN));
                            }
                            else
                                all_tokens.Add(new Token(Token.TokenType.Unknown, Token.TokenValue.UNKNOWN));
                        }
                    }
                }
            }
            Output(all_tokens);
            Console.ReadKey();
        }

        public static void Output(List<Token> all_tokens)
        {
            for (int i = 0; i < all_tokens.Count; i++)
            {
                Token token = all_tokens[i];
                switch (token.getType())
                {
                    case Token.TokenType.Data:
                        Console.WriteLine(i + " DataType");
                        break;
                    case Token.TokenType.Identifier:
                        Console.WriteLine(i + " Identifier");
                        break;
                    case Token.TokenType.Number:
                        Console.WriteLine(i + " Number");
                        break;
                    case Token.TokenType.Operator:
                        Console.WriteLine(i + " Operator");
                        break;
                    case Token.TokenType.Unknown:
                        Console.WriteLine(i + " Unknow");
                        break;
                }

            }
        }
    }
}
