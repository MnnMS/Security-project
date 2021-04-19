using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            string processedText = "";
            for(int i = 0; i < plainText.Length; i += 2)
            {
                processedText += plainText[i];
                if (plainText[i] == plainText[i + 1])
                    processedText += 'x';
                processedText += plainText[i + 1];
            }
            if (processedText.Length % 2 != 0) processedText += 'x';
            char[,] matrix = getKey(key);
            // same row 

            // same column

            // diagonal
        }

        private char[,] getKey(string keyword)
        {
            char[,] matrix = new char[5,5];
            Dictionary<char, int> alpha = new Dictionary<char, int>();
            int ind = 0;
            string uniqueText = "";
            char s = 'a';
            for(int i = 0; i < 26; i++)
            {
                alpha.Add(s, 0);
                s++;
            }
            for(int i = 0; i < keyword.Length; i++)
            {
                char curr = keyword[i];
                if (alpha[curr] == 0)
                {
                    uniqueText += curr;
                    alpha[curr] = 1;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if (ind < uniqueText.Length)
                    {
                        matrix[i, j] = uniqueText[ind];
                        ind++;
                    }
                    else
                    {
                        char c = alpha.First(x => x.Value == 0).Key;
                        matrix[i, j] = c;
                        alpha[c] = 1;
                    }
                }
            }
            return matrix;
        }
    }
}
