using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    private PlayerController player;
    private float manaCost;
    private Damage dmg;
    private Vector3 offset = new Vector3(0,4,0);
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        manaCost = 2;
        dmg = gameObject.GetComponent<Damage>();
    }
    void Update()
    {
        player.manasys.useMana(manaCost*Time.deltaTime);
        transform.position = player.transform.position + offset;
    }
}
