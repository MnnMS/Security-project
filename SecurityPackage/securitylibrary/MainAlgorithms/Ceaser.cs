using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string Encrypt(string plainText, int key)
        {
            String ct = "";
            foreach (char c in plainText)
            {
                String pt = c.ToString().ToUpper();
                int idx = Characters.IndexOf(pt);
                idx = (idx + key) % 26;
                ct = ct + Characters[idx];
            }
            return ct;
        }

        public string Decrypt(string cipherText, int key)
        {
            String pt = "";
            foreach (char c in cipherText)
            {
                String ct = c.ToString();
                int idx = Characters.IndexOf(ct);
                int id = idx - key;
                while (id < 0)
                {
                    id += 26;
                }
                string x = Characters[id].ToString().ToLower();
                pt = pt + x;
            }
            return pt;
        }

        public int Analyse(string plainText, string cipherText)
        {
            string pt = plainText[0].ToString().ToUpper();
            int idx = Characters.IndexOf(pt);
            string ct = cipherText[0].ToString();
            int id2 = Characters.IndexOf(ct);
            int Key = id2 - idx;
            while(Key < 0)
            {
                Key += 26;
            }
            return Key;
        }
    }
}
