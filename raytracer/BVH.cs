namespace raytracer;

public class BVHNode : Hittable
{
    private Hittable Left;
    private Hittable Right;
    private AABB Bbox;

    private static int BoxCmp(Hittable a, Hittable b, int axis)
    {
        Interval aInterval = a.BoundingBox().AxisInterval(axis);
        Interval bInterval = b.BoundingBox().AxisInterval(axis);

        return aInterval.Min.CompareTo(bInterval.Min);
    }

    public BVHNode(List<Hittable> objects, int start, int end)
    {
        Bbox = new();
        for(int i = start; i < end; i++)
        {
            Bbox = new AABB(Bbox, objects[i].BoundingBox());
        }
        int axis = Bbox.LongestAxis();
        int objectSpan = end - start;

        if(objectSpan == 1)
        {
            Left = Right = objects[start]; 
        } 
        else if(objectSpan == 2)
        {
            Left = objects[start];
            Right = objects[start + 1];   
        }
        else
        {
            objects.Sort(start, objectSpan, Comparer<Hittable>.Create((a, b) => BoxCmp(a, b, axis)));

            int mid = start + objectSpan / 2;
            Left = new BVHNode(objects, start, mid);
            Right = new BVHNode(objects, mid, end);
        }

        Bbox = new AABB(Left.BoundingBox(), Right.BoundingBox());
    }
    
    public override bool Hit(Ray r, Interval rayT, ref HitRecord rec)
    {
        if(!Bbox.Hit(r, rayT))
        {
            return false;
        }

        bool hitLeft = Left.Hit(r, rayT, ref rec); 
        bool hitRight = Right.Hit(r, new Interval(rayT.Min, hitLeft ? rec.T : rayT.Max), ref rec);
        
        return hitLeft || hitRight;
    }

    public override AABB BoundingBox()
    {
        return Bbox;
    }
}