using System.Reflection.Metadata;

namespace raytracer;

public static class Extensions {
    public static void WriteColor(Color c)
    {
        Interval intensity = new(0.000, 0.999);
        int rByte = (int)(256 * intensity.Clamp(c.X));
        int gByte = (int)(256 * intensity.Clamp(c.Y));
        int bByte = (int)(256 * intensity.Clamp(c.Z));

        Console.WriteLine($"{rByte} {gByte} {bByte}");
    }

    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static readonly Random random = new();
    public static double RandomDouble()
    {
        return random.NextDouble();
    }
    public static double RandomeDouble(double min, double max)
    {
        return min + (max - min) * RandomDouble();
    }
}