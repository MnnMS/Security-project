using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<List<int>> lists = new List<List<int>>();
        Random rand = new Random();
        public List<int> Analyse(string plainText, string cipherText)
        {
            int count = 2;
            List<int> Key = new List<int>();
            while(count< plainText.Length-1)
            {
                
                int f = Fctorial(count);
                while (f > 0)
                {
                    Key = GetPermutations(count);
                   
                    if (Encrypt(plainText, Key) == cipherText.ToLower())
                        return Key;
                    f--;
                }
                count++;
                lists.Clear();


            }
            return Key;
           
        }

        public int Fctorial(int c)
        {
            int f = 1;
            for (int i = 1; i <= c; i++)
                f *= i;
            return f;
        }
        public List<int> GetPermutations(int count)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < count; i++)
            {
               
                int num = rand.Next(1, count + 1);
                while (result.Contains(num))
                {
                    num = rand.Next(1, count + 1);
                }
                result.Add(num);
            }
            List<int> l = lists.Find(list => list.SequenceEqual(result) == true);
            if (l != null) { 
                GetPermutations(count);
            }
            if(l==null)
               lists.Add(result);
            return lists.Last();
        }
        public string Decrypt(string cipherText, List<int> key)
        {
            int rowLength = key.Count;
            int ctLength = cipherText.Length;
            int columnLength = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ctLength) / rowLength));
            int cellNotHaveValue = (rowLength * columnLength) - ctLength;
            char[] plainText = new char[ctLength];
            int planIndex = 0;
            int index;
            for(int i =0;i< rowLength; i++)
            {     
                int j = 0;
                index = (key[i]-1) * columnLength;
                if (rowLength - i <= cellNotHaveValue)
                {
                    cipherText=cipherText.Insert(index+columnLength-1, "*");
                }
                planIndex = i;
                while (j < columnLength) {
                    if ( index>= cipherText.Length|| planIndex>= ctLength|| cipherText[index] == '*')
                        break;
                    plainText[planIndex] +=cipherText[index];
                    index++;
                    planIndex += rowLength;
                    j++;
                }

            }
            return new string(plainText);
        }

        public string Encrypt(string plainText, List<int> key)
        {
            int rowLength = key.Count;
            double ptLength = plainText.Length;
            int columnLength = Convert.ToInt32(Math.Ceiling(ptLength / rowLength));

            string cipherText = string.Empty;

            for (int i = 0; i < rowLength; i++)
            {
                int j = 0;
                int index =key.IndexOf(i+1);
                while (j < columnLength)
                {
                    if (index >= ptLength)
                        break;
                    cipherText += plainText[index];
                    index += rowLength;
                    j++;
                }
            }


            return cipherText;
        }
    }
}
