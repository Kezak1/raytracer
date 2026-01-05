namespace raytracer;

public class HittableList : Hittable
{
    public List<Hittable> Objects;
    public HittableList()
    {
        Objects = new List<Hittable>();
    }
    
    public HittableList(Hittable obj)
    {
        Objects = new List<Hittable>();
        Add(obj);
    }
    public void Clear()
    {
        Objects.Clear();
    }

    public void Add(Hittable obj)
    {
        Objects.Add(obj);
    }
    
    public override bool Hit(Ray r, Interval rayT, ref HitRecord rec)
    {
        HitRecord tmpRec = new HitRecord();
        bool hitAny = false;
        var closetsSoFar = rayT.Max;

        foreach(var obj in Objects) {
            if(obj.Hit(r, new Interval(rayT.Min, closetsSoFar), ref tmpRec))
            {
                hitAny = true;
                closetsSoFar = tmpRec.T;
                rec = tmpRec;
            }
        }

        return hitAny;
    }
}