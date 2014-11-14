using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestTokenizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner scanner = new Scanner("test.c");
            scanner.StateChange();
            List<Token> list = scanner.getAllToken();
            foreach (Token token in list)
            {
                Console.WriteLine(token.outPutToken());
            }
            Console.ReadKey();
        }
    }
}
