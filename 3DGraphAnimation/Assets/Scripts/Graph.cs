using UnityEngine;
using static UnityEngine.Mathf;

public class Graph : MonoBehaviour {

    [SerializeField]
    Transform cloudPrefab = default;
    int resolution = 150;
    [SerializeField]
   FunctionLibrary.FunctionName function = default;
   Transform [] points;
    void Awake(){
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        var position = Vector3.zero;
        points = new Transform[resolution * resolution];
        for (int i = 0; i < points.Length; i++){
            Transform point = Instantiate(cloudPrefab);
            point.localScale = scale; 
            // finds whatever transform is attached to the script
            point.SetParent(transform, false);
            // grab point to store in array for changing its position
            points[i] = point;
        }

    }

    void Update() {
        // delegate variable that grabs reference to function
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        FunctionLibrary.Function m = FunctionLibrary.GetFunction(FunctionLibrary.FunctionName.Sphere);
        // change function animation
        resolution = 225;
        float time = Time.time; 
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++){
            // create path with no cubes
            if (x == (resolution * 1/3)){
                // skip a distance
               x = (int)(resolution * 1.5/3);
            }
            if (x == resolution){
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            // grab vector to change the position
            Vector3 positionTorus = points[i].localPosition;
            // grab updated xz coordinates
            float u = (x + 0.5f) * step - 1f;
            // invoke the method call using the delegate variable
            positionTorus = f(u, v, time);
            // set new position of point based on above function
           points[i].localPosition = positionTorus;

        }
        // grab current rotation around y-axis
        Vector3 newRotation = this.transform.eulerAngles;
        newRotation.y = 360 * Cos(PI * 0.1f *time);
        this.transform.eulerAngles = newRotation;
    }
}