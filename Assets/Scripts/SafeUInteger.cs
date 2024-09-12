using System;
using System.Globalization;

public readonly struct SafeUInteger {
    public readonly ulong limb;

    public SafeUInteger(int _limb) {
        limb = (ulong)_limb;
    }

    public SafeUInteger(uint _limb) {
        limb = _limb;
    }

    public SafeUInteger(long _limb) {
        limb = (ulong)_limb;
    }

    public SafeUInteger(ulong _limb) {
        limb = _limb;
    }

    public static explicit operator SafeUInteger(int _limb) {
        return new(_limb);
    }

    public static implicit operator SafeUInteger(uint _limb) {
        return new(_limb);
    }

    public static explicit operator SafeUInteger(long _limb) {
        return new(_limb);
    }

    public static implicit operator SafeUInteger(ulong _limb) {
        return new(_limb);
    }

    public static explicit operator ulong(SafeUInteger value) {
        return value.limb;
    }

    public override string ToString() {
        return limb.ToString(CultureInfo.InvariantCulture);
    }

    public static SafeUInteger Parse(string value) {
        return new(ulong.Parse(value,CultureInfo.InvariantCulture));
    }

    public override bool Equals(object obj) {
        return obj is SafeUInteger integer && limb == integer.limb;
    }

    public override int GetHashCode() {
        return HashCode.Combine(limb);
    }

    public static SafeUInteger operator+(SafeUInteger a,SafeUInteger b) {
        if(a.limb <= ulong.MaxValue - b.limb) {
            return new(a.limb + b.limb);
        }
        return new(ulong.MaxValue);
    }

    public static SafeUInteger operator-(SafeUInteger a,SafeUInteger b) {
        if(a.limb >= b.limb) {
            return new(a.limb - b.limb);
        }
        return new(0);
    }

    public static SafeUInteger operator*(SafeUInteger a,SafeUInteger b) {
        if(a.limb == 0 || b.limb == 0) {
            return new(0);
        }
        if(a.limb <= ulong.MaxValue / b.limb) {
            return new(a.limb * b.limb);
        }
        return new(ulong.MaxValue);
    }

    public static SafeUInteger operator/(SafeUInteger a,SafeUInteger b) {
        if(b.limb == 0) {
            throw new DivideByZeroException("Dividing by zero isn't allowed.");
        }
        return new(a.limb / b.limb);
    }

    public static SafeUInteger operator%(SafeUInteger a,SafeUInteger b) {
        if(b.limb == 0) {
            throw new DivideByZeroException("Dividing by zero isn't allowed.");
        }
        return new(a.limb % b.limb);
    }

    public static bool operator==(SafeUInteger a,SafeUInteger b) {
        return a.limb == b.limb;
    }

    public static bool operator!=(SafeUInteger a,SafeUInteger b) {
        return a.limb != b.limb;
    }

    public static bool operator>(SafeUInteger a,SafeUInteger b) {
        return a.limb > b.limb;
    }

    public static bool operator<(SafeUInteger a,SafeUInteger b) {
        return a.limb < b.limb;
    }

    public static bool operator>=(SafeUInteger a,SafeUInteger b) {
        return a.limb >= b.limb;
    }

    public static bool operator<=(SafeUInteger a,SafeUInteger b) {
        return a.limb <= b.limb;
    }
}
