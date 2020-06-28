using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class Question1
    {
        public class LetterFrequency {
            public char Letter { get; set; }
            public int Frequency { get; set; }
        }

        public int Task1(string S)
        {
            var res1 = (from inp in S.ToCharArray()
                        group inp by inp into grp1
                        orderby grp1.Count() descending
                        select new LetterFrequency { Letter = grp1.Key, Frequency = grp1.Count() }
                       ).ToList();

            int removeLetterCount = 0;

            if (res1.Count == 1)
            {
                return removeLetterCount;
            }

            
            for (int i = 1; i < res1.Count; i++)
            {
                var curItemCount = res1[i].Frequency;
                var prevItemCount = res1[i - 1].Frequency;

                if (curItemCount >= prevItemCount)
                {
                    do
                    {
                        removeLetterCount++;
                        res1[i].Frequency -= 1;
                        curItemCount = res1[i].Frequency;
                    } while (curItemCount >= prevItemCount && (prevItemCount > 0 || curItemCount > 0));
                }
            }


            return removeLetterCount;
        }
    }
}
