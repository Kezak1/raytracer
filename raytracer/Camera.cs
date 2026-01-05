namespace raytracer;

public class Camera
{
    public double AspectRatio = 1.0;
    public int ImageWidth = 100;
    public int SamplesPerPixel = 10;
    public int MaxDepth = 10;

    public void Render(Hittable world)
    {
        Initialize();
        Console.Write($"P3\n{ImageWidth} {ImageHeight}\n255\n");
        for (int j = 0; j < ImageHeight; j++)
        {
            Console.Error.Write($"\rScanlines remaining: {ImageHeight - j} \n");
            Console.Error.Flush();

            
            for (int i = 0; i < ImageWidth; i++)
            {
                Color pixelColor = new(0, 0, 0);
                for(int sample = 0; sample < SamplesPerPixel; sample++)
                {
                    Ray r = GetRay(i, j);
                    pixelColor += RayColor(r, MaxDepth, world);
                }

                Extensions.WriteColor(pixelColor * PixelSamplesScale);
            }
        }
        Console.WriteLine("Done!");
    }
    
    private int ImageHeight;
    private double PixelSamplesScale;
    private Point3 Center;
    private Point3 Pixel100Loc;
    private Vec3 PixelDeltaU;
    private Vec3 PixelDeltaV;

    private void Initialize()
    {
        ImageHeight = (int)(ImageWidth / AspectRatio);
        if(ImageHeight < 1)
        {
            ImageHeight = 1;
        }

        PixelSamplesScale = 1.0 / SamplesPerPixel;

        Center = new Point3(0, 0, 0);
        var focalLenght = 1.0;
        var viewportHeight = 2.0;
        var viewportWidth = viewportHeight * ((double)(ImageWidth) / ImageHeight);

        var viewportU = new Vec3(viewportWidth, 0, 0);
        var viewportV = new Vec3(0, -viewportHeight, 0);

        PixelDeltaU = viewportU / ImageWidth;
        PixelDeltaV = viewportV / ImageHeight;

        var viewportUpperLeft = Center - new Vec3(0, 0, focalLenght) - (viewportU / 2) - (viewportV / 2);

        Pixel100Loc = viewportUpperLeft + (0.5 * (PixelDeltaU + PixelDeltaV));
    }

    private Vec3 SampleSquare()
    {
        return new Vec3(Extensions.RandomDouble() - 0.5, Extensions.RandomDouble() - 0.5, 0);
    }

    private Ray GetRay(int i, int j)
    {
        var offset = SampleSquare();
        var pixelSmaple = Pixel100Loc + ((i + offset.X) * PixelDeltaU) + ((j + offset.Y) * PixelDeltaV);

        var rayOrigin = Center;
        var rayDirection = pixelSmaple - rayOrigin;

        return new Ray(rayOrigin, rayDirection);
    }

    private Color RayColor(Ray r, int depth, Hittable world)
    {
        if(depth <= 0) {
            return new Color(0, 0, 0);
        }
        HitRecord rec = new();
        if(world.Hit(r, new Interval(0.001, double.PositiveInfinity), ref rec))
        {
            if(rec.Mat.Scatter(r, rec, out Color attenuation, out Ray scattered))
            {
                return attenuation * RayColor(scattered, depth - 1, world);
            }
            return new Color(0, 0, 0);
        }
        
        Vec3 unitDirection = Vec3.UnitVector(r.Direction);
        var a = 0.5*(unitDirection.Y + 1.0);
        return (1.0-a) * new Color(1.0, 1.0, 1.0) + a * new Color(0.5, 0.7, 1.0);
    }
}