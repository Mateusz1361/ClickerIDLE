using System;
using System.Text;
using System.Numerics;
using System.Diagnostics;

public struct Rational {
    public BigInteger Num { get; private set; }
    public BigInteger Denom { get; private set; }

    public Rational(int value) {
        Num = value;
        Denom = 1;
    }

    public Rational(long value) {
        Num = value;
        Denom = 1;
    }

    public Rational(ulong value) {
        Num = value;
        Denom = 1;
    }

    public Rational(double value) {
        int sign = Math.Sign(value);
        value = Math.Abs(value);
        var wholePart = (long)Math.Truncate(value);
        value -= wholePart;

        BigInteger fractNum = 0;
        BigInteger fractDenom = 1;
        if(Math.Abs(value) > 0.00001) {
            //@TODO: This is slow.
            var str = value.ToString()[2..];
            fractNum = BigInteger.Parse(str);
            fractDenom = BigInteger.Pow(10,str.Length);
        }

        Num = sign * (wholePart * fractDenom + fractNum);
        Denom = fractDenom;
        ToCanonicalForm();
    }

    public Rational(long _num,long _denom) {
        Trace.Assert(_denom > 0);
        Num = _num;
        Denom = _denom;
        ToCanonicalForm();
    }

    public Rational(BigInteger _num,BigInteger _denom) {
        Trace.Assert(_denom > 0);
        Num = _num;
        Denom = _denom;
        ToCanonicalForm();
    }

    public Rational(BigInteger _num) {
        Num = _num;
        Denom = 1;
    }

    public static implicit operator Rational(int value) {
        return new(value);
    }

    public static implicit operator Rational(long value) {
        return new(value);
    }

    public static implicit operator Rational(ulong value) {
        return new(value);
    }

    public static implicit operator Rational(BigInteger value) {
        return new(value);
    }

    public static explicit operator int(Rational value) {
        return (int)(value.Num / value.Denom);
    }

    public static explicit operator long(Rational value) {
        return (long)(value.Num / value.Denom);
    }

    public static explicit operator ulong(Rational value) {
        return (ulong)(value.Num / value.Denom);
    }

    public static explicit operator BigInteger(Rational value) {
        return value.Num / value.Denom;
    }

    private static Rational CanonicalForm(Rational value) {
        var div = BigInteger.GreatestCommonDivisor(value.Num,value.Denom);
        return new(value.Num / div,value.Denom / div);
    }

    private void ToCanonicalForm() {
        var div = BigInteger.GreatestCommonDivisor(Num,Denom);
        Num /= div;
        Denom /= div;
    }

    public static Rational operator+(Rational v) {
        return new(v.Num,v.Denom);
    }

    public static Rational operator-(Rational v) {
        return new(-v.Num,v.Denom);
    }

    public static Rational operator+(Rational a,Rational b) {
        return CanonicalForm(new() {
            Num = a.Num * b.Denom + b.Num * a.Denom,
            Denom = a.Denom * b.Denom
        });
    }

    public static Rational operator-(Rational a,Rational b) {
        return CanonicalForm(new() {
            Num = a.Num * b.Denom - b.Num * a.Denom,
            Denom = a.Denom * b.Denom
        });
    }

    public static Rational operator*(Rational a,Rational b) {
        return CanonicalForm(new() {
            Num = a.Num * b.Num,
            Denom = a.Denom * b.Denom
        });
    }

    public static Rational operator/(Rational a,Rational b) {
        Trace.Assert(b.Num != 0);
        int sign = b.Num < 0 ? -1 : 1;
        return CanonicalForm(new() {
            Num = a.Num * b.Denom * sign,
            Denom = a.Denom * BigInteger.Abs(b.Num)
        });
    }

    public static bool operator==(Rational a,Rational b) {
        return a.Num == b.Num && a.Denom == b.Denom;
    }

    public static bool operator!=(Rational a,Rational b) {
        return a.Num != b.Num || a.Denom != b.Denom;
    }

    public static bool operator<(Rational a,Rational b) {
        return a.Num * b.Denom < b.Num * a.Denom;
    }

    public static bool operator>(Rational a,Rational b) {
        return a.Num * b.Denom > b.Num * a.Denom;
    }

    public static bool operator<=(Rational a,Rational b) {
        return a.Num * b.Denom <= b.Num * a.Denom;
    }

    public static bool operator>=(Rational a,Rational b) {
        return a.Num * b.Denom >= b.Num * a.Denom;
    }

    public override readonly string ToString() {
        return ToString(Num,Denom,2);
    }

    public readonly string ToString(uint precision) {
        return ToString(Num,Denom,precision);
    }

    private static string DecimalExpansionToString(BigInteger numerator,BigInteger denominator,uint precision) {
        Trace.Assert(numerator < denominator);
        if(precision == 0 || numerator == 0) {
            return "";
        }
        StringBuilder builder = new();
        builder.Append(".");
        for(uint i = 0;i < precision && numerator > 0;i += 1) {
            var tmp = numerator * 10;
            var div = tmp / denominator;
            builder.Append(div);
            numerator = tmp - div * denominator;
        }
        return builder.ToString();
    }

    private static string ToString(BigInteger numerator,BigInteger denominator,uint precision) {
        if(numerator == 0) {
            return "0";
        }
        bool isNegative = (numerator < 0);
        numerator = BigInteger.Abs(numerator);
        if(numerator >= denominator) {
            var div = BigInteger.DivRem(numerator,denominator,out var rem);
            var str = DecimalExpansionToString(rem,denominator,precision);
            return isNegative ? $"-{div}{str}" : $"{div}{str}";
        }
        var str2 = DecimalExpansionToString(numerator,denominator,precision);
        return isNegative ? $"-0{str2}" : $"0{str2}";
    }

    public readonly override bool Equals(object obj) {
        return base.Equals(obj);
    }

    public readonly override int GetHashCode() {
        return base.GetHashCode();
    }

    public static Rational Abs(Rational value) {
        return new(BigInteger.Abs(value.Num),value.Denom);
    }

    public static Rational Truncate(Rational value) {
        return new(value.Num / value.Denom);
    }

    public static Rational Parse(string str) {
        if(string.IsNullOrEmpty(str)) {
            throw new ArgumentNullException();
        }
        bool isNegative = false;
        str = str.TrimStart(' ');
        if(str[0] == '+' || str[0] == '-') {
            isNegative = (str[0] == '-');
            str = str.Remove(0,1);
        }
        int decimalPointIndex = str.Length;
        for(int i = 0;i < str.Length;i += 1) {
            if(str[i] == '.') {
                if(decimalPointIndex != str.Length) {
                    throw new FormatException($"More than one decimal separator.");
                }
                decimalPointIndex = i;
            }
            else if(!char.IsDigit(str[i])) {
                throw new FormatException($"'{str[i]}' isn't a digit.");
            }
        }
        BigInteger integerPart = 0;
        BigInteger decimalPartNum = 0;
        BigInteger decimalPartDenom = 1;
        if(decimalPointIndex > 0) {
            integerPart = BigInteger.Parse(str[..decimalPointIndex]);
        }
        if(decimalPointIndex < str.Length - 1) {
            decimalPartNum = BigInteger.Parse(str[(decimalPointIndex + 1)..]);
            decimalPartDenom = BigInteger.Pow(10,str.Length - (decimalPointIndex + 1));
        }
        return new((isNegative ? -1 : 1) * (integerPart * decimalPartDenom + decimalPartNum),decimalPartDenom);
    }
}
