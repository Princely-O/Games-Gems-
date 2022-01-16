
using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary {

    // declaration of delegate function
    // delegate variables/methods must have these 3 parameters 
    // the Vector3 refers to the type the referenced function must return
    public delegate Vector3 Function (float u, float v, float time);
    public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus}
    public static Function[] functions = {Wave, MultiWave, Ripple, Sphere, Torus};

    public static Function GetFunction(FunctionName name){
        // grab the integer value of the enum
        return functions[(int)name];
    }
    
    public static Vector3 Wave (float u, float v, float t){
        Vector3 f;
        f.x = u;
        f.z = v;
        f.y = Sin(PI * (u + v + t));
        return f;
    }

    
    public static Vector3 MultiWave (float u, float v, float t){
        Vector3 f;
        f.x = u;
        f.z = v;
        f.y = Sin(PI * (u + 0.5f * t));
        f.y += 0.5f * Sin(2f * PI * (v + t));
		f.y += Sin(PI * (u + v + 0.25f * t));
        f.y *= (1f / 2.5f);
		return f;

    }

    public static Vector3 Ripple (float u, float v, float t){
        Vector3 f;
        f.x = u;
        f.z = v;
        float d = Sqrt(u * u + v * v);
        f.y = Sin(PI * (4f * d + t));
        f /= (1f + 10f * d);
        return f;
    }

    public static Vector3 Sphere (float u, float v, float t){
        float r = 0.25f + 0.05f * Sin(PI * t);
        // horizontal bands    r = 0.9f + 0.1f * Sin(8f * PI * u);
        // verical bands = 0.9f + 0.1f * Sin(8f * PI * v);
		float s = r * Cos(0.5f * PI * v);
		Vector3 p;
		p.x = s * Sin(PI * u);
		p.y = r * Sin(0.5f * PI * v);
		p.z = s * Cos(PI * u);
		return p; 
    }

    public static Vector3 Torus (float u, float v, float t){
        Vector3 p;
  	    float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
		float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
		float s = r1 + r2 * Cos(PI * v);
        p.x = s * Sin(PI * u);
		p.y = r2 * Sin(PI * v);
		p.z = s * Cos(PI * u);
        return p;
    }


}