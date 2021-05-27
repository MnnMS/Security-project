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
        static int block_size = 16;
        int num_of_rounds = 10;
        int row_col = (int)Math.Sqrt(block_size);
        char[] seps = { '\t', ' ' };
        Dictionary<string, int> Hex = new Dictionary<string, int>(){
            { "a", 10 }, { "b", 11 }, { "c", 12 }, { "d", 13 }, { "e", 14 }, { "f", 15 }
        };
        List<string> rcon;
        int[,] constant_matrix = { { 0x02, 0x03, 0x01, 0x01 }, { 0x01, 0x02, 0x03, 0x01 }, { 0x01, 0x01, 0x02, 0x03 }, { 0x03, 0x01, 0x01, 0x02 } };
        string[,] sbox;
        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            rcon = new List<string>();
            sbox = read_SBox(false);

            string[,] plainTextMat = GetMatrix(plainText.ToLower());
            string[,] keyMat= GetMatrix(key.ToLower());

            plainTextMat = AddRoundKey(plainTextMat, keyMat);

            for(int i=0;i< num_of_rounds - 1; i++)
            {
                plainTextMat=SubBytes(plainTextMat);

                plainTextMat=ShiftRows(plainTextMat);

                plainTextMat = MixColumns(plainTextMat);

                keyMat = KeySchedule(keyMat, i + 1);
                plainTextMat = AddRoundKey(plainTextMat, keyMat);

            }
            plainTextMat = SubBytes(plainTextMat);

            plainTextMat = ShiftRows(plainTextMat);

            keyMat = KeySchedule(keyMat, num_of_rounds);
            plainTextMat = AddRoundKey(plainTextMat, keyMat);

            return GetCipherString(plainTextMat);
        }
        public string[,] GetMatrix(string stringarray)
        {
            string[,] matrix = new string[row_col, row_col];
            int ind = 2;
            for(int i=0; i< row_col; i++)
            {
                for(int j=0;j< row_col; j++)
                {
                    matrix[j, i] = stringarray.Substring(ind, 2);
                    ind+=2;
                }
                    
            }
            return matrix;
        }

        public string GetCipherString(string[,] plain)
        {
            string Cipher = "0x";
            for (int i = 0; i < row_col; i++)
            {
                for (int j = 0; j < row_col; j++)
                {
                    Cipher += plain[j, i];
                }

            }
            return Cipher;

        }

        public void UpdateRcon(int index)
        {
            if (index == 0)
            {
                rcon.Add("01");
                return;
            }
            int R_prev = int.Parse(rcon[index - 1], System.Globalization.NumberStyles.HexNumber);
            int new_R = R_prev * 2;
            if (R_prev >= 128)
            {
                new_R = new_R ^ 283;
            }
           
            rcon.Add(new_R.ToString("x2"));
        }

        public string[,] KeySchedule(string[,] prev_key, int round)
        {
            string[,] new_key = new string[row_col, row_col];
            string[,] prev_W = new string[row_col, 1];
            for(int i = 0; i < row_col; i++)
            {
                prev_W[i, 0] = prev_key[i, row_col - 1];
            }

            string temp = prev_W[0, 0];
            for (int i = 1; i < row_col; i++)
            {
                prev_W[i - 1, 0] = prev_W[i, 0];
            }
            prev_W[row_col - 1, 0] = temp;

            prev_W = SubBytes(prev_W);
            UpdateRcon(round - 1);
            string rcon_val = rcon[round - 1];

            new_key[0, 0] = XOR(XOR(prev_key[0, 0], prev_W[0, 0]), rcon_val);

            for (int i = 1; i < row_col; i++)
            {
                new_key[i, 0] = XOR(prev_key[i, 0], prev_W[i, 0]);
            }

            for(int j = 1; j < row_col; j++)
            {
                for(int i = 0; i < row_col; i++)
                {
                    new_key[i, j] = XOR(prev_key[i, j], new_key[i, j-1]);
                }       
            }

            return new_key;
        }

        public string XOR(string a, string b)
        {
            int[] byte1 = Byte_to_Int(a);
            int[] byte2 = Byte_to_Int(b);
            int[] res = { byte1[0] ^ byte2[0], byte1[1] ^ byte2[1] };
            string ans = res[0].ToString("x") + res[1].ToString("x");
            return ans;
        }

        public string[,] SubBytes(string [,] plain)
        {
            int rowsOrHeight = plain.GetLength(0);
            int colsOrWidth = plain.GetLength(1);
            string[,] new_block = new string[row_col, row_col];
            

            for(int i = 0; i < rowsOrHeight; i++)
            {
                for(int j = 0; j < colsOrWidth; j++)
                {
                    string curr_byte = plain[i, j];
                    if (curr_byte.Length == 1)
                    {
                        curr_byte=curr_byte.Insert(0, "0");
                    }
                    int[] RC = Byte_to_Int(curr_byte);
                    string new_byte = sbox[RC[0], RC[1]];
                    new_block[i, j] = new_byte;
                }
            }

            return new_block;
        }
        

        public string[,] AddRoundKey(string[,] plain, string[,] key)
        {
            string[,] new_block = new string[row_col, row_col];

            for(int i = 0; i < row_col; i++)
            {
                for (int j = 0; j < row_col; j++)
                {
                    int[] byte1 = Byte_to_Int(plain[i, j]), byte2 = Byte_to_Int(key[i, j]);
                    int[] res = {byte1[0]^byte2[0], byte1[1]^byte2[1]};
                    string ans = res[0].ToString("x") + res[1].ToString("x");

                    new_block[i, j] = ans;
                }
            }

            return new_block;
        }

        public int[] Byte_to_Int(string Byte)
        {
            int[] res = new int[2];
            string a = Byte[0].ToString(), b = Byte[1].ToString();
            if(Hex.ContainsKey(a)==true)
                res[0] = Hex[a];
            else
                res[0] = int.Parse(a);
            if (Hex.ContainsKey(b) == true)
                res[1] = Hex[b];
            else
                res[1] = int.Parse(b);
            return res;
        }

        public string[,] read_SBox(bool inverse)
        {
            string[,] Sbox = new string[block_size, block_size];
            string file_name = (inverse ? "invers-sbox.txt" : "s-box.txt");
            string[] lines = File.ReadAllLines(file_name);
            for(int index=0;index<block_size;index++)
            {
                string line = lines[index];
                string[] row = line.Split(seps, StringSplitOptions.RemoveEmptyEntries);
                for(int j = 0; j < block_size; j++)
                {
                    
                    if (row[j].Length == 1)
                        row[j]=row[j].Insert(0, "0");
                    Sbox[index, j] = row[j];
                }
            }

            return Sbox;
        }
        public string[,] ShiftRows(string[,] plain)
        {
            string[,] newPlain = new string[row_col, row_col];
            for (int i = 0; i < row_col; i++)
                newPlain[0, i] = plain[0, i];
            for (int i = 1; i < row_col; i++)
            {
                for (int j = 0; j < row_col; j++)
                {
                    int col = (j + (row_col-i)) % row_col;
                    newPlain[i, col] = plain[i, j];
                }
            }
            return newPlain;
        }
        public string[,] MixColumns(string[,] plain)
        {
            string[,] newPlain = new string[row_col, row_col];
            for (int i = 0; i < row_col; i++)
            {
                for (int j = 0; j < row_col; j++)
                {
                    int value = 0x00;
                    for (int k = 0; k < row_col; k++)
                    {
                        value = value^GetMultiplicationValue(constant_matrix[j, k], Int32.Parse(plain[k, i], System.Globalization.NumberStyles.HexNumber));
                    }
                    newPlain[j, i] = value.ToString("x2");
                }
            }
            return newPlain;
        }

        public int GetMultiplicationValue(int r, int Byte)
        {
            if (r == 0x01)
                return Byte;
            else if (r == 0x02)
            {
                if ((Byte & 10000000) > 0)
                {
                    Byte = (Byte << 1) & 0xff;
                    Byte ^= 0x1b;
                }
                else
                {
                    Byte = (Byte << 1);
                }
                return Byte;
            }
            else
            {
                int r1 = Byte;
                if ((Byte & 10000000) > 0)
                {
                    Byte = (Byte << 1) & 0xff;
                    Byte ^= 0x1b;
                }
                else
                {
                    Byte = (Byte << 1);
                }
                return Byte ^ r1;
            }
        }
    }
}
