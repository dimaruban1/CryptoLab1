using System;
using System.Collections.Generic;
using System.Linq;

public static class PrimalityBPSW
{
    private static List<long> smallPrimes = Enumerable.Range(1, 1000).Where(isPrimeTrialDiv).ToList();

    // Trial division primality testing for precomputing small primes
    private static bool isPrimeTrialDiv(int n)
    {
        if (n == 0 || n == 1)
            return false;
        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0)
                return false;
        }
        return true;
    }

    // Baillie-PSW probable prime test
    public static bool isPrimeBPSW(long n)
    {
        if (n == 0 || n == 1)
            return false;
        if (n == 2)
            return true;
        if (n < 0)
            return false;

        if (n <= smallPrimes.Max() && smallPrimes.Contains(n))
            return true;

        bool isSmallPrime = smallPrimes.Any(p => p == n);

        return !divisibleBySmallPrime(n) && fermatStrongProbablePrime(n) && lucasStrongProbablePrime(n);
    }

    private static bool divisibleBySmallPrime(long n)
    {
        return smallPrimes.Any(p => n % p == 0 && n != p);
    }

    private static bool fermatStrongProbablePrime(long n)
    {
        if (n == 2)
            return true;

        long d = n - 1;
        int s = 0;
        while (d % 2 == 0)
        {
            d /= 2;
            s++;
        }

        for (int r = 0; r < s; r++)
        {
            long a = modExp(2, d * (long)Math.Pow(2, r), n);
            if (a == 1 || a == n - 1)
                continue;
            bool isWitness = false;
            for (int i = 0; i < s - 1; i++)
            {
                a = modExp(a, 2, n);
                if (a == 1)
                    return false;
                if (a == n - 1)
                {
                    isWitness = true;
                    break;
                }
            }
            if (!isWitness)
                return false;
        }
        return true;
    }

    private static long modExp(long baseNum, long exp, long modulo)
    {
        if (modulo == 1)
            return 0;
        long result = 1;
        baseNum = baseNum % modulo;
        while (exp > 0)
        {
            if (exp % 2 == 1)
                result = (result * baseNum) % modulo;
            exp /= 2;
            baseNum = (baseNum * baseNum) % modulo;
        }
        return result;
    }

    private static bool lucasStrongProbablePrime(long n)
    {
        long dn = n + 1;
        long d = dn;
        int s = 0;
        while (d % 2 == 0)
        {
            d /= 2;
            s++;
        }

        long p = 1, q = (1 - d) / 4;
        if (selfredgeParams(n, out d, out p, out q))
        {
            if (pred1(d, p, q))
                return true;
            if (pred2(d, p, q))
                return true;
        }
        return false;
    }

    private static bool selfredgeParams(long n, out long d, out long p, out long q)
    {
        d = 0; p = 0; q = 0;
        long[] ds = { 5, -7, 9 };
        while (true)
        {
            bool found = false;
            foreach (long item in ds)
            {
                if (jacobiSymbol(item, n) == -1)
                {
                    d = item;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                if (isSquare(n))
                    return false;
                ds = Enumerable.Range(11, int.MaxValue).Select((x, index) => x * (index % 2 == 0 ? 1 : -1)).ToArray();
                continue;
            }
            if (jacobiSymbol(d, n) == 0)
                return false;
            p = 1;
            q = (1 - d) / 4;
            return true;
        }
    }

    private static bool pred1(long d, long p, long q)
    {
        long u, v;
        lucasNumber(d, d, p, q, out u, out v);
        return u == 0;
    }

    private static bool pred2(long d, long p, long q)
    {
        long u, v;
        lucasNumber(d * 2, d, p, q, out u, out v);
        return v == 0;
    }

    private static void lucasNumber(long n, long m, long p, long q, out long u, out long v)
    {
        if (n == 0)
        {
            u = 0;
            v = 2;
            return;
        }
        if (n == 1)
        {
            u = p;
            v = q;
            return;
        }

        long u1 = p, v1 = q, u2 = 0, v2 = 2;
        long d = m;

        while (n > 1)
        {
            if (n % 2 == 0)
            {
                u2 = (u1 * v1) % d;
                v2 = ((u1 * v2) + (v1 * u2)) % d;
                u1 = (u1 * u1 + (v1 * v1 * 2)) % d;
                v1 = ((u1 * v1) + (v1 * u1)) % d;
                n /= 2;
            }
            else
            {
                long tmp1 = (p * u1 + v1) % d;
                long tmp2 = (p * v1 + (d - q) * u1) % d;
                u2 = u1 * tmp1 % d;
                v2 = v1 * tmp2 % d;
                u1 = tmp1;
                v1 = tmp2;
                n -= 1;
            }
        }
        u = u2;
        v = v2;
    }

    private static bool isSquare(long n)
    {
        if (n < 0)
            return false;
        long root = (long)Math.Sqrt(n);
        return root * root == n;
    }

    private static int jacobiSymbol(long a, long n)
    {
        if (a == 0)
            return 0;
        if (a == 1)
            return 1;
        if (a >= n || a < 0)
            return jacobiSymbol(a % n, n);
        if (a % 2 == 0)
            return ((n % 8 == 1) || (n % 8 == 7)) ? jacobiSymbol(a / 2, n) : -jacobiSymbol(a / 2, n);
        if (gcd(a, n) != 1)
            return 0;
        return ((a % 4 == 3) && (n % 4 == 3)) ? -jacobiSymbol(n, a) : jacobiSymbol(n, a);
    }

    private static long gcd(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static Tuple<long, long> halveUntilOdd(long n)
    {
        long s = 0;
        while (n % 2 == 0)
        {
            n /= 2;
            s++;
        }
        return Tuple.Create(n, s);
    }
}
