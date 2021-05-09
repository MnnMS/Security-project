using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            string pT = "";
            string cT = cipherText.ToLower();
            int ascInd;
            for (int i = 0; i < cT.Length; i++)
            {
                if (cT[i] < key[i])
                    ascInd = 'z' - key[i] + cT[i] + 1;
                else
                    ascInd = cT[i] - key[i] + 'a';
                pT += (char)ascInd;
                if (key.Length<cT.Length)
                {
                    key += pT[i];
                }
            }
            
            Console.WriteLine(pT);
            return pT;
        }

        public string Encrypt(string plainText, string key)
        {
            string keyStream = "";
            string cT = "";
            int diff = plainText.Length - key.Length;

            if (key.Length != plainText.Length)
                keyStream = String.Concat(key, plainText.Substring(0, diff));
            else
                keyStream = key;
            for (int i = 0; i < plainText.Length; i++)
            {
                int ascInd = plainText[i] - 'a';
                ascInd += keyStream[i];
                if (ascInd  > 'z')
                {
                    int asciDiff = (ascInd - 'z')-1 ;
                    ascInd = asciDiff + 'a';
                }
                cT += (char)ascInd;

            }


            
            return cT.ToUpper();
        }
    }
}
