namespace raytracer;

public class Camera
{
    public double AspectRatio = 1.0;
    public int ImageWidth = 100;
    public int SamplesPerPixel = 10;
    public int MaxDepth = 10;
    public double VFov = 90;
    public Point3 LookFrom = new(0, 0, 0);
    public Point3 LookAt = new(0, 0, -1);
    public Vec3 VUp = new(0, 1, 0);

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
    private Vec3 U, V, W;

    private void Initialize()
    {
        ImageHeight = (int)(ImageWidth / AspectRatio);
        if(ImageHeight < 1)
        {
            ImageHeight = 1;
        }

        PixelSamplesScale = 1.0 / SamplesPerPixel;

        Center = LookFrom;
        var focalLenght = (LookFrom - LookAt).Lenght();
        var theta = Extensions.DegreesToRadians(VFov);
        var h = Math.Tan(theta / 2);
        var viewportHeight = 2 * h * focalLenght;
        var viewportWidth = viewportHeight * ((double)(ImageWidth) / ImageHeight);

        W = Vec3.UnitVector(LookFrom - LookAt);
        U = Vec3.UnitVector(Vec3.Cross(VUp, W));
        V = Vec3.Cross(W, U);


        var viewportU = viewportWidth * U;
        var viewportV = viewportHeight * -V;

        PixelDeltaU = viewportU / ImageWidth;
        PixelDeltaV = viewportV / ImageHeight;

        var viewportUpperLeft = Center - (focalLenght * W) - viewportU / 2 - viewportV / 2;
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