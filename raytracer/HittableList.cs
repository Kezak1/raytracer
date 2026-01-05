namespace raytracer;

public class HittableList : Hittable
{
    private AABB Bbox;
    public List<Hittable> Objects;
    public HittableList()
    {
        Objects = [];
        Bbox = new();
    }
    
    public HittableList(Hittable obj)
    {
        Objects = [];
        Add(obj);
        Bbox = new();
    }
    public void Clear()
    {
        Objects.Clear();
    }

    public void Add(Hittable obj)
    {
        Objects.Add(obj);
        Bbox = new AABB(Bbox, obj.BoundingBox());
    }
    
    public override bool Hit(Ray r, Interval rayT, ref HitRecord rec)
    {
        HitRecord tmpRec = new();
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

    public override AABB BoundingBox()
    {
        return Bbox;
    }
}