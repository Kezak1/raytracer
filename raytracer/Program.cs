global using Point3 = raytracer.Vec3;
global using Color = raytracer.Vec3;

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
        Console.Error.WriteLine("Done!");
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

                if((center - new Point3(4, 0.2, 0)).Length() > 0.9)
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

        BVHNode bvhWorld = new(world.Objects, 0, world.Objects.Count);
        Camera cam = new()
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 1920,
            SamplesPerPixel = 500,
            MaxDepth = 50,
            VFov = 20,
            LookFrom = new Point3(13, 2, 3),
            LookAt = new Point3(0, 0, 0),
            VUp = new Vec3(0, 1, 0),
            DefocusAngle = 0.6,
            FocusDist = 10.0
        };

        cam.Render(bvhWorld);
    }

    static void Main()
    {
        CreateFinalScene();
    } 
}