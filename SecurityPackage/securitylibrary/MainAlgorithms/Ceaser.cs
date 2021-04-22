using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        List<string> Characters = new List<string>();
        string[] array = new string[] { "A" , "B" , "C" , "D" , "E", "F", "G", "H", "I", "J", "K" , "L", "M", "N", "O", "P", "Q" ,"R" ,"S" ,"T" ,"U" ,"V" ,"W" ,"X" ,"Y" ,"Z" };
        public string Encrypt(string plainText, int key)
        {
            Characters.AddRange(array);
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
            throw new NotImplementedException();
        }

        public int Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }
    }
}
