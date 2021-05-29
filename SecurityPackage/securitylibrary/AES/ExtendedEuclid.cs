using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            int A1 = 1, A2 = 0, A3 = baseN;
            int B1 = 0, B2 = 1, B3 = number;

            while (true)
            {
                int q = A3 / B3;
                int T1 = A1 - (q * B1), T2 = A2 - (q * B2), T3 = A3 - (q * B3);

                A1 = B1;
                A2 = B2;
                A3 = B3;

                B1 = T1;
                B2 = T2;
                B3 = T3;

                if (B3 == 0)
                    return -1;

                else if (B3 == 1)
                {
                    while(B2 < 0)
                    {
                        B2 += baseN;
                    }
                    return B2;
                }
            }
        }
    }
}
