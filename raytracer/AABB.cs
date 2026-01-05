namespace raytracer;

public class AABB
{
    public Interval X, Y, Z;

    public AABB()
    {
        X = new();
        Y = new();
        Z = new();
    }
    public AABB(Interval x, Interval y, Interval z) {
        X = x;
        Y = y;
        Z = z;
    }

    public AABB(Point3 a, Point3 b)
    {
        X = (a.X <= b.X) ? new Interval(a.X, b.X) : new Interval(b.X, a.X);
        Y = (a.Y <= b.Y) ? new Interval(a.Y, b.Y) : new Interval(b.Y, a.Y);
        Z = (a.Z <= b.Z) ? new Interval(a.Z, b.Z) : new Interval(b.Z, a.Z);
    }

    public AABB(AABB box0, AABB box1)
    {
        X = new Interval(box0.X, box1.X);
        Y = new Interval(box0.Y, box1.Y);
        Z = new Interval(box0.Z, box1.Z);
    }

    public Interval AxisInterval(int n)
    {
        if(n == 1) return Y;
        if(n == 2) return Z;
        return X;
    }

    public bool Hit(Ray r, Interval rayT)
    {
        Point3 rayOrig = r.Origin;
        Vec3 rayDir = r.Direction;
        
        for(int axis = 0; axis < 3; axis++)
        {
            Interval ax = AxisInterval(axis);
            double adinv = 1.0 / rayDir[axis];

            var t0 = (ax.Min - rayOrig[axis]) * adinv;
            var t1 = (ax.Max - rayOrig[axis]) * adinv;

            if(t0 < t1)
            {
                if(t0 > rayT.Min) rayT.Min = t0;
                if(t1 < rayT.Max) rayT.Max = t1;
            } 
            else
            {
                if(t1 > rayT.Min) rayT.Min = t1;
                if(t0 < rayT.Max) rayT.Max = t0;
            }

            if(rayT.Max <= rayT.Min)
            {
                return false;
            }
        }

        return true;   
    }

    public int LongestAxis()
    {
        if(X.Size() > Y.Size())
        {
            return X.Size() > Z.Size() ? 0 : 2;
        } 
        else
        {
            return Y.Size() > Z.Size() ? 1 : 2;
        }
    }
}