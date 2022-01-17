using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{

    public Material _skybox;
    public Material _default;
    
    [SerializeField]
    float _playerHeight = 0.18f;
    public GameObject _player;
    public Transform _destination;


    private bool _overlapping = false;

    // Update is called once per frame
    void Update()
    {

        if (_overlapping)
        {
            Debug.Log("old pos: " + _player.transform.position.y);
            // Determine distance between player and portal
            Vector3 _portalToPlayer = _player.transform.position - transform.position;
            // float _dotProduct = Vector3.Dot(transform.forward.normalized, _portalToPlayer.normalized);
            //Debug.Log("dot: " + _dotProduct);
            // disable character controller briefly
            
           
            _player.GetComponent<CharacterController>().enabled = false;
            // get rotational difference
            float _rotationDiff = -Quaternion.Angle(transform.rotation, _destination.rotation);
            
            // Micro adjustmemts to camera based on world
                
            if (gameObject.name == "World 1 collider")
                {
                   // flip
                   // Debug.Log("rotation diff after: " + _rotationDiff);
                    _rotationDiff += 180f;
                   // Change skybox
                   RenderSettings.skybox = _skybox;
                  
            } 
            else if (gameObject.name  == "World 2 collider")
                {
                    // flip
                    _rotationDiff -= 180f;
                    // Change skybox
                    RenderSettings.skybox = _skybox;
            }
            else if (gameObject.name == "World 3 collider")
                {
                    // flip
                    _rotationDiff -= 180f;
                    // Change skybox
                    RenderSettings.skybox = _skybox;
            } 
            else if (gameObject.name == "World 4 collider")
                {
                    // flip
                    _rotationDiff += 0f;
                    // Change skybox
                    RenderSettings.skybox = _skybox;
            }
            else if (_destination.name == "Camera 1 Tesseract")
            {
                // flip
                _rotationDiff += 180f;
                RenderSettings.skybox = _default;
            }
            else if (_destination.name == "Camera 2 Tesseract")
            {
                // flip
                _rotationDiff -= 180f;
                RenderSettings.skybox = _default;
            }
            else if (_destination.name == "Camera 3 Tesseract")
            {
                // flip
                _rotationDiff -= 180f;
                RenderSettings.skybox = _default;
            }
            else 
            {
                // flip
                _rotationDiff -= 0f;
                Debug.Log("WORLD 4 " + _destination.name);
                RenderSettings.skybox = _default;
            }


            // rotate player
            _player.transform.Rotate(Vector3.up, _rotationDiff); // works
                
                // adjust position
                // why multiply by a rotation by a vector? magnitude and direction?
                Vector3 _positionOffset = Quaternion.Euler(0f, _rotationDiff, 0f) * _portalToPlayer;
                //_player.transform.position = _destination.position + _positionOffset;
                 _player.transform.position =  _destination.position;
                 
                // Debug.Log("destination pos: " + _destination.position);
                // Micro adjustmemts to height based on world
                //_player.transform.position = new Vector3(_player.transform.position.x, _destination.transform.position.y, _player.transform.position.z);
                
                _player.GetComponent<CharacterController>().enabled = true;
                Debug.Log("new pos: " + _player.transform.position.y);

                _overlapping = false;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 _portalToPlayer = transform.position - _player.transform.position;
            if (Vector3.Dot(transform.forward.normalized, _portalToPlayer.normalized) > 0)
            {
                Debug.Log("dot: " + Vector3.Dot(transform.forward.normalized, _portalToPlayer.normalized));
                print("Back");
                _overlapping = false;
            }
            else if (Vector3.Dot(transform.forward.normalized, _portalToPlayer.normalized) < 0.5f)
            {
                Debug.Log("dot: " + Vector3.Dot(transform.forward.normalized, _portalToPlayer.normalized));
                print("Front");
                _overlapping = true;
            }
            else if (Vector3.Dot(transform.forward.normalized, _portalToPlayer.normalized) == 0)
            {
                Debug.Log("dot: " + Vector3.Dot(transform.forward.normalized, _portalToPlayer.normalized));
                print("Side");
                _overlapping = false;
            }
            Debug.Log("collided");
      
        }
    }

   private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("exit");
            _overlapping = false;
        }
    }
   
}
