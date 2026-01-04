namespace raytracer;

public struct Ray
{   
    public Point3 Origin { get; }
    public Vec3 Direction { get; }

    public Ray(Point3 orig, Vec3 dir)
    {
        Origin = orig;
        Direction = dir;
    }

    public readonly Point3 At(double t)
    {
        return Origin + t*Direction;
    }
}