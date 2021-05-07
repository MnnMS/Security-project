using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        private int[,] toMatrix(List<int> arr, int n, int m)
        {
            int[,] mat = new int[n, m];
            int ind = 0;

            for (int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    mat[i, j] = arr[ind++];
                }
            }

            return mat;
        }

        private List<int> toList(int[,] mat)
        {
            int n = mat.GetLength(0);
            int m = mat.GetLength(1);
            List<int> arr = new List<int>(n*m);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    arr.Add(mat[i, j]);
                }
            }

            return arr;
        }

        private int mod(int x)
        {
            while (x < 0) x += 26;
            return x % 26;
        }

        private int[,] inverse2(int [,] matrix)
        {
            int m = matrix.GetLength(0);
            int[,] inverse = new int[m,m];
            int a = matrix[0, 0], b = matrix[0,1], c = matrix[1,0], d = matrix[1,1];
            int x = 1 / determ2(matrix);

            inverse[0, 0] = d * x;
            inverse[0, 1] = -1 * b * x;
            inverse[1, 0] = -1 * c * x;
            inverse[1, 1] = a * x;

            return inverse;
        }

        private int[,] inverse3(int[,] matrix, int b)
        {
            int m = matrix.GetLength(0);
            int[,] inverse = new int[m, m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int subDet = determ2(getSubMat(matrix, i, j));
                    inverse[i, j] = mod(b * (int)Math.Pow(-1, i + j) * subDet);
                }
            }

            return transpose(inverse);
        }

        private int[,] transpose(int [,] mat)
        {
            int n = mat.GetLength(0);
            int m = mat.GetLength(1);
            int[,] trans = new int[m, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    trans[j, i] = mat[i, j];
                }
            }
            return trans;
        }

        private int[,] multiplyMat(int[,] mat1, int[,] mat2)
        {
            int n = mat2.GetLength(1);
            List<int> cipher = toList(mat2);
            int[,] result ;
            List<int> res = new List<int>();
            for(int i = 0; i < n; i++)
            {
                List<int> col = cipher.GetRange(i*3,3) ;
                int[] resCol = new int[3];
                for(int k = 0; k < 3; k++)
                {
                    int cell = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        cell += col[j] * mat1[k, j];
                    }
                    resCol[k] = mod(cell);
                }
                res.AddRange(resCol);
            }
            result = toMatrix(res, 3, n);
            return result;
        }

        private int[,] getSubMat(int [,] mat, int i, int j)
        {
            int m = mat.GetLength(0);
            List<int> sub = new List<int>();
            int[,] subMat = new int[m - 1, m - 1];
            for (int k = 0; k < m; k++)
            {
                for (int n = 0; n < m; n++)
                {
                    if (k != i && n != j)
                        sub.Add(mat[k, n]);
                }
            }
            return toMatrix(sub, 2, 2);
        }

        private int determ3(int [,] mat)
        {
            int m = mat.GetLength(0);
            int result = 0;
            int sign = 1;

            for(int i = 0; i < m; i++)
            {
                int x = mat[0, i] * sign;
                List<int> sub = new List<int>();
                int[,] subMat = getSubMat(mat, 0, i);
                int subDet = determ2(subMat);
                result += x * subDet;
                sign *= -1;
            }
            return result;
        }

        private int determ2(int [,] mat)
        {
            int d1 = mat[0,0] * mat[1,1];
            int d2 = mat[0, 1] * mat[1, 0];
            return d1 - d2;
        }

        private static int gcd(int n1, int n2)
        {
            if (n2 == 0)
                return n1;
            else
                return gcd(n2, n1 % n2);
        }

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> plain = new List<int>();
            int[,] cipher = (toMatrix(cipherText, 3, cipherText.Count/3)), keyMat = toMatrix(key, 3, 3);
            if(key.Count == 4) // 2x2 inverse
            {
                int [,] keyInv = inverse2(keyMat);
                
            }
            else // 3x3 inverse 
            {
                int det = determ3(keyMat);
                if(gcd(det, 1) == 1)
                {
                    int b = 1;
                    while (mod(b * det) != 1)
                        b++;
                    int[,] keyInv = inverse3(keyMat, b);
                    int[,] plainMat = multiplyMat(keyInv, cipher);
                    plain = toList(plainMat);
                }
            }
            return plain;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            throw new NotImplementedException();
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            List<int> key = new List<int>();
            int[,] cipher = (toMatrix(cipherText, 3, cipherText.Count / 3)), plain = (toMatrix(plainText, 3, plainText.Count / 3));
            //int[,] plainTrns = transpose(plain);
            int det = determ3(plain);
            int[,] plainInv = new int[3,3];
            if (gcd(det, 1) == 1)
            {
                int b = 1;
                while (mod(b * det) != 1)
                    b++;
                plainInv = inverse3(plain, b);
                int[,] keyMat = multiplyMat( plainInv,transpose(cipher));
                key = toList(keyMat);
            }

            foreach(int item in key)
            {
                Console.WriteLine(item);
            }
            foreach (int item in cipher)
            {
                Console.WriteLine(item);
            }
            foreach (int item in plainInv)
            {
                Console.WriteLine(item);
            }
            return key;
        }

    }
}
