using System.Numerics;

namespace CryptoLab1
{
    internal class Tests
    {
        RNG _rng;
        public Tests(RNG rng)
        {
            _rng = rng;
        }
        bool miillerTest(BigInteger d, BigInteger n)
        {
            BigInteger r = _rng.RandomInRange(2, n - 2);
            BigInteger a = 2 + (r % (n - 4));
            BigInteger x = FastArithmetic.FastPower(a, d, n);

            if (x == 1 || x == n - 1)
                return true;
            while (d != n - 1)
            {
                x = (x * x) % n;
                d *= 2;

                if (x == 1)
                    return false;
                if (x == n - 1)
                    return true;
            }

            return false;
        }

        public bool isPrime(BigInteger n, BigInteger k)
        {
            if (n <= 1 || n == 4)
                return false;
            if (n <= 3)
                return true;

            BigInteger d = n - 1;

            while (d % 2 == 0)
                d /= 2;

            for (BigInteger i = 0; i < k; i++)
                if (miillerTest(d, n) == false)
                    return false;

            return true;
        }
    }
}
