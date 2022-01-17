using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCam : MonoBehaviour
{
    public Transform _playercam;
    public Transform _thisportal;
    // we need the position of the other portal to determine the player's
    // position relative to that portal (i.e. portal A)
    // this determines how far this camera should be from portal B
    public Transform _otherportal;
    private float _camoffset = 0f;
    private int _signpos = 1;
    [SerializeField]
    private float _tempOffsetx, _tempOffsety, _tempOffsetz = 0f;
    bool _adjustPos = false;
    // Start is called before the first frame update
    void Start()
    {
        // Add offset to camera rotation based on the world view 
        if (this.name == "Camera 1")
        {
            _camoffset = 180f;
        }
        else if (this.name == "Camera 2")
        {
            _camoffset += 0f;
            _signpos = -1;
        }
        else if (this.name == "Camera 3")
        {
            _camoffset = -90f;
            _adjustPos = true;
        }
        else if (this.name == "Camera 4")
        {
            _camoffset = 90f;
            _signpos = -1;
            _adjustPos = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // getting the player offsets from his portal and using it to adjust the position of camera B
        Vector3 _playeroffset = new Vector3((_playercam.position - _otherportal.position).x + _tempOffsetz, 
                                            (_playercam.position - _otherportal.position).y + _tempOffsety, 
                                            (_playercam.position - _otherportal.position).z + _tempOffsetx);
        Vector3 _newPos = _thisportal.position + _signpos * _playeroffset;
        if (_adjustPos)
        {
            this.transform.position = new Vector3(_newPos.z, _newPos.y, _newPos.x);
        }
        else
        {
            this.transform.position = _thisportal.position + _signpos * _playeroffset;
        }

        // Get angular difference between portal rotations
        float _angularDifferenceBetweenPortalRotations = Quaternion.Angle(_playercam.rotation, _otherportal.rotation);
        // Debug.Log("angle between portal: " + _angularDifferenceBetweenPortalRotations);
        // .. 
        // Use angular difference to create a rotation
        Quaternion _portalRotationalDifference = Quaternion.AngleAxis(_angularDifferenceBetweenPortalRotations + _camoffset, Vector3.up);
       // Direction to point the rotation
        Vector3 _newCameraDirection = _portalRotationalDifference * _playercam.forward;
        transform.rotation = Quaternion.LookRotation(_newCameraDirection, Vector3.up);

    }
}
