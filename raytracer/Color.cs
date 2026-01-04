namespace raytracer;

using Color = Vec3;

public static class ColorExtension {
    public static void WriteColor(Color c)
    {
        int rByte = (int)(255.999 * c.X);
        int gByte = (int)(255.999 * c.Y);
        int bByte = (int)(255.999 * c.Z);

        Console.WriteLine($"{rByte} {gByte} {bByte}\n");
    }
}