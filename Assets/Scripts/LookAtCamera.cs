using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 looktarget = new Vector3(cam.transform.position.x,cam.transform.position.y,transform.position.z);
        transform.LookAt(looktarget);
    }
}
