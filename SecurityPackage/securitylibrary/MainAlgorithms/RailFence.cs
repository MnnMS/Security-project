using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, int key)
        {
            String pT = "";
            string cT = cipherText.ToLower();
            float div = (float)cT.Count() / key;
            int col = (int)Math.Ceiling(div);
            int count = cT.Count();
            //int accomulator = (count + 1) - (col * key);
            //int index;
            string[] cipherMat = new string[key];
            int row = 0, column = 0;
            for (int j = 0; j < count; j++)
            {
                if ((row + 1) * col == j)
                {
                    row++;
                }
                cipherMat[row] += cT[j];
                
            }
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < key; j++)
                {
                    if (cipherMat[j].Length>i)
                    {
                        pT += cipherMat[j][i];
                    }
                    
                }
                
            }
            return pT;
        }

        public string Encrypt(string plainText, int key)
        {
            string cT = "";
            //float div = plainText.Count() / key;
            string[] plainMat = new string[key];
            for (int i = 0; i < plainText.Count(); i++)
            {
                int index = i % (key);
                plainMat[index] += plainText[i];
            }
            for (int i = 0; i < key; i++)
            {
                cT += plainMat[i]; 
            }
            return cT.ToUpper();
        }
        
    }
}
