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

            List<Token>.Enumerator ip = tokens.GetEnumerator();
            ip.MoveNext();
            while (true)
            {
                char ch = getSymbolValue(ip.Current); //current char
                if (ch != '#')
                {
                    int S = state.Peek();
                    int a = terminateDic[ch];
                    string str = action[S, a];

                    if (str.Equals(ERROR))
                    {
                        Console.WriteLine(ERROR);
                        return;
                    }
                    else if (str[0] == 's')
                    {
                        int stateNumber = Int32.Parse(str.Substring(1,str.Length-1));
                        state.Push(stateNumber);
                        symbol.Push(ch);
                        Console.WriteLine("移进符号 " + ch);
                        ip.MoveNext();
                    }
                    else if (str[0] == 'r')
                    {
                        int k = str[1] - '0';
                        int lengthOfExp = expr[k-1].Item2.Length;
                        for (; lengthOfExp > 0; lengthOfExp--)
                        {
                            state.Pop();
                            Console.WriteLine("弹出 " + symbol.Pop());
                        }
                        symbol.Push(expr[k-1].Item1);
                        Console.WriteLine("按" + expr[k-1].Item1 + "->" + expr[k-1].Item2 + "进行规约");
                        Console.WriteLine("压入 "+expr[k-1].Item1);
                        int stateToPush = go[state.Peek(), gotoDic[expr[k-1].Item1]];
                        state.Push(stateToPush);
                    }
                    else
                    {
                        Console.WriteLine("Success!");
                        return;
                    }
                    Console.WriteLine("===========================");
                }
                else
                    ip.MoveNext();
            }
        }

        private char getSymbolValue(Token token)
        {
            if (token.getType() == Token.TokenType.Number)
                return 'i';
            else if (token.getType() == Token.TokenType.Operator)
            {
                string value = token.getStrValue();
                return value[0];
            }
            else if (token.getValue() == Token.TokenValue.SEMICOLON)
            {
                return ';';
            }
            else
                return '#';
        }
    }
}
