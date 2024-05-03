using CryptoLab1;
using System.Numerics;
using System.Security.Principal;

using System.Security.Cryptography.RandomNumberGenerator rng = System.Security.Cryptography.RandomNumberGenerator.Create();

/*
 Математичні операції та генерація простих чисел (перевірка простоти чисел)
+Реалізувати операцію швидкого піднесення до степеню по модулю (дуже бажано оптимізувати і інші арифметичні операції, зокрема множення)
Реалізувати 2 алгоритма перевірки чисел на простоту:
+Miller–Rabin primality test
-Baillie–PSW primality test  ***
+Написати до них тести. Передбачити можливість виводу результатів у base2, base10, base64, byte[]
+Передбачити пошук простих чисел з заданою “бітністю”. (степені двійки)
 */

RNG my_rng = new RNG(rng);
Tests tests = new Tests(my_rng);

const int TEST_NUMBER = 2333;
Console.WriteLine($"Got: {tests.isPrime(BigInteger.Parse("23297167639393"), TEST_NUMBER)}, expected: True");
Console.WriteLine($"Got: {tests.isPrime(BigInteger.Parse("17"), TEST_NUMBER)}, expected: True");
Console.WriteLine($"Got: {tests.isPrime(BigInteger.Parse("181"), TEST_NUMBER)}, expected: True");
Console.WriteLine($"Got: {tests.isPrime(BigInteger.Parse("65156161681"), TEST_NUMBER)}, expected: False");
Console.WriteLine($"Got: {tests.isPrime(BigInteger.Parse("999999999"), TEST_NUMBER)}, expected: False");
Console.WriteLine($"Got: {tests.isPrime(BigInteger.Parse("856698984984562616848947989556189"), TEST_NUMBER)}, expected: False");
Console.WriteLine($"Got: {tests.isPrime(BigInteger.Parse("55555"), TEST_NUMBER)}, expected: False");
Console.WriteLine($"Got: {tests.isPrime(BigInteger.Parse("3"), TEST_NUMBER)}, expected: True");
Console.WriteLine($"Got: {tests.isPrime(BigInteger.Parse("15"), TEST_NUMBER)}, expected: False");

BigInteger generatePrimeNumber(int bits)
{

    BigInteger maxInt = BigInteger.Pow(2, bits);
    BigInteger minInt = BigInteger.Pow(2, bits-1);
    BigInteger n = my_rng.randomInRangeFromZeroToPositive(maxInt);
    while (!tests.isPrime(n, TEST_NUMBER))
    {
        n = my_rng.RandomInRange(minInt, maxInt);
        continue;
    }
    return n;
}

const int n = 32;
var number = generatePrimeNumber(n);
Console.WriteLine($"Generated prime number with {n} bits is: {number}");
var bytes = number.ToByteArray();
Console.Write("Byte array = [");
foreach (var b in bytes)
{
    Console.Write($"{b},");
}
Console.WriteLine("]");
Console.WriteLine($"Base 2: {ToBase2(number)}");
Console.WriteLine($"Base 64: {Convert.ToBase64String(bytes)}");

string ToBase2(BigInteger number)
{
    BigInteger remainder;
    string result = string.Empty;
    while (number > 0)
    {
        remainder = number % 2;
        number /= 2;
        result = remainder.ToString() + result;
    }
    return result;
}