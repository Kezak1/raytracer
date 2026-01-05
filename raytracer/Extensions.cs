namespace raytracer;

public static class Extensions {
    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    } 
    public static void WriteColor(Color c)
    {
        int rByte = (int)(255.999 * c.X);
        int gByte = (int)(255.999 * c.Y);
        int bByte = (int)(255.999 * c.Z);

        Console.WriteLine($"{rByte} {gByte} {bByte}");
    }
}