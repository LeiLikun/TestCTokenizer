using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTokenizer
{
    class Parser
    {
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.action = new string[,] {{"s5",ERROR,ERROR,"s4",ERROR,ERROR},
                            {ERROR,"s6",ERROR,ERROR,ERROR,"acc"},
                            {ERROR,"r2","s7",ERROR,"r2","r2"},
                            {ERROR,"r4","r4",ERROR,"r4","r4"},
                            {"s5",ERROR,ERROR,"s4",ERROR,ERROR},
                            {ERROR,"r6","r6",ERROR,"r6","r6"},
                            {"s5",ERROR,ERROR,"s4",ERROR,ERROR},
                            {"s5",ERROR,ERROR,"s4",ERROR,ERROR},
                            {ERROR,"s6",ERROR,ERROR,"s11",ERROR},
                            {ERROR,"r1","s7",ERROR,"r1","r1"},
                            {ERROR,"r3","r3",ERROR,"r3","r3"},
                            {ERROR,"r5","r5",ERROR,"r5","r5"}};

            this.go = new int[,] { {1,2,3},
                                   {0,0,0},
                                   {0,0,0},
                                   {0,0,0},
                                   {8,2,3},
                                   {0,0,0},
                                   {0,9,3},
                                   {0,0,10},
                                   {0,0,0},
                                   {0,0,0},
                                   {0,0,0},
                                   {0,0,0}};

            this.terminateDic = new Dictionary<char, int>();
            this.terminateDic.Add('i',0);
            this.terminateDic.Add('+',1);
            this.terminateDic.Add('*',2);
            this.terminateDic.Add('(',3);
            this.terminateDic.Add(')',4);
            this.terminateDic.Add(';',5);

            this.gotoDic = new Dictionary<char, int>();
            this.gotoDic.Add('E',0);
            this.gotoDic.Add('T',1);
            this.gotoDic.Add('F',2);

            this.expr = new Tuple<char, string>[] { makeTuple('E',"E+T"),
                                                    makeTuple('E',"T"),
                                                    makeTuple('T',"T*F"),
                                                    makeTuple('T',"F"),
                                                    makeTuple('F',"(E)"),
                                                    makeTuple('F',"i")};
        }

        private List<Token> tokens;
        private string[,] action;
        private int[,] go;
        private Dictionary<char, int> terminateDic;
        private Dictionary<char, int> gotoDic;
        private Tuple<char, string>[] expr;
        private const string ERROR = "error";

        private Tuple<char,string> makeTuple(char first,string second)
        {
            return new Tuple<char, string>(first, second);
        }

        public void parse()
        {
            Stack<char> symbol = new Stack<char>();
            Stack<int> state = new Stack<int>();
            state.Push(0);
            symbol.Push(';');

            List<Token>.Enumerator ip = tokens.GetEnumerator();
            while (true)
            {
                char ch = getSymbolValue(ip.Current);
                if (ch != '#')
                {
                    int S = terminateDic[ch];
                    int a = state.Peek();
                    string str = action[S, a];

                    if (str.Equals(ERROR))
                    {
                        Console.WriteLine(ERROR);
                        return;
                    }
                    else if (str[0] == 's')
                    {
                        int stateNumber = str[1] - '0';
                        state.Push(stateNumber);
                        symbol.Push(ch);
                        Console.WriteLine("移进符号 "+ch);
                        ip.MoveNext();
                    }
                    else if (str[0] == 'r')
                    {
                        int k = str[1] - '0';
                        int lengthOfExp = expr[k].Item2.Length;
                        for (; lengthOfExp > 0; lengthOfExp--)
                        {
                            state.Pop();
                            Console.WriteLine("弹出 " + symbol.Pop());
                        }
                        symbol.Push(expr[k].Item1);
                        Console.WriteLine("按"+expr[k].Item1+"="+expr[k].Item2+"进行规约");
                        int stateToPush = go[state.Peek(), gotoDic[expr[k].Item1]];
                        state.Push(stateToPush);
                    }
                    else
                    {
                        Console.WriteLine("Success!");
                    }
                }
            }
        }

        private char getSymbolValue(Token token)
        {
            if (token.getType() == Token.TokenType.Number)
                return 'i';
            else if (token.getType() == Token.TokenType.Identifier)
            {
                string value = token.getStrValue();
                return value[0];
            }
            else if (token.getValue() == Token.TokenValue.COMMA)
            {
                return ';';
            }
            else
                return '#';
        }
    }
}
