namespace raytracer;

public struct Interval
{
    public double Min;
    public double Max;
    public Interval()
    {
        Min = double.PositiveInfinity;
        Max = double.NegativeInfinity;
    }
    public Interval(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public readonly double Size()
    {
        return Max - Min;
    }

    public readonly bool Contains(double x)
    {
        return x >= Min && x <= Max;
    }

    public readonly bool Surrounds(double x)
    {
        return Min < x && x < Max;
    }

    public readonly double Clamp(double x)
    {
        if(x < Min) return Min;
        if(x > Max) return Max;
        return x;
    }

    public static readonly Interval Empty = new();
    public static readonly Interval Universe = new(double.NegativeInfinity, double.PositiveInfinity);
}