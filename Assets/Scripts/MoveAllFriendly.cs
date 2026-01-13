using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAllFriendly : MonoBehaviour
{
    private string[] moveableThings = new string[4]{"Player","EnemyPlayer","Friendly","Enemy"};
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        foreach (string tag in moveableThings)
        {
            if ((other.tag == tag)&&(!other.gameObject.GetComponent<FriendlyTowerHit>()))
            {
                other.gameObject.transform.Translate(new Vector3(0,0,1)*10,Space.World);
            }
        }
    }
}
