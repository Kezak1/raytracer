global using Point3 = raytracer.Vec3;
global using Color = raytracer.Vec3;
using System.Runtime.InteropServices.Marshalling;

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

        var materialGround = new Lambertian(new Color(0.8, 0.8, 0.0));
        var materialCenter = new Lambertian(new Color(0.1, 0.2, 0.5));
        var materialLeft = new Metal(new Color(0.8, 0.8, 0.8), 0.3);
        var materialRight = new Metal(new Color(0.8, 0.6, 0.2), 1.0);
        
        world.Add(new Sphere(new Point3(0.0, -100.5, -1.0), 100, materialGround));
        world.Add(new Sphere(new Point3(0.0, 0.0, -1.2), 0.5, materialCenter));
        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.5, materialLeft));
        world.Add(new Sphere(new Point3(1.0, 0.0, -1.0), 0.5, materialRight));

        Camera cam = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            SamplesPerPixel = 100,
            MaxDepth = 50
        };

        cam.Render(world);
    }

    static void Main()
    {
        // Console.WriteLine("Hello World!");
        // CreateImage();
        CreateScene();
    } 
}