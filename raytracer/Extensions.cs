namespace raytracer;

public static class Extensions {
    public static double LinearToGamma(double linearComponent)
    {
        if(linearComponent > 0)
        {
            return Math.Sqrt(linearComponent);
        }
        return 0;
    }

    public static void WriteColor(Color c)
    {
        Interval intensity = new(0.000, 0.999);
        int rByte = (int)(256 * intensity.Clamp(LinearToGamma(c.X)));
        int gByte = (int)(256 * intensity.Clamp(LinearToGamma(c.Y)));
        int bByte = (int)(256 * intensity.Clamp(LinearToGamma(c.Z)));

        Console.WriteLine($"{rByte} {gByte} {bByte}");
    }

    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    [ThreadStatic]
    private static Random? Random;
    public static double RandomDouble()
    {
        Random ??= new Random();
        return Random.NextDouble();
    }
    public static double RandomDouble(double min, double max)
    {
        return min + (max - min) * RandomDouble();
    }

    public static int RandomInt(int min, int max)
    {
        Random ??= new Random();
        return Random.Next(min, max + 1);
    }
}