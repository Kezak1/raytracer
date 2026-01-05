using System.Text;

namespace raytracer;

public struct Vec3
{
    public double X, Y, Z;
    
    public Vec3()
    {
        X = Y = Z = 0;
    }
    public Vec3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public readonly double LenghtSquared()
    {
        return X*X + Y*Y + Z*Z;
    }

    public readonly double Lenght()
    {
        return Math.Sqrt(LenghtSquared());
    }
    
    public static Vec3 Random()
    {
        return new Vec3(Extensions.RandomDouble(), Extensions.RandomDouble(), Extensions.RandomDouble());
    }

    public static Vec3 Random(double min, double max)
    {
        return new Vec3(Extensions.RandomDouble(min, max), Extensions.RandomDouble(min, max), Extensions.RandomDouble(min, max));
    }

    public static Vec3 operator +(Vec3 u, Vec3 v)
    {
        return new Vec3(u.X + v.X, u.Y + v.Y, u.Z + v.Z);
    }

    public static Vec3 operator -(Vec3 u, Vec3 v)
    {
        return new Vec3(u.X - v.X, u.Y - v.Y, u.Z - v.Z);
    }

    public static Vec3 operator -(Vec3 v)
    {
        return new Vec3(-v.X, -v.Y, -v.Z);
    }

    public static Vec3 operator *(Vec3 u, Vec3 v)
    {
        return new Vec3(u.X * v.X, u.Y * v.Y, u.Z * v.Z);
    } 

    public static Vec3 operator *(double c, Vec3 v)
    {
        return new Vec3(c * v.X, c * v.Y, c * v.Z);
    }

    public static Vec3 operator *(Vec3 v, double c)
    {
        return c * v;
    }

    public static Vec3 operator /(Vec3 v, double t)
    {
        return 1/t * v;
    }

    public static double Dot(Vec3 u, Vec3 v)
    {
        return u.X * v.X + u.Y * v.Y + u.Z * v.Z;
    }

    public static Vec3 Cross(Vec3 u, Vec3 v)
    {
        return new Vec3(u.Y * v.Z - u.Z * v.Y, u.Z * v.X - u.X * v.Y, u.X * v.Y - u.Y * v.X);
    }

    public static Vec3 UnitVector(Vec3 v)
    {
        return v / v.Lenght();
    }

    public static Vec3 RandomUnitVector()
    {
        while(true)
        {
            var p = Random(-1, 1);
            var lensq = p.LenghtSquared();
            if(1e-160 < lensq && lensq <= 1)
            {
                return p / Math.Sqrt(lensq);
            }
        }   
    }

    public static Vec3 RandomOnHemisphere(Vec3 normal)
    {
        Vec3 onUnitSphere = RandomUnitVector();
        if(Dot(onUnitSphere, normal) > 0.0)
        {
            return onUnitSphere;
        } 
        else
        {
            return -onUnitSphere;
        }
    }

    public override string ToString()
    {
        return $"{X} {Y} {Z}";
    }
}