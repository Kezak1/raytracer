namespace raytracer;

public class Sphere : Hittable
{
    private Point3 Center;
    private double Radius;
    private Material Mat;
    private AABB Bbox;

    public Sphere(Point3 center, double radius, Material mat)
    {
        Center = center;
        Radius = double.Max(0.0, radius);
        Mat = mat;

        var rvec = new Vec3(radius, radius, radius);
        Bbox = new AABB(center - rvec, center + rvec);
    }

    public override bool Hit(Ray r, Interval rayT, ref HitRecord rec)
    {
        Vec3 oc = Center - r.Origin;
        var a = r.Direction.LengthSquared();
        var h = Vec3.Dot(r.Direction, oc);
        var c = oc.LengthSquared() - Radius * Radius;

        var delta = h*h - a*c;
        if(delta < 0)
        {
            return false;
        }

        var sqrtD = Math.Sqrt(delta);
        var root = (h - sqrtD) / a;
        if(root <= rayT.Min || rayT.Max <= root)
        {
            root = (h + sqrtD) / a;
            if(root <= rayT.Min|| rayT.Max <= root)
            {
                return false;
            }
        }

        rec.T = root;
        rec.P = r.At(rec.T);
        Vec3 outwardNormal = (rec.P - Center) / Radius;
        rec.SetFaceNormal(r, outwardNormal);
        rec.Mat = Mat;

        return true;
    }

    public override AABB BoundingBox()
    {
        return Bbox;
    }
}
