using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    private PlayerController player;
    private Vector3 offset = new Vector3(0, 1.5f,0);
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    void OnDestroy()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + player.transform.forward*1.25f + offset;
    }
}
