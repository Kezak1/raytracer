using System.Diagnostics;
using System.Reflection;

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
    public double DefocusAngle = 0;
    public double FocusDist = 10;

    public void Render(Hittable world)
    {
        var sw = Stopwatch.StartNew();
        
        Initialize();
        Console.Write($"P3\n{ImageWidth} {ImageHeight}\n255\n");

        var pixels = new Color[ImageHeight, ImageWidth];
        int count = 0;
        Parallel.For(0, ImageHeight, j =>
        {
            for (int i = 0; i < ImageWidth; i++)
            {
                Color pixelColor = new(0, 0, 0);
                for(int sample = 0; sample < SamplesPerPixel; sample++)
                {
                    Ray r = GetRay(i, j);
                    pixelColor += RayColor(r, MaxDepth, world);
                }

                pixels[j, i] = pixelColor * PixelSamplesScale;
            }

            int current = Interlocked.Increment(ref count);
            DrawProgressBar(count, ImageHeight);
        });

        for(int j = 0; j < ImageHeight; j++)
        {
            for(int i = 0; i < ImageWidth; i++)
            {
                Extensions.WriteColor(pixels[j, i]);
            }
        }

        sw.Stop();
        Console.Error.WriteLine($"\nRender time: {sw.Elapsed}");
    }

    private static void DrawProgressBar(int completed, int total)
    {
        int barWidth = 50;
        double progress = (double)completed / total;
        int percentages = (int)double.Floor(progress * 100);
        int filledWidth = (int)(barWidth * progress);
        
        Console.Error.Write("\r[");
        Console.Error.Write(new string('#', filledWidth));
        Console.Error.Write(new string(' ', barWidth - filledWidth));
        Console.Error.Write($"] {completed}/{total} ({percentages}%)");
    }
    
    private int ImageHeight;
    private double PixelSamplesScale;
    private Point3 Center;
    private Point3 Pixel100Loc;
    private Vec3 PixelDeltaU;
    private Vec3 PixelDeltaV;
    private Vec3 U, V, W;
    private Vec3 DefocusDiskU;
    private Vec3 DefocusDiskV;

    private void Initialize()
    {
        ImageHeight = (int)(ImageWidth / AspectRatio);
        if(ImageHeight < 1)
        {
            ImageHeight = 1;
        }

        PixelSamplesScale = 1.0 / SamplesPerPixel;

        Center = LookFrom;
        var theta = Extensions.DegreesToRadians(VFov);
        var h = Math.Tan(theta / 2);
        var viewportHeight = 2 * h * FocusDist;
        var viewportWidth = viewportHeight * ((double)ImageWidth / ImageHeight);

        W = Vec3.UnitVector(LookFrom - LookAt);
        U = Vec3.UnitVector(Vec3.Cross(VUp, W));
        V = Vec3.Cross(W, U);

        var viewportU = viewportWidth * U;
        var viewportV = viewportHeight * -V;

        PixelDeltaU = viewportU / ImageWidth;
        PixelDeltaV = viewportV / ImageHeight;

        var viewportUpperLeft = Center - (FocusDist * W) - viewportU / 2 - viewportV / 2;
        Pixel100Loc = viewportUpperLeft + (0.5 * (PixelDeltaU + PixelDeltaV));

        var defocusRadius = FocusDist * Math.Tan(Extensions.DegreesToRadians(DefocusAngle / 2));
        DefocusDiskU = U * defocusRadius;
        DefocusDiskV = V * defocusRadius;
    }

    private Vec3 SampleSquare()
    {
        return new Vec3(Extensions.RandomDouble() - 0.5, Extensions.RandomDouble() - 0.5, 0);
    }

    private Point3 DefocusSamepleDisk()
    {
        var p = Vec3.RandomInUnitDisk();
        return Center + (p.X * DefocusDiskU) + (p.Y * DefocusDiskV);
    }

    private Ray GetRay(int i, int j)
    {
        var offset = SampleSquare();
        var pixelSmaple = Pixel100Loc + ((i + offset.X) * PixelDeltaU) + ((j + offset.Y) * PixelDeltaV);

        var rayOrigin = (DefocusAngle <= 0) ? Center : DefocusSamepleDisk();
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