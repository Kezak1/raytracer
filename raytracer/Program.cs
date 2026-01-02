using System;

namespace raytracer;
class Program
{
    private static void CreateImage()
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
                var r = (double)i / (imageWidth - 1);
                var g = (double)j / (imageHeight - 1);
                var b = 0.0;

                int ir = (int)(255.999 * r);
                int ig = (int)(255.999 * g);
                int ib = (int)(255.999 * b);
                
                Console.Write($"{ir} {ig} {ib}\n");
            }
        }
        Console.WriteLine("Done!");
    }
    static void Main()
    {
        // Console.WriteLine("Hello World!");
        CreateImage();
    }
}