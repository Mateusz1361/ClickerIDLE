using System;
using System.Globalization;

[Serializable]
public readonly struct SafeInteger {
    private readonly ulong limb;

    public SafeInteger(int _limb) {
        limb = (ulong)_limb;
    }

    public SafeInteger(long _limb) {
        limb = (ulong)_limb;
    }

    public SafeInteger(ulong _limb) {
        limb = _limb;
    }

    public static explicit operator SafeInteger(int _limb) {
        return new(_limb);
    }

    public static explicit operator SafeInteger(long _limb) {
        return new(_limb);
    }

    public static implicit operator SafeInteger(ulong _limb) {
        return new(_limb);
    }

    public static explicit operator ulong(SafeInteger value) {
        return value.limb;
    }

    public override string ToString() {
        return limb.ToString(CultureInfo.InvariantCulture);
    }

    public static SafeInteger Parse(string value) {
        return new(ulong.Parse(value,CultureInfo.InvariantCulture));
    }

    public override bool Equals(object obj) {
        return obj is SafeInteger integer && limb == integer.limb;
    }

    public override int GetHashCode() {
        return HashCode.Combine(limb);
    }

    public static SafeInteger operator+(SafeInteger a,SafeInteger b) {
        if(a.limb <= ulong.MaxValue - b.limb) {
            return new(a.limb + b.limb);
        }
        return new(ulong.MaxValue);
    }

    public static SafeInteger operator-(SafeInteger a,SafeInteger b) {
        if(a.limb >= b.limb) {
            return new(a.limb - b.limb);
        }
        return new(0);
    }

    public static SafeInteger operator*(SafeInteger a,SafeInteger b) {
        if(a.limb == 0 || b.limb == 0) {
            return new(0);
        }
        if(a.limb <= ulong.MaxValue / b.limb) {
            return new(a.limb * b.limb);
        }
        return new(ulong.MaxValue);
    }

    public static SafeInteger operator/(SafeInteger a,SafeInteger b) {
        if(b.limb == 0) {
            throw new DivideByZeroException("Dividing by zero isn't allowed.");
        }
        return new(a.limb / b.limb);
    }

    public static SafeInteger operator%(SafeInteger a,SafeInteger b) {
        if(b.limb == 0) {
            throw new DivideByZeroException("Dividing by zero isn't allowed.");
        }
        return new(a.limb % b.limb);
    }

    public static bool operator==(SafeInteger a,SafeInteger b) {
        return a.limb == b.limb;
    }

    public static bool operator!=(SafeInteger a,SafeInteger b) {
        return a.limb != b.limb;
    }

    public static bool operator>(SafeInteger a,SafeInteger b) {
        return a.limb > b.limb;
    }

    public static bool operator<(SafeInteger a,SafeInteger b) {
        return a.limb < b.limb;
    }

    public static bool operator>=(SafeInteger a,SafeInteger b) {
        return a.limb >= b.limb;
    }

    public static bool operator<=(SafeInteger a,SafeInteger b) {
        return a.limb <= b.limb;
    }
}
