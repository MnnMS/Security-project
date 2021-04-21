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

            string result = mainLoop(-1, cipherText, key);

            int l = result.Length;

            string newResult = string.Empty;

            for (int i = 0; i < l-2; i+=2)
                if (result[i+1] == 'x' && result[i] == result[i+2]) {
                    newResult += result[i];
                }
                else
                {
                    newResult += result.Substring(i,2);
                }
            newResult += result.Substring(l-2, 2);

            result = newResult;

            if (result[result.Length - 1] == 'x')
                result = result.Remove(result.Length - 1, 1);

            return result;
        }

        public string Encrypt(string plainText, string key)
        {
            string result = string.Empty;
            string processedText = "";
            
            if (plainText.Length % 2 != 0) plainText += 'x';

            for (int i = 0; i < plainText.Length; i+=2)
            {
                processedText += plainText[i];
                if (plainText[i] == plainText[i + 1])
                    processedText += 'x';
                processedText += plainText[i + 1];
            }
            if(processedText.Length%2!=0 && processedText[processedText.Length-1]=='x')
                processedText = processedText.Remove(processedText.Length - 1, 1);
            else if(processedText.Length % 2 != 0)
                processedText += 'x';

            return mainLoop(1, processedText, key);
        }

        private string mainLoop(int flag, string text, string key)
        {
            char[,] matrix = getKey(key);
            string result = "";
            for (int i = 0; i < text.Length; i += 2)
            {
                int[] indexesForChar1 = findIndexes(text[i], matrix);
                int[] indexesForChar2 = findIndexes(text[i + 1], matrix);

                //same row 
                if (indexesForChar1[0] == indexesForChar2[0])
                {
                    int index1 = indexesForChar1[1] + 1*flag;
                    int index2 = indexesForChar2[1] + 1*flag;
                    if (index1 == -1) index1 = 4;
                    if (index2 == -1) index2 = 4;
                    result += matrix[indexesForChar1[0], index1%5];
                    result += matrix[indexesForChar2[0], index2%5];
                }

                //same column
                else if (indexesForChar1[1] == indexesForChar2[1])
                {
                    int index1 = indexesForChar1[0] + 1 * flag;
                    int index2 = indexesForChar2[0] + 1 * flag;
                    if (index1 == -1) index1 = 4;
                    if (index2 == -1) index2 = 4;
                    result += matrix[index1%5, indexesForChar1[1]];
                    result += matrix[index2%5, indexesForChar2[1]];
                }

                //diagonal
                else
                {
                    result += matrix[indexesForChar1[0], indexesForChar2[1]];
                    result += matrix[indexesForChar2[0], indexesForChar1[1]];
                }
            }
            return result;
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

        private int[] findIndexes(char c, char[,] matrix)
        {
            int[] arr = new int[2];
            for (int i = 0; i < 5; i++)
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
