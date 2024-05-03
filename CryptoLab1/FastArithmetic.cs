using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLab1
{
    internal class FastArithmetic
    {
        public static BigInteger FastPower(BigInteger baseNum, BigInteger exponent, BigInteger modulus)
        {
            BigInteger result = 1;
            baseNum %= modulus;
            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                {
                    result = (result * baseNum) % modulus;
                }
                exponent >>= 1;
                baseNum = (baseNum * baseNum) % modulus;
            }
            return result;
        }

    }
}
