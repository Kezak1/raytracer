global using Point3 = raytracer.Vec3;
global using Color = raytracer.Vec3;
using Microsoft.VisualBasic;

namespace raytracer;

class Program
{
    static void CreateImage()
    {
        int imageWidth = 256;
        int imageHeight = 256;
        
        Console.Write($"P3\n{imageWidth} {imageHeight}\n255\n");

        for (int j = 0; j < imageHeight; j++)
        {
            Console.Error.Write($"\rScanlines remaining: {imageHeight - j} \n");
            Console.Error.Flush();
            for (int i = 0; i < imageWidth; i++)
            {
                var pixelColor = new Color((double)i/(imageWidth-1), (double)j/(imageHeight-1), 0);
                Extensions.WriteColor(pixelColor);
            }
        }
        Console.WriteLine("Done!");
    }

    static void CreateScene()
    {
        HittableList world = new();
        world.Add(new Sphere(new Point3(0, 0, -1), 0.5));
        world.Add(new Sphere(new Point3(0, -100.5, -1), 100));

        Camera cam = new();
        cam.AspectRatio = 16.0 / 9.0;
        cam.ImageWidth = 400;

        cam.Render(world);
    }

    static void Main()
    {
        // Console.WriteLine("Hello World!");
        // CreateImage();
        CreateScene();
    }
}