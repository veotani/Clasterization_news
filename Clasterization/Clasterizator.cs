using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Clasterization
{
    class Clasterizator
    {
        public static void Clasterizate(string[] lines, string[] toClaster)
        {
            List<string> res = new List<string>();

            int D = lines.Length;

            Dictionary<string, int> uniqueWords = new Dictionary<string, int>();
            int ji = 0;
            foreach (var line in lines)
            {
                var words = line.Split(' ');
                foreach (var word in words)
                {
                    if (uniqueWords.ContainsKey(word))
                    {
                        uniqueWords[word]++;
                    }
                    else
                    {
                        uniqueWords[word] = 1;
                    }
                }
                Console.WriteLine(++ji);
            }
            int V = uniqueWords.Count - 10;


                string[] categories = new string[]
            {
                "science", "style", "culture", "life", "economics", "business", "travel", "forces", "media", "sport"
            };


            Dictionary<string, int> Dc = new Dictionary<string, int>();
            Dictionary<string, int> Lc = new Dictionary<string, int>();

            foreach (var category in categories)
            {
                var linesCat = File.ReadAllLines(@"D:\news\themes\" + category + ".txt");
                Dc[category] = linesCat.Length;

                Lc[category] = 0;
                foreach (var lineCat in linesCat)
                {
                    Lc[category] += lineCat.Split(' ').Length;
                }
            }


            /*int j = 0;
            foreach (var str in toClaster)
            {
                j++;
                Dictionary<string, double> percantage = new Dictionary<string, double>();
                foreach (var category in categories)
                {
                    percantage[category] = Math.Log(Dc[category]/D);
                    foreach (var word in str.Split(' '))
                    {
                        int Wic = 0;
                        var linesCat = File.ReadAllLines(@"D:\news\themes\" + category + ".txt");
                        foreach (var lineCat in linesCat)
                        {
                            var matchQuery = from searchWord in lineCat.Split(' ')
                                             where searchWord == word
                                             select searchWord;
                            Wic += matchQuery.Count();
                        }
                        percantage[category] += Math.Log(Wic + 1/(V + Lc[category]));
                    }
                }
                var max = percantage.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                res.Add(max);
                double perc = j*100/toClaster.Length;
                Console.WriteLine("{0}%", perc);
            }
        }*/


            Dictionary<string, Dictionary<string, int>> bigCounter = new Dictionary<string, Dictionary<string, int>>();
            foreach (var category in categories)
            {
                var linesCat = File.ReadAllLines(@"D:\news\themes\" + category + ".txt");
                Dictionary<string, int> countWords = new Dictionary<string, int>();
                foreach (var line in linesCat)
                {
                    foreach (var word in line.Split(' '))
                    {
                        if (countWords.ContainsKey(word))
                        {
                            countWords[word] += 1;
                        }
                        else
                        {
                            countWords[word] = 1;
                        }
                    }
                }
                bigCounter[category] = countWords;
            }

            int j = 0;
            foreach (var str in toClaster)
            {
                j++;
                //Dictionary<string, double> percantage = new Dictionary<string, double>();
                double maxPercent = Double.MinValue;
                string categ = categories[0];
                foreach (var cat in categories)
                {
                    //percantage[cat] = Math.Log(Dc[cat] / D);
                    double percent = Math.Log((double)Dc[cat] / D);
                    int Wic = 0;
                    string[] strings = str.Split(' ');
                    foreach (var word in strings)
                    {
                        if (bigCounter[cat].ContainsKey(word))
                            Wic = bigCounter[cat][word];
                        else Wic = 0;
                        double eval = (double)(Wic + 1)/(V + Lc[cat]);
                        percent += Math.Log(eval);
                        //percantage[cat] += Math.Log(((float)Wic + 1)/(V + Lc[cat]));
                    }
                    if (percent > maxPercent)
                    {
                        maxPercent = percent;
                        categ = cat;
                    }
                }
                //var max = percantage.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                res.Add(categ);
                //Console.WriteLine("{0}%", perc);
            }
            File.WriteAllLines(@"D:\news\result.txt", res);

        }
    }
}
