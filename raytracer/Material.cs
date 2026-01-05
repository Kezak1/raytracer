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

public class Dielectric : Material {
    private readonly double RefractionIndex;

    public Dielectric(double index)
    {
        RefractionIndex = index;   
    }

    public override bool Scatter(Ray rIn, HitRecord rec, out Color attenuation, out Ray scattered)
    {
        attenuation = new Color(1.0, 1.0, 1.0);
        double ri = rec.FrontFace ? (1.0 / RefractionIndex) : RefractionIndex;
        
        Vec3 unitDirection = Vec3.UnitVector(rIn.Direction);
        double cosTheta = double.Min(Vec3.Dot(-unitDirection, rec.Normal), 1.0);
        double sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);

        bool cannotRefract = ri * sinTheta > 1.0;
        Vec3 direction;
        
        if(cannotRefract || Reflectance(cosTheta, ri) > Extensions.RandomDouble())
        {
            direction = Vec3.Reflect(unitDirection, rec.Normal);
        } 
        else
        {
            direction = Vec3.Reflect(unitDirection, rec.Normal, ri);
        }
        scattered = new Ray(rec.P, direction);
        return true;
    }

    private static double Reflectance(double cosine, double reflectedIndex)
    {
        var r0 = (1 - reflectedIndex) / (1 + reflectedIndex);
        r0 *= r0;
        return r0 + (1-r0) * Math.Pow(1 - cosine, 5);
    }
}