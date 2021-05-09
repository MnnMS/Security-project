using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string GetKey(string key, string plainText)
        {
            int lenOfKey = key.Length;
            int lenOfPt = plainText.Length;
            int n = lenOfPt - lenOfKey;
            int ind = 0;

            while (n > 0)
            {
                key = key.Insert(key.Length, key[ind].ToString());
                n--;
                ind = (ind == lenOfKey - 1) ? 0 : ind += 1;

            }

            return key;

        }

        public char[,] GetVigenereTable()
        {
            char start = 'A';
            char value = start;
            char[,] vigenereTable = new char[26, 26];

            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    vigenereTable[i, j] = value;
                    if (value < 'Z') value++;
                    else value = 'A';

                }
                start++;
                value = start;
            }
            return vigenereTable;
        }
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            string key_stream = GetKey(key, plainText);
            char[,] vigenereTable = GetVigenereTable();
            string ct = string.Empty;
            int length = plainText.Length;
            for (int i = 0; i < length; i++)
            {
                int ptInd = ((int)plainText[i]) % 97;
                int keyInd = ((int)key_stream[i]) % 97;
                ct += vigenereTable[ptInd, keyInd];
            }


            return ct;
        }
    }
}