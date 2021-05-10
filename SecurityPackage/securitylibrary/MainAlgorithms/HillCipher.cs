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
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            List<int> key = new List<int>();
            List<List<int>> choices = generateKey();
            bool flag = false;
            for(int i = 0; i < choices.Count; i++)
            {
                Matrix choice = new Matrix(choices[i], 2, 2);
                int det = choice.determ2();
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
            Matrix cipher = new Matrix(cipherText, m, cipherText.Count / m);
            Matrix keyMat = new Matrix(key, m, m);
            if(m == 2) // 2x2 inverse
            {
                Matrix keyInv = keyMat.inverse();
                Matrix plainMat = keyInv.multiply(cipher);
                plain = plainMat.toList();         
            }
            else // 3x3 inverse 
            {
                int det = keyMat.determ3();
                if (gcd(det, 1) == 1)
                {
                    Matrix keyInv = keyMat.inverse();
                    Matrix plainMat = keyInv.multiply(cipher);
                    plain = plainMat.toList();
                }
                else
                    throw new Exception();
            }
            return plain;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int m =(int)Math.Sqrt(key.Count());
            Matrix keyMat = new Matrix(key, m, m);
            int n = (int)Math.Ceiling((double)(plainText.Count() / m));
            Matrix ptMat = new Matrix(plainText,m,n);
            Matrix resultMat = keyMat.multiply(ptMat);
            List<int> ct = resultMat.toList();
            return ct;
        }

        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            List<int> key = new List<int>();
            Matrix cipher = new Matrix(cipherText, 3, cipherText.Count / 3);
            Matrix plain = new Matrix(plainText, 3, plainText.Count / 3);

            int det = plain.determ3();
            if (gcd(det, 1) == 1)
            {
                Matrix plainInv = plain.inverse();
                Matrix keyMat = plainInv.multiply(cipher.T());
                key = keyMat.toList();
            }
            return key;
        }

        private List<List<int>> generateKey()
        {
            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            List<int> key = new List<int>(new int[] { i, j, k, l });
                            result.Add(key);
                        }
                    }

                }
            }
            return result;
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

    }

    public class Matrix
    {
        int n, m;
        public int[,] value;

        public Matrix(int n, int m)
        {
            this.n = n;
            this.m = m;
            value = new int[n, m];
        }

        public Matrix(List<int> arr, int n, int m)
        {
            this.n = n;
            this.m = m;
            value = new int[n, m];
            int ind = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    value[i, j] = arr[ind++];
                }
            }
        }

        public List<int> toList()
        {
            int n = value.GetLength(0);
            int m = value.GetLength(1);
            List<int> arr = new List<int>(n * m);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    arr.Add(value[i, j]);
                }
            }

            return arr;
        }

        private int mod(int x)
        {
            while (x < 0) x += 26;
            return x % 26;
        }

        public Matrix multiply(Matrix textMat)
        {
            int n = textMat.m;
            int m = textMat.n;
            int keyDim = this.n;
            List<int> cipher = textMat.toList();
            List<int> res = new List<int>();
            int nOfCols = cipher.Count / keyDim;
            for (int i = 0; i < nOfCols; i++)
            {
                List<int> col = cipher.GetRange(i * keyDim, keyDim);
                int[] resCol = new int[keyDim];
                for (int k = 0; k < keyDim; k++)
                {
                    int cell = 0;
                    for (int j = 0; j < keyDim; j++)
                    {
                        cell += col[j] * this.value[k, j];
                    }
                    resCol[k] = mod(cell);
                }
                res.AddRange(resCol);
            }
            return new Matrix(res, m, n);
        }

        public Matrix getSubMat(int i, int j)
        {
            int m = this.n;
            List<int> sub = new List<int>();
            for (int k = 0; k < m; k++)
            {
                for (int n = 0; n < m; n++)
                {
                    if (k != i && n != j)
                        sub.Add(value[k, n]);
                }
            }
            return new Matrix(sub, 2, 2);
        }

        public int determ3()
        {
            int m = n;
            int result = 0;
            int sign = 1;

            for (int i = 0; i < m; i++)
            {
                int x = value[0, i] * sign;
                Matrix subMat = getSubMat(0, i);
                int subDet = subMat.determ2();
                result += x * subDet;
                sign *= -1;
            }
            return result;
        }

        public int determ2()
        {
            int d1 = value[0, 0] * value[1, 1];
            int d2 = value[0, 1] * value[1, 0];
            return d1 - d2;
        }

        public Matrix inverse()
        {
            int m = n;
            Matrix inverse = new Matrix(m, m);
            if (m == 3)
            {
                int det = determ3();
                int b = 1;
                while (mod(b * det) != 1)
                    b++;
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        Matrix subMatrix = getSubMat(i, j);
                        int subDet = (m == 3 ? subMatrix.determ2() : 1);
                        inverse.value[i, j] = mod(b * (int)Math.Pow(-1, i + j) * subDet);
                    }
                }
                inverse = inverse.T();
            }
            else
            {
                int a = value[0, 0], b = value[0, 1], c = value[1, 0], d = value[1, 1];
                int det = determ2();
                if (det != -1 && det != 1)
                    throw new Exception();


                int x = 1 / det;
                inverse.value[0, 0] = mod(d * x);
                inverse.value[0, 1] = mod(-1 * b * x);
                inverse.value[1, 0] = mod(-1 * c * x);
                inverse.value[1, 1] = mod(a * x);
            }
                   
            return inverse;
        }

        public Matrix T()
        {
            int n = this.n;
            int m = this.m;
            Matrix trans = new Matrix(m, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    trans.value[j, i] = value[i, j];
                }
            }
            return trans;
        }
    }
}
