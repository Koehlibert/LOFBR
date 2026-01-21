using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryColliderBehaviour : MonoBehaviour
{
    private PlayerController player;
    private Vector3 offset = new Vector3(0,3,0);
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        transform.position = player.transform.position + offset + player.transform.forward*2;
        transform.rotation = player.transform.rotation;
    }
    void OnTriggerEnter(Collider col)
    {
        if ((col.gameObject.CompareTag("BulletEnemy"))||(col.gameObject.CompareTag("BulletEnemyPlayer")))
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(player.transform.forward*2000);
            col.gameObject.tag = "BulletPlayer";
        }
    }
}
