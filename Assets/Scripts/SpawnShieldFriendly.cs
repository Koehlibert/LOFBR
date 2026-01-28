using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShieldFriendly : MonoBehaviour
{
    public PlayerController player;
    public CapsuleCollider shield;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        shield = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletEnemy")||other.gameObject.CompareTag("BulletEnemyPlayer"))
        {
            Destroy(other.gameObject);
        }
    }
}
