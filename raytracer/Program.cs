namespace raytracer;

using Color = Vec3;
using Point3 = Vec3;

class Program
{
    static Color RayColor(Ray r)
    {
        return new Color(0.1, 0.2, 0.3);
    }
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
                ColorExtension.WriteColor(pixelColor);
            }
        }
        Console.WriteLine("Done!");
    }

    static void CreateScene()
    {
        var aspectRatio = 16.0 / 9.0;
        int imageWidth = 400;
        int imageHeight = (int)(imageWidth / aspectRatio);

        var focalLenght = 1.0;
        var viewportHeight = 2.0;
        var viewportWidth = viewportHeight * ((double)(imageWidth) / imageHeight);
        var cameraCenter = new Point3(0, 0, 0);

        var viewportU = new Vec3(viewportWidth, 0, 0);
        var viewportV = new Vec3(0, -viewportHeight, 0);

        var pixelDeltaU = viewportU / imageWidth;
        var pixelDeltaV = viewportV / imageHeight;

        var viewportUpperLeft = cameraCenter - new Vec3(0, 0, focalLenght) - (viewportU / 2) - (viewportV / 2);
        var pixel00Loc = viewportUpperLeft + (0.5 * (pixelDeltaU + pixelDeltaV));

        Console.Write($"P3\n{imageWidth} {imageHeight}\n255\n");
        for (int j = 0; j < imageHeight; j++)
        {
            Console.Error.Write($"\rScanlines remaining: {imageHeight - j} \n");
            Console.Error.Flush();
            for (int i = 0; i < imageWidth; i++)
            {
                var pixelCenter = pixel00Loc + (i * pixelDeltaU) + (j * pixelDeltaV);
                var rayDirection = pixelCenter - cameraCenter;
                Ray r = new Ray(cameraCenter, rayDirection);

                var pixelColor = RayColor(r);
                ColorExtension.WriteColor(pixelColor);
            }
        }
        Console.WriteLine("Done!");
    }
    static void Main()
    {
        // Console.WriteLine("Hello World!");
        // CreateImage();
        CreateScene();
    }
}