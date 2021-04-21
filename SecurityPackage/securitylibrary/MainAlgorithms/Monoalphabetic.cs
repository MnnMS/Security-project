using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            char[] key = new char[26];
            List<char> alpha = new List<char>();
            cipherText = cipherText.ToLower();
            int ciTxtCnter = 0;
            for (int i = 0; i < 26; i++)
            {
                alpha.Add((char)('a' + i));
                key[i] = '0';
            }
            foreach(char c in plainText)
            {
                int index = c - 'a';
                key[index] = cipherText[ciTxtCnter];
                alpha.Remove(cipherText[ciTxtCnter]);
                ciTxtCnter++;
            }
            for (int i = 0; i < 26; i++)
            {
                Console.WriteLine(key[i]);

                if (key[i] == '0')
                {
                    key[i] = alpha[0];
                    alpha.Remove(key[i]);
                }
            }
            string output = new string(key);
            Console.WriteLine(output);
            return output;
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = "";
            foreach (char c in cipherText.ToLower())
            {
                int index = key.IndexOf(c);
                plainText += (char)(index + 'a');
            }
            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = "";
            foreach (char c in plainText)
            {
                int index = c - 'a';
                cipherText += key[index];
            }
            return cipherText.ToUpper();
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            cipher = cipher.ToLower();
            string plainTxt = "";
            List<int> test = new List<int>();
            Dictionary<char, int> freqArr = new Dictionary<char, int>();
            Dictionary<char, char> subArr = new Dictionary<char, char>();
            foreach(char c in cipher)
            {
                if (freqArr.ContainsKey(c))
                {
                    freqArr[c]++;
                }
                else
                {
                    test[c - 'a'] = 0;
                    freqArr.Add(c,1);
                }
            }
            foreach(char c in cipher)
            {

            }


            return plainTxt;
        }
    }
}
