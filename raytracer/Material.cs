using System.Reflection.Emit;

namespace raytracer;

public abstract class Material
{
    public virtual bool Scatter(Ray rIn, HitRecord rec, out Color attenuation, out Ray scattered)
    {
        attenuation = default;
        scattered = default;
        return false;
    } 
}

public class Lambertian : Material {
    private Color Albedo;
    
    public Lambertian(Color albedo)
    {
        Albedo = albedo;
    }

    public override bool Scatter(Ray rIn, HitRecord rec, out Color attenuation, out Ray scattered)
    {
        var scatterDirection = rec.Normal + Vec3.RandomUnitVector();

        if(scatterDirection.NearZero())
        {
            scatterDirection = rec.Normal;
        }
        scattered = new Ray(rec.P, scatterDirection);
        attenuation = Albedo;
        
        return true;
    }
}

public class Metal : Material
{
    private Color Albedo;
    private double Fuzz;
    public Metal(Color albedo, double fuzz)
    {
        Albedo = albedo;
        if(fuzz < 1)
        {
            Fuzz = fuzz;
        } 
        else
        {
            Fuzz = 1;
        }
    }

    public override bool Scatter(Ray rIn, HitRecord rec, out Color attenuation, out Ray scattered)
    {
        Vec3 reflected = Vec3.Reflect(rIn.Direction, rec.Normal);
        reflected = Vec3.UnitVector(reflected) + (Fuzz * Vec3.RandomUnitVector());
        scattered = new Ray(rec.P, reflected);
        attenuation = Albedo;

        return Vec3.Dot(scattered.Direction, rec.Normal) > 0;
    }
}