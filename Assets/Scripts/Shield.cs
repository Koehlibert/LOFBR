using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public MainPlayerBehaviour player;
    public void SetPlayer(MainPlayerBehaviour player)
    {
        this.player = player;
    }
    void Update()
    {
        this.transform.position = player.transform.position + new Vector3(0f,2f,0f);
        this.transform.rotation = player.transform.rotation;
    }
}
