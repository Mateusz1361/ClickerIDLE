using System;
using System.Text;
using System.Numerics;
using System.Diagnostics;

public struct Rational {
    public BigInteger Num { get; private set; }
    public BigInteger Denom {  get; private set; }

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
        var wholePart = (ulong)Math.Truncate(value);
        value -= Math.Truncate(value);

        BigInteger fractNum = 0;
        BigInteger fractDenom = 1;

        while(Math.Abs(value) > 0.0001) {
            var tmp = (int)Math.Truncate(value * 10.0);
            fractNum += tmp * fractDenom;
            value -= tmp;
            fractDenom *= 10;
        }

        Num = sign * (wholePart * fractDenom + fractNum);
        Denom = fractDenom;
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
        return CanonicalForm(new() {
            Num = a.Num * b.Denom * (b.Num < 0 ? -1 : 1),
            Denom = a.Denom * BigInteger.Abs(b.Num)
        });
    }

    public static bool operator==(Rational a,Rational b) {
        return a.Num == b.Num && a.Denom == b.Denom;
    }

    public static bool operator!=(Rational a,Rational b) {
        return !(a == b);
    }

    public static bool operator<(Rational a,Rational b) {
        return a.Num * b.Denom < b.Num * a.Denom;
    }

    public static bool operator>(Rational a,Rational b) {
        return a.Num * b.Denom > b.Num * a.Denom;
    }

    public static bool operator<=(Rational a,Rational b) {
        return a < b || a == b;
    }

    public static bool operator>=(Rational a,Rational b) {
        return a > b || a == b;
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
        builder.Append(",");
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
        if(numerator >= denominator) {
            var div = BigInteger.DivRem(numerator,denominator,out var rem);
            var str = DecimalExpansionToString(rem,denominator,precision);
            return numerator < 0 ? $"-{div}{str}" : $"{div}{str}";
        }
        var str2 = DecimalExpansionToString(BigInteger.Abs(numerator),denominator,precision);
        return numerator < 0 ? $"-0{str2}" : $"0{str2}";
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
}
