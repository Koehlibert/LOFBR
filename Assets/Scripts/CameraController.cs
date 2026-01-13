using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private float smooth = 0.15f;
    private Vector3 velocity = Vector3.zero;
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        offset = transform.position - player.transform.position;
    }
    void LateUpdate()
    {
        if (!player)
        {
            player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        }
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, smooth);
    }
}
