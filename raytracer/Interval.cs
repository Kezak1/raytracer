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

    public double Size()
    {
        return Max - Min;
    }

    public bool Contains(double x)
    {
        return x >= Min && x <= Max;
    }

    public bool Surrounds(double x)
    {
        return Min < x && x < Max;
    }

    public static readonly Interval Empty = new Interval();
    public static readonly Interval Universe = new Interval(double.NegativeInfinity, double.PositiveInfinity);
}