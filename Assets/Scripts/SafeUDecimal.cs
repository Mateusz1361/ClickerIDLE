using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;

public struct SafeUDecimal {
    public SafeUInteger Value { get; private set; }

    public static readonly SafeUDecimal MaxValue = new() { Value = SafeUInteger.MaxValue };
    public static readonly SafeUDecimal MinValue = new() { Value = SafeUInteger.MinValue };

    public SafeUDecimal(SafeUInteger value) {
        Value = value * 100;
    }

    public SafeUDecimal(double value) {
        //@TODO: This seems inaccurate.
        Value = ((SafeUInteger)(ulong)Math.Truncate(value)) * 100;
        Value += (ulong)Math.Truncate((value - Math.Truncate(value)) * 100);
        Value = (value <= 0.0) ? 0 : Value;
    }

    public static SafeUDecimal CentiUnits(SafeUInteger numerator) {
        return new() { Value = numerator };
    }

    public static explicit operator SafeUDecimal(int integer) {
        return new((SafeUInteger)integer);
    }

    public static implicit operator SafeUDecimal(uint integer) {
        return new((SafeUInteger)integer);
    }

    public static explicit operator SafeUDecimal(long integer) {
        return new((SafeUInteger)integer);
    }

    public static implicit operator SafeUDecimal(ulong integer) {
        return new((SafeUInteger)integer);
    }

    public static explicit operator SafeUDecimal(double @float) {
        return new(@float);
    }

    public static implicit operator SafeUDecimal(SafeUInteger integer) {
        return new(integer);
    }

    public static explicit operator SafeUInteger(SafeUDecimal @decimal) {
        return @decimal.Value / 100;
    }

    public static explicit operator double(SafeUDecimal @decimal) {
        return (ulong)@decimal.Value / 100.0;
    }

    public override readonly string ToString() {
        var part = Value % 100;
        var format = (part == 0) ? "{0}" : "{0}.{1}";
        return string.Format(CultureInfo.InvariantCulture,format,Value / 100,part.ToString().PadLeft(2,'0'));
    }

    public static SafeUDecimal Parse(string input) {
        Trace.Assert(input != null);
        input = input.TrimStart();
        int decimalPointPos = -1;
        for(int i = 0;i < input.Length;i += 1) {
            if(input[i] == '.') {
                if(decimalPointPos != -1) {
                    throw new ArgumentException("There can be at most one decimal point.");
                }
                decimalPointPos = i;
            }
            else if(!char.IsDigit(input[i])) {
                throw new ArgumentException($"'{input[i]}' isn't a digit.");
            }
        }
        SafeUDecimal @decimal = new();
        if(decimalPointPos == -1) {
            @decimal.Value = SafeUInteger.Parse(input) * 100;
        }
        else {
            if(decimalPointPos > 0) {
                @decimal.Value += SafeUInteger.Parse(input[..decimalPointPos]) * 100;
            }
            if(decimalPointPos < input.Length - 1) {
                int maxLen = Math.Min(2,input.Length - 1 - decimalPointPos);
                @decimal.Value += SafeUInteger.Parse(input.Substring(decimalPointPos + 1,maxLen).PadRight(2,'0'));
            }
        }
        return @decimal;
    }

    public override readonly bool Equals(object obj) {
        return obj is SafeUDecimal @decimal && EqualityComparer<SafeUInteger>.Default.Equals(Value,@decimal.Value);
    }

    public override readonly int GetHashCode() {
        return HashCode.Combine(Value);
    }

    public static SafeUDecimal operator+(SafeUDecimal left,SafeUDecimal right) {
        return new() { Value = left.Value + right.Value };
    }

    public static SafeUDecimal operator-(SafeUDecimal left,SafeUDecimal right) {
        return new() { Value = left.Value - right.Value };
    }

    public static SafeUDecimal operator*(SafeUDecimal left,SafeUDecimal right) {
        return new() { Value = (left.Value * right.Value) / 100 };
    }

    public static SafeUDecimal operator/(SafeUDecimal left,SafeUDecimal right) {
        return new() { Value = (left.Value * 100) / right.Value };
    }

    public static bool operator==(SafeUDecimal left,SafeUDecimal right) {
        return left.Value == right.Value;
    }

    public static bool operator!=(SafeUDecimal left,SafeUDecimal right) {
        return left.Value != right.Value;
    }

    public static bool operator>(SafeUDecimal left,SafeUDecimal right) {
        return left.Value > right.Value;
    }

    public static bool operator<(SafeUDecimal left,SafeUDecimal right) {
        return left.Value < right.Value;
    }

    public static bool operator>=(SafeUDecimal left,SafeUDecimal right) {
        return left.Value >= right.Value;
    }

    public static bool operator<=(SafeUDecimal left,SafeUDecimal right) {
        return left.Value <= right.Value;
    }

    public static SafeUDecimal Truncate(SafeUDecimal value) {
        return new() {Value = (value.Value / 100) * 100 };
    }
}
