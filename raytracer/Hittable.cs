namespace raytracer;

public struct HitRecord
{
    public Point3 P;
    public Vec3 Normal;
    public Material Mat;
    public double T;
    public bool FrontFace;

    public void SetFaceNormal(Ray r, Vec3 outwardNormal)
    {
        FrontFace = Vec3.Dot(r.Direction, outwardNormal) < 0;
        if(FrontFace)
        {
            Normal = outwardNormal;
        } else
        {
            Normal = -outwardNormal;
        }
    }
};

public abstract class Hittable
{
    public abstract bool Hit(Ray r, Interval rayT, ref HitRecord rec);
    public abstract AABB BoundingBox();
};