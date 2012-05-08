using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace OdinsRevenge
{
    class ScoreManagement
    {

        private const int MAX = 1000;

        public Dictionary<String, int> ReadScores()
        {
            
            Dictionary<String, int> fileDic = new Dictionary<String, int>();
            string[] file = new string[MAX]; 
            string line;
            int count = 0;
            StreamReader filereader = new StreamReader("scores.txt");
            line = filereader.ReadLine();

            while (line != null)
            {
                file[count] = line;
                count++;
                line = filereader.ReadLine();
            }
            filereader.Close();
            fileDic = SortScores(file, count); 
            return fileDic;
        }

        private Dictionary<String, int> SortScores(string[] scores, int count)
        {   
            string[] split;
            
            Dictionary<String, int> scoreDic = new Dictionary<string,int>();
            Dictionary<String, int> tempDic = new Dictionary<string, int>();

            for (int i = 0; i < count ; i++)
            {
                split = scores[i].Split(' ');
                scoreDic.Add(split[0] + i.ToString(), int.Parse(split[1])); 
            }
            
            tempDic = scoreDic.OrderByDescending(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return (Dictionary<String, int>)tempDic;
        }

        public void WriteScore(string score)
        {
            StreamWriter swriter = File.AppendText("scores.txt");
            swriter.WriteLine("\n" + score);
            swriter.Close();
        }
    }
}

