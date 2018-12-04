using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target; // player
    public float pace = 5f; // a lil lag to make camera movement smoother..

    Vector3 offset; // distance between camera and player

    void Start()
    {
        offset = transform.position - target.position; // initial camera player distance
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset; // variable that maintain initial relative position from player every fixedUpdate
        // move camera smoothly with Lerp from its current position to the new position 
        transform.position = Vector3.MoveTowards(transform.position, targetCamPos, pace * Time.deltaTime);
    }
}
