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

    public double this[int idx]
    {
        get
        {
            switch (idx)
            {
                case 0: 
                    return X;
                case 1: 
                    return Y;
                case 2: 
                    return Z;
                default: 
                    throw new IndexOutOfRangeException();
            }
        }

        set
        {
            switch (idx)
            {
                case 0: 
                    X = value; 
                    break;
                case 1: 
                    Y = value; 
                    break;
                case 2: 
                    Z = value; 
                    break;
                default: 
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public readonly double LengthSquared()
    {
        return X*X + Y*Y + Z*Z;
    }

    public readonly double Length()
    {
        return Math.Sqrt(LengthSquared());
    }

    public readonly bool NearZero()
    {
        var s = 1e-8;
        return (double.Abs(X) < s) && (double.Abs(Y) < s) && (double.Abs(Z) < s);
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
        return new Vec3(u.Y * v.Z - u.Z * v.Y, u.Z * v.X - u.X * v.Z, u.X * v.Y - u.Y * v.X);
    }

    public static Vec3 UnitVector(Vec3 v)
    {
        return v / v.Length();
    }

    public static Vec3 RandomInUnitDisk()
    {
        while(true)
        {
            var p = new Vec3(Extensions.RandomDouble(-1, 1), Extensions.RandomDouble(-1, 1), 0);
            if(p.LengthSquared() < 1)
            {
                return p;
            }
        }
    }

    public static Vec3 RandomUnitVector()
    {
        while(true)
        {
            var p = Random(-1, 1);
            var lensq = p.LengthSquared();
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

    public static Vec3 Reflect(Vec3 v, Vec3 n)
    {
        return v - 2 * Dot(v, n) * n;
    }

    public static Vec3 Refract(Vec3 uv, Vec3 n, double etaiOverEtat)
    {
        var cosTheta = double.Min(Dot(-uv, n), 1.0);
        Vec3 rOutPerp = etaiOverEtat * (uv + cosTheta * n);
        Vec3 rOutParallel = -Math.Sqrt(double.Abs(1.0 - rOutPerp.LengthSquared())) * n;

        return rOutPerp + rOutParallel;
    }

    public readonly override string ToString()
    {
        return $"{X} {Y} {Z}";
    }
}