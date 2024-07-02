using System;
using System.Globalization;

public static class NumberFormat {
    public static string Format(ulong value) {
        ulong bound = 1000;
        string[] suffixes = {"","k","M","B","T"};
        for(int i = 1;i < suffixes.Length;i += 1) {
            if(value < bound) {
                double div = bound / 1000.0;
                return string.Format(new CultureInfo("en-EN"),"{0}{1}",Math.Round(value / div * 100.0) / 100.0,suffixes[i - 1]);
            }
            bound *= 1000;
        }
        return string.Format(new CultureInfo("en-EN"),"{0}{1}",Math.Round(value / bound * 100.0) / 100.0,suffixes[^1]);
    }
}
