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
            int det = determ2(matrix);
            if (det != -1 && det != 1)
                throw new Exception();


            int x = 1 / det;
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
                    int subDet = (m == 3 ? determ2(getSubMat(matrix, i, j)) : 1);
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

        private List<List<int>> generateKey()
        {
            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < 26; i++)
            {
                for(int j = 0; j < 26; j++)
                {    
                    for(int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            List<int> key = new List<int>(new int[]{i, j, k, l});
                            result.Add(key);
                        }
                    }          
                   
                }
            }
            return result;
        }

        /*private int[,] multMat(int[,] A, int[,] B)
        {
            int n = A.GetLength(0);
            int m = B.GetLength(1);
            int x = B.GetLength(0);
            int[,] res = new int[n, m];
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    int sum = 0;
                    for(int k = 0; k < x; k++)
                    {
                        sum += A[i, k] * B[k, j];
                    }
                    res[i, j] = mod(sum);
                }
            }
            return res;
        }*/

        private int[,] multiplyMat(int[,] key, int[,] textMat)
        {
            int n = textMat.GetLength(1);
            int m = textMat.GetLength(0);
            int keyDim = key.GetLength(0);
            List<int> cipher = toList(textMat);
            int[,] result ;
            List<int> res = new List<int>();
            int nOfCols = cipher.Count / keyDim;
            for(int i = 0; i < nOfCols; i++)
            {
                List<int> col = cipher.GetRange(i* keyDim, keyDim) ;
                int[] resCol = new int[keyDim];
                for(int k = 0; k < keyDim; k++)
                {
                    int cell = 0;
                    for (int j = 0; j < keyDim; j++)
                    {
                        cell += col[j] * key[k, j];
                    }
                    resCol[k] = mod(cell);
                }
                res.AddRange(resCol);
            }
            result = toMatrix(res, m, n);
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

        private bool isValid(List<int> key, List<int> plainText, List<int> cipherText)
        {
            List<int> plain = Decrypt(cipherText, key);
            for (int i = 0; i < plain.Count; i++)
            {
                if (plain[i] != plainText[i])
                    return false;
            }
            return true;
        }

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            List<int> key = new List<int>();
            List<List<int>> choices = generateKey();
            bool flag = false;
            for(int i = 0; i < choices.Count; i++)
            {
                int det = determ2(toMatrix(choices[i], 2, 2));
                if ((det==1 || det==-1)&&(isValid(choices[i], plainText, cipherText)))
                {
                    flag = true;
                    key = choices[i];
                    break;
                }
            }
            if (!flag)
                throw new InvalidAnlysisException();
  
            return key;
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> plain = new List<int>();
            int m = (int)Math.Sqrt(key.Count);
            int[,] cipher = (toMatrix(cipherText, m, cipherText.Count/m)), keyMat = toMatrix(key, m, m);
            if(m == 2) // 2x2 inverse
            {
                int [,] keyInv = inverse2(keyMat);
                int[,] plainMat = multiplyMat(keyInv, cipher);
                plain = toList(plainMat);         
            }
            else // 3x3 inverse 
            {
                int det = determ3(keyMat);
                if (gcd(det, 1) == 1)
                {
                    int b = 1;
                    while (mod(b * det) != 1)
                        b++;
                    int[,] keyInv = inverse3(keyMat, b);
                    int[,] plainMat = multiplyMat(keyInv, cipher);
                    plain = toList(plainMat);
                }
                else
                    throw new Exception();
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
            return key;
        }

    }
}
