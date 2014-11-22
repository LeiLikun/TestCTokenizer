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
        private const int NAN = -999;

        private Tuple<char,string> makeTuple(char first,string second)
        {
            return new Tuple<char, string>(first, second);
        }

        private struct mark
        {
            public char ch;
            public int value;
        }

        public void parse()
        {
            Stack<mark> symbol = new Stack<mark>();
            Stack<int> state = new Stack<int>();
            state.Push(0);

            List<Token>.Enumerator ip = tokens.GetEnumerator();
            ip.MoveNext();
            while (true)
            {
                mark m = getSymbolValue(ip.Current); //current mark
                if (m.ch != '#')
                {
                    int S = state.Peek();
                    int a = terminateDic[m.ch];
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
                        symbol.Push(m);
                        Console.WriteLine("移进符号 " + m.ch);
                        ip.MoveNext();
                    }
                    else if (str[0] == 'r')
                    {
                        int k = str[1] - '0';
                        int lengthOfExp = expr[k-1].Item2.Length;
                        mark pushMark = new mark();

                        if (lengthOfExp == 1)
                        {
                            state.Pop();
                            mark popMark = symbol.Pop();
                            Console.WriteLine("弹出 " + popMark.ch);
                            pushMark.value = popMark.value;
                        }
                        else if (lengthOfExp == 3)
                        {
                            if (expr[k - 1].Item2[0] == '(')
                            {
                                symbol.Pop();
                                mark popMark = symbol.Pop();
                                symbol.Pop();
                                Console.WriteLine("弹出 (E)");
                                pushMark.value = popMark.value;
                            }
                            else if (expr[k - 1].Item2[1] == '+')
                            {
                                mark popMark1 = symbol.Pop();
                                symbol.Pop();
                                mark popMark2 = symbol.Pop();
                                Console.WriteLine("弹出 E+T");
                                pushMark.value = popMark1.value + popMark2.value;
                            }
                            else
                            {
                                mark popMark1 = symbol.Pop();
                                symbol.Pop();
                                mark popMark2 = symbol.Pop();
                                Console.WriteLine("弹出 T*F");
                                pushMark.value = popMark1.value * popMark2.value;
                            }
                            for (int i = 0; i < 3; i++) state.Pop();
                        }
                        pushMark.ch = expr[k - 1].Item1;
                        symbol.Push(pushMark);

                        Console.WriteLine("按" + expr[k-1].Item1 + "->" + expr[k-1].Item2 + "进行规约");
                        Console.WriteLine("压入 "+expr[k-1].Item1 + " 值为 " + pushMark.value);
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

        private mark getSymbolValue(Token token)
        {
            mark m = new mark();
            if (token.getType() == Token.TokenType.Number)
            {
                m.ch = 'i';
                m.value = Int32.Parse(token.getStrValue());
            }
            else if (token.getType() == Token.TokenType.Operator)
            {
                m.ch = token.getStrValue()[0];
                m.value = NAN;
            }
            else if (token.getValue() == Token.TokenValue.SEMICOLON)
            {
                m.ch = ';';
                m.value = NAN;
            }
            else
            {
                m.ch = '#';
                m.value = NAN;
            }
            return m;
        }
    }
}
