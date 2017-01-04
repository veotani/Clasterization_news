using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Clasterization
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"D:\news\news_train.txt");
            var linesToClas = File.ReadAllLines(@"D:\news\news_test.txt");
            Clasterizator.Clasterizate(lines, linesToClas);
            Console.ReadKey();
        }
    }
}