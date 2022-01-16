using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    // Start is called before the first frame update
    bool shoot = false;
    bool backward = false;
    AudioSource m_MyAudioSource = default;
    [SerializeField]
    Transform canvasPrefab = default;
    [SerializeField]
    GameObject instructionsPrefab = default;
    GameObject instructions = default;
    [SerializeField]
    Transform asteroidPrefab = default;
    [SerializeField]
    Transform electricityPrefab = default;
    Vector3 originalPostion = default;
    void Awake() {
        instructions = Instantiate(instructionsPrefab);
        Transform asteroid = Instantiate(asteroidPrefab);
        asteroid.SetParent(transform, false);
        originalPostion = this.transform.localPosition;
        m_MyAudioSource = GetComponent<AudioSource>();
    }
   private void OnTriggerEnter(Collider other)
    {
         
         if (other.gameObject.name == "Cloud(Clone)") {
            shoot = false;
            backward = true;
            //print("Cloud!");
         }
         if (other.gameObject.name == "Moon"){
           // electricity animation
            shoot = false;
            m_MyAudioSource.Play();
            Destroy(instructions);
           // print("Moonshot!");
            electricityPrefab = Instantiate(electricityPrefab);
            other.gameObject.GetComponent<Moon>().setMoonRes(10);
            Vector3 scale = this.transform.localScale;
            scale.x = 0.01f;
            scale.y = 0.01f;
            scale.z = 0.01f;
            this.transform.localScale = scale;
            Transform canvas = Instantiate(canvasPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // time since game started
        float time = Time.time;
        if (Input.GetKeyDown("space") || shoot)
        {
            // fire
            shoot = true;
            Vector3 forward = this.transform.localPosition;
            forward.z +=  Mathf.Sin(0.1f * 5f);
            this.transform.localPosition = forward;
        }

        if (backward && this.transform.localPosition.z > originalPostion.z){
            Vector3 forward = this.transform.localPosition;
            forward.z -=  Mathf.Sin(0.1f * 5f);
            this.transform.localPosition = forward;
        } else {
            backward = false;
        }
    }
}
