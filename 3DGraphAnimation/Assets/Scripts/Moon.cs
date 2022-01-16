using UnityEngine;


public class Moon : MonoBehaviour {

    [SerializeField]
    Transform pointPrefab = default;
    int moonRes = 85;
    [SerializeField]
    FunctionLibrary.FunctionName function = default;
    Transform [] moon;
    void Awake(){
        float step = 2f / moonRes;
        var scale = Vector3.one * step;
        var position = Vector3.zero;
        moon = new Transform[moonRes * moonRes];
        for (int i = 0; i < moon.Length; i++){
            Transform pointMoon = Instantiate(pointPrefab);
            pointMoon.localScale = scale;
            pointMoon.SetParent(transform, false);
            moon[i] = pointMoon;
        }
    }

    public void setMoonRes (int resolution){
        moonRes = resolution;
    }

    void Update() {
        // delegate variable that grabs reference to function
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        FunctionLibrary.Function m = FunctionLibrary.GetFunction(FunctionLibrary.FunctionName.Sphere);
        // change function animation
        float time = Time.time; 
        float stepMoon = 2f / moonRes;
        float v = 0.5f * stepMoon - 1f;
        for (int i = 0, x = 0, z = 0; i < moon.Length; i++, x++){
            // create path with no cubes
            if (x == (moonRes * 1/3)){
                // skip a distance
               x = moonRes * 2/3;
            }
            if (x == moonRes){
                x = 0;
                z += 1;
                v = (z + 0.5f) * stepMoon - 1f;
            }
        
            // grab vector to change the position
            Vector3 positionMoon = moon[i].localPosition;
            // grab updated xz coordinates
            float u = (x + 0.5f) * stepMoon - 1f;
            // invoke the method call using the delegate variable
            positionMoon = m(u, v, time);
            // set new position of point based on above function
           moon[i].localPosition = positionMoon;
        }
    }
}