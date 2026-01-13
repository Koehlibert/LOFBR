using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public IMainPlayer player;
    public void SetPlayer(IMainPlayer player)
    {
        this.player = player;
    }
    void Update()
    {
        this.transform.position = player.GetTransform().position + new Vector3(0f,2f,0f);
        this.transform.rotation = player.GetTransform().rotation;
    }
    /*void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletEnemy")||other.gameObject.CompareTag("BulletEnemyPlayer"))
        {
            Destroy(other.gameObject);
        }
    }*/
}
