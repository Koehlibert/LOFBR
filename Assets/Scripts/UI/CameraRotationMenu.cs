using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationMenu : MonoBehaviour
{
    private float alpha;
    void Start()
    {
        alpha = Mathf.PI*0.5f;
    }
    void Update()
    {
        alpha += Time.deltaTime*0.1f;
        transform.position = new Vector3(45*Mathf.Cos(alpha),40f,-140*Mathf.Sin(alpha));
        transform.LookAt(Vector3.zero);
    }
}
