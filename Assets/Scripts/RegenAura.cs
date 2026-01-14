using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenAura : MonoBehaviour
{
    private CapsuleCollider aura;
    private List<GameObject> objectList;
    private PlayerController player;
    private float buff;
    void Start()
    {
        Destroy(this.gameObject,8f);
        aura = GetComponent<CapsuleCollider>();
        objectList = new List<GameObject>();
        player = FindObjectOfType<PlayerController>();
        buff = player.levelsys.getLevel()*3 + 10;
        player.GetHealth().ActivateSuperRegen(buff);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isActiveAndEnabled)
        {
            transform.position = player.transform.position;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Friendly")&&(!other.GetComponent<TowerBehaviourFriendly>()))
        {
            objectList.Add(other.gameObject);
            other.gameObject.GetComponent<Health>().ActivateSuperRegen(buff);
        }
    }
    void OnTriggerExit(Collider other)
    {
       if ((objectList.Contains(other.gameObject))&&(!other.GetComponent<TowerBehaviourFriendly>()))
       {
            other.gameObject.GetComponent<Health>().DeactivateSuperRegen();
            objectList.Remove(other.gameObject);
       } 
    }
    void OnDestroy()
    {
        objectList.RemoveAll(item => item == null);
        foreach (GameObject minion in objectList)
        {
            minion.gameObject.GetComponent<Health>().DeactivateSuperRegen();
        }
        player.GetHealth().DeactivateSuperRegen();
    }
}
