using System;
using System.Linq;
using System.Numerics;

public static class NumberFormat {
    public static string ShortForm(Rational value) {
        bool isNegative = (value < 0);
        value = Rational.Abs(value);

        if(value < 1000) {
            return $"{(isNegative ? "-" : "")}{value}";
        }

        BigInteger counter = 1000000;
        foreach(var suffix in numberSuffixes) {
            if(value < counter) {
                var newValue = Rational.Truncate(value / (counter / 1000) * 100) / 100;
                return $"{(isNegative ? "-" : "")}{newValue}{suffix}";
            }
            counter *= 1000;
        }
        var lastNewValue = Rational.Truncate(value / (counter / 1000) * 100) / 100;
        return $"{(isNegative ? "-" : "")}{lastNewValue}{numberSuffixes.Last()}";
    }

    public static readonly string[] numberSuffixes = { "k","M","B","T","Q" };
}

public static class RandomExtensions {
    public static double NextDouble(this Random random,double min,double max) {
        return random.NextDouble() * (max - min) + min;
    }
}
