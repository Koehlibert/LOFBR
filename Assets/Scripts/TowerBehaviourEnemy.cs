using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviourEnemy : MonoBehaviour
{
    public float range;
    public GameObject player;
    public string enemytype;
    public GameObject currentenemy;
    public float cooldown;
    public GameObject bullet;
    private Vector3 offset;
    [SerializeField] private Animator animator;
    private bool loaded;
    private float reloadtime;
    private GameObject bulletinstance;
    // Start is called before the first frame update
    void Start()
    {
        enemytype = "Friendly";
        player = GameObject.FindWithTag("Player");
        offset = new Vector3(0,9,0);
        range = 35;
        loaded = true;
        reloadtime = 1.25f;
    }
    void Update()
    {
        currentenemy = findClosestEnemy();
        if (inRange((currentenemy))&&(loaded))
        {
            animator.Play("Throw",0,0f);
            attack(currentenemy);
        }
    }
    public bool inRange(GameObject enemy)
    {
        bool isInRange = false;
        if ((enemy != null)&&(Vector3.Distance(enemy.transform.position,this.transform.position)<=range))
        {
            isInRange = true;
        }
        return isInRange;
    }
    public GameObject findClosestEnemy()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag(enemytype);
        GameObject closestEnemy = null;
        if (allEnemies.Length != 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (GameObject currenemy in allEnemies)
            {
                float distanceToEnemy = Vector3.Distance(currenemy.transform.position,this.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = currenemy;
                }
            }
            if ((player != null)&&(player.activeSelf)&&(Vector3.Distance(this.transform.position,player.transform.position) < 30))
            {
               closestEnemy = player;
            }
            return closestEnemy;
        }
        else
        {
            if ((player != null)&&(player.activeSelf))
            {
                closestEnemy = player;
            }
            return closestEnemy;
        }
    }
    void attack(GameObject target)
    {
        transform.LookAt(target.transform.position);
        bulletinstance = Instantiate(bullet, transform.position + offset +1.5f*(transform.position-target.transform.position).normalized, transform.rotation);
        bulletinstance.GetComponent<Damage>().SetProperties(60, 0, CombatUtils.Team.Enemy, true);
        Rigidbody bulletrig = bulletinstance.GetComponent<Rigidbody>();
        bulletrig.AddForce(gameObject.transform.forward*250000f*Time.deltaTime);
        StartCoroutine("reload");
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
}
