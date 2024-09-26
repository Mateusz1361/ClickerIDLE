using System;
using System.Linq;

public static class NumberFormat {
    public static string ShortForm(SafeUDecimal value) => ShortForm(value.Value / 100);

    public static string ShortForm(SafeUInteger value) {
        SafeUInteger counter = 1000;
        foreach(var suffix in numberSuffixes) {
            if(value < counter) {
                var newValue = value / (counter / 1000);
                return $"{newValue}{suffix}";
            }
            counter *= 1000;
        }
        var lastNewValue = value / (counter / 1000);
        return $"{lastNewValue}{numberSuffixes.Last()}";
    }
    public static readonly string[] numberSuffixes = { "","k","M","B","T","Q" };
}

public class MutablePair<T0,T1> {
    public T0 first;
    public T1 second;
}

public static class RandomExtensions {
    public static double NextDouble(this Random random,double min,double max) {
        return random.NextDouble() * (max - min) + min;
    }
}
