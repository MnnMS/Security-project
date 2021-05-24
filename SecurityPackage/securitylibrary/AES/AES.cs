using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        int block_size = 16;
        int num_of_rounds = 10;
        int row_col = 4;
        char[] seps = { '\t', ' ' };
        Dictionary<string, int> Hex = new Dictionary<string, int>(){
            { "a", 10 }, { "b", 11 }, { "c", 12 }, { "d", 13 }, { "e", 14 }, { "f", 15 }
        };

        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            // --- (plain to blocks) ---

            // --- (intial) ---
            // addRoundKey

            // --- (num_of_rounds - 1) ---
            // 1.subBytes
            // 2.shiftRows - Menna
            // 3.mixColumns - Menna
            // 4.addRoundKey

            // --- (final round) ---
            // 1.subBytes
            // 2.shiftRows
            // 3.addRoundKey


            throw new NotImplementedException();
        }

        public string[,] SubBytes(string [,] plain)
        {
            string[,] new_block = new string[row_col, row_col];
            string[,] sbox = read_SBox(true);

            for(int i = 0; i < row_col; i++)
            {
                for(int j = 0; j < row_col; j++)
                {
                    string curr_byte = plain[i, j];
                    int[] RC = Byte_to_Int(curr_byte);
                    string new_byte = sbox[RC[0] + 1, RC[1] + 1];
                    new_block[i, j] = new_byte;
                }
            }

            return new_block;
        }

        public int[] Byte_to_Int(string Byte)
        {
            int[] res = new int[2];
            string a = Byte[0].ToString(), b = Byte[1].ToString();
            res[0] = Math.Min(int.Parse(a), Hex[a]);
            res[1] = Math.Min(int.Parse(b), Hex[b]);
            return res;
        }

        public string[,] read_SBox(bool inverse)
        {
            string[,] Sbox = new string[block_size+1, block_size+1];
            string file_name = (inverse ? "invers-sbox.txt" : "s-box.txt");
            string[] lines = File.ReadAllLines(file_name);
            int index = 0;
            foreach (string line in lines)
            {
                string[] row = line.Split(seps, StringSplitOptions.RemoveEmptyEntries);
                for(int j = 0; j <= block_size; j++)
                {
                    Sbox[index, j] = row[j];
                    if (index == 0 && j + 1 == block_size)
                        break;
                }
                index++;
            }

            return Sbox;
        }
    }
}
