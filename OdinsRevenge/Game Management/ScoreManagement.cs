using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace OdinsRevenge
{
    class ScoreManagement
    {

        private const int MAX = 1000;

        public string[] ReadScores()
        {
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

            return file;
        }

        private string[] SortScores(string[] scores)
        {
            string[] score = new string[MAX];
            string temp;
            int score1;
            int score2;
            string[] split1;

            score = scores; 
            return score;
        }
    }
}
