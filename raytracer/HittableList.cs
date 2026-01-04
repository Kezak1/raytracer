using System.Security.Cryptography.X509Certificates;

namespace raytracer;

public class HittableList : Hittable
{
    public List<Hittable> Objects;
}