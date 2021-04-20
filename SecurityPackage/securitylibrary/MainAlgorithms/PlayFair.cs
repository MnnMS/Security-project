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
            cipherText = cipherText.ToLower();
            char[,] matrix = getKey(key);
            string result = string.Empty;
            for (int i = 0; i<cipherText.Length; i += 2)
            {
                int[] indexesForChar1 = findIndexes(cipherText[i], matrix);
                int[] indexesForChar2 = findIndexes(cipherText[i+1], matrix);
                //same row 
                if(indexesForChar1[0] == indexesForChar2[0])
                {
                    if (indexesForChar1[1] == 0) result += matrix[indexesForChar1[0], 4];
                    else result += matrix[indexesForChar1[0], indexesForChar1[1]-1];

                    if (indexesForChar2[1] == 0) result += matrix[indexesForChar2[0], 4];
                    else result += matrix[indexesForChar2[0], indexesForChar2[1] - 1];
                }

                //same column
                else if(indexesForChar1[1] == indexesForChar2[1])
                {
                    if (indexesForChar1[0] == 0) result += matrix[4, indexesForChar1[1]];
                    else result += matrix[indexesForChar1[0]-1, indexesForChar1[1]];

                    if (indexesForChar2[0] == 0) result += matrix[4, indexesForChar2[1]];
                    else result += matrix[indexesForChar2[0]-1, indexesForChar2[1]];
                }

                //diagonal

                else
                {
                    result += matrix[indexesForChar1[0], indexesForChar2[1]];
                    result += matrix[indexesForChar2[0], indexesForChar1[1]];
                }
                
                

            }
            if (result[result.Length - 1] == 'x')
                result = result.Remove(result.Length - 1, 1);


            for (int i = 1; i < result.Length; i++)
                if (result[i] == 'x' && result[i - 1] == result[i + 1]) { 
                    result = result.Remove(i, 1);
                }

            return result;

        }

        public string Encrypt(string plainText, string key)
        {
            string result = string.Empty;
            string processedText = "";
            for(int i = 0; i+1 < plainText.Length; i += 2)
            {
                processedText += plainText[i];
                if (plainText[i] == plainText[i + 1])
                    processedText += 'x';
                processedText += plainText[i + 1];
            }
            if (processedText.Length % 2 != 0) processedText += 'x';
            char[,] matrix = getKey(key);

            //same row 

            //same column

            //diagonal

            return "";
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
                if (s == 'j')
                    alpha['j'] = 1;
                s++;
            }
            for(int i = 0; i < keyword.Length; i++)
            {
                char curr = keyword[i];
                if (curr == 'j') curr = 'i';
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
                    char c;
                    if (ind < uniqueText.Length)
                    {
                        c = uniqueText[ind];
                        ind++;
                    }
                    else
                    {
                        c = alpha.First(x => x.Value == 0).Key;
                        alpha[c] = 1;
                    }
                    matrix[i, j] = c;
                }
            }
            return matrix;
        }

        private int[] findIndexes(char c , char[,] matrix)
        {
            int[] arr=new int[2];
            for(int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                    if (c == matrix[i, j])
                    {
                        arr[0] = i;
                        arr[1] = j;
                        break;
                    }    
            }
            return arr;

        }

    }
}
