using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using LemmaSharp;

namespace Clasterization
{
    class Stemmer
    {
        public string[] ReadDocument()
        {
            var path = @"D:\prog\Data\news\news_train.txt";
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                return lines;
            }
        }

        public void RefactorLines(string[] lines)
        {
            List<string> newLines = new List<string>();
            foreach (var line in lines)
            {
                var str = line;
                str = str.Replace("Ё", "Е")
                    .Replace("ё", "е")
                    .Replace(".", " ")
                    .Replace(";", " ")
                    .Replace(",", " ");
                    //#.Replace("\ufeff", "");
                str = Regex.Replace(str, @"\s+", " ");
                str = str.ToLower();
                newLines.Add(str);
            }
            string[] stoplist = new string[]
            {
                "из", "из", "большой", "бы", "быть", "весь", "вот", "все",
                "всей", "вы", "говорить", "год", "да", "для", "до", "еще",
                "же", "знать", "и", "из", "к", "как", "который", "мочь",
                "мы", "на", "наш", "не", "него", "нее", "нет", "них", "но",
                "о", "один", "она", "они", "оно", "оный", "от", "ото", "по",
                "с", "свой", "себя", "сказать", "та", "такой", "только", "тот",
                "ты", "у", "что", "это", "этот", "я", "без", "более", "больше",
                "будет", "будто", "бы", "был", "была", "были", "было", "быть",
                "вам", "вас", "ведь", "весь", "вдоль", "вдруг", "вместо",
                "вне", "вниз", "внизу", "внутри", "во", "вокруг", "вот",
                "впрочем", "все", "всегда", "всего", "всех", "всю", "вы",
                "где", "да", "давай", "давать", "даже", "для", "до",
                "достаточно", "другой", "его", "ему", "ее", "её", "ей", "если",
                "есть", "ещё", "еще", "же", "за", "здесь",
                "из", "изза", "из", "или", "им", "иметь", "иногда", "их",
                "както", "кто", "когда", "кроме", "кто", "куда", "ли", "либо",
                "между", "меня", "мне", "много", "может", "мое", "моё", "мои",
                "мой", "мы", "на", "навсегда", "над", "надо", "наконец", "нас",
                "наш", "не", "него", "неё", "нее", "ней", "нет", "ни",
                "нибудь", "никогда", "ним", "них", "ничего", "но", "ну", "об",
                "однако", "он", "она", "они", "оно", "опять", "от", "отчего",
                "очень", "перед", "по", "под", "после", "потом", "потому",
                "потому что", "почти", "при", "про", "раз", "разве", "свою",
                "себя", "сказать", "снова", "с", "со", "совсем", "так", "также",
                "такие", "такой", "там", "те", "тебя", "тем", "теперь",
                "то", "тогда", "того", "тоже", "той", "только", "том", "тот",
                "тут", "ты", "уже", "хоть", "хотя", "чего", "чегото", "чей",
                "чем", "через", "что", "чтото", "чтоб", "чтобы", "чуть",
                "чьё", "чья", "эта", "эти", "это", "эту", "этого", "этом",
                "этот", "к", "около", "будут", "нас", "нам", "например",
                "пока", "чаще", "to", "other", "you", "is", "was", "were",
                "the", "того", "которые", "то", "свое", "сами", "можно",
                "всем", "этому", "сколько"
            };
            for (int i = 0; i < newLines.Count; i++)
            {
                foreach (var word in stoplist)
                {
                    newLines[i] = newLines[i].Replace(" " + word + " ", " ");
                }
            }
            File.WriteAllLines(@"D:\refactoredTest.txt", newLines);
        }

        public void addTabs()
        {
            var lines = File.ReadAllLines(@"D:\refactoredTest.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var regex = new Regex(Regex.Escape(" "));
                line = regex.Replace(line, "\t", 1);
                lines[i] = line;
            }
            File.WriteAllLines(@"D:\refactoredTest2.txt", lines);
        }

        public void lemmByString(string[] lines)
        {
            ILemmatizer lmtz = new LemmatizerPrebuiltCompact(LemmaSharp.LanguagePrebuilt.Russian);
            for (int i = 0; i < lines.Length; i++)
            {
                var tempLine = lines[i].Split('\t')[1];
                var words = tempLine.Split(
                                            new char[] { ' ', ',', '.', ')', '(', '\"', '\'', '»', '«'}, 
                                            StringSplitOptions.RemoveEmptyEntries
                 ); 
                for (int j = 0; j < words.Length; j++)
                {
                    words[j] = lmtz.Lemmatize(words[j]);
                }
                lines[i] = lines[i].Split('\t')[0] + "\t" + string.Join(" ", words);
                //lines[i] = string.Join(" ", words);
                double percent = i*100/60000;
                Console.WriteLine("{0}%", percent);
            }
            File.WriteAllLines(@"D:\lemm.txt", lines);
        }

        public void category(string[] lines)
        {
            int j = 0;
            string[] categories = new string[]
            {
                "science", "style", "culture", "life", "economics", "business", "travel", "forces", "media", "sport"
            };
            foreach (var cat in categories)
            {
                j++;
                List<string> newLines = new List<string>();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Split('\t')[0] == cat)
                    {
                        newLines.Add(lines[i].Split('\t')[1]);
                    }
                }
                File.WriteAllLines(@"D:\news\themes\" + cat + ".txt", newLines);
                double percent = j*100/categories.Length;
                Console.WriteLine(percent);
            }
        }
    }
}
