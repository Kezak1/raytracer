global using Point3 = raytracer.Vec3;
global using Color = raytracer.Vec3;
using System.Net.Http.Headers;

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

    static void CreateScene1()
    {
        HittableList world = new();

        var materialGround = new Lambertian(new Color(0.8, 0.8, 0.0));
        var materialCenter = new Lambertian(new Color(0.1, 0.2, 0.5));
        var materialLeft = new Dielectric(1.50);
        var materialBubble = new Dielectric(1.00 / 1.50);
        var materialRight = new Metal(new Color(0.8, 0.6, 0.2), 1.0);
        
        world.Add(new Sphere(new Point3(0.0, -100.5, -1.0), 100, materialGround));
        world.Add(new Sphere(new Point3(0.0, 0.0, -1.2), 0.5, materialCenter));
        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.5, materialLeft));
        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.4, materialBubble));
        world.Add(new Sphere(new Point3(1.0, 0.0, -1.0), 0.5, materialRight));

        Camera cam = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            SamplesPerPixel = 100,
            MaxDepth = 50,
            VFov = 20,
            LookFrom = new Point3(-2, 2, 1),
            LookAt = new Point3(0, 0, -1),
            VUp = new Vec3(0, 1, 0),
            DefocusAngle = 10.0,
            FocusDist = 3.4
        };

        cam.Render(world);
    }

    static void CreateScene2()
    {
        HittableList world = new();

        var R = Math.Cos(Math.PI/4);
        
        var materialLeft = new Lambertian(new Color(0, 0, 1));
        var materialRight = new Lambertian(new Color(1, 0, 0));

        world.Add(new Sphere(new Point3(-R, 0, -1), R, materialLeft));
        world.Add(new Sphere(new Point3(R, 0, -1), R, materialRight));

        Camera cam = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 400,
            SamplesPerPixel = 100,
            MaxDepth = 50,
            VFov = 90
        };

        cam.Render(world);
    }

    static void CreateFinalScene()
    {
        HittableList world = new();
        
        var materialGround = new Lambertian(new Color(0.5, 0.5, 0.5));
        world.Add(new Sphere(new Point3(0, -1000, 0), 1000, materialGround));

        for(int a = -11; a < 11; a++)
        {
            for(int b = -11; b < 11; b++)
            {
                var chooseMat = Extensions.RandomDouble();
                Point3 center = new(a + 0.9 * Extensions.RandomDouble(), 0.2, b + 0.9 * Extensions.RandomDouble());

                if((center - new Point3(4, 0.2, 0)).Lenght() > 0.9)
                {
                    Material sphereMaterial;

                    if(chooseMat < 0.8)
                    {
                        var albedo = Color.Random() * Color.Random();
                        sphereMaterial = new Lambertian(albedo);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));
                    } 
                    else if(chooseMat < 0.95)
                    {
                        var albedo = Color.Random(0.5, 1);
                        var fuzz = Extensions.RandomDouble();
                        sphereMaterial = new Metal(albedo, fuzz);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));
                    }
                    else
                    {
                        sphereMaterial = new Dielectric(1.5);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));
                    }
                }
            }
        }

        var material1 = new Dielectric(1.5);
        world.Add(new Sphere(new Point3(0, 1, 0), 1.0, material1));

        var material2 = new Lambertian(new Color(0.4, 0.2, 0.1));
        world.Add(new Sphere(new Point3(-4, 1, 0), 1.0, material2));

        var material3 = new Metal(new Color(0.7, 0.6, 0.5), 0.0);
        world.Add(new Sphere(new Point3(4, 1, 0), 1.0, material3));

        Camera cam = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 1200,
            SamplesPerPixel = 50,
            MaxDepth = 50,
            VFov = 20,
            LookFrom = new Point3(13, 2, 3),
            LookAt = new Point3(0, 0, 0),
            VUp = new Vec3(0, 1, 0),
            DefocusAngle = 0.6,
            FocusDist = 10.0
        };

        cam.Render(world);
    }

    static void Main()
    {
        CreateFinalScene();
    } 
}