using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Subscriber
{
    bool canSpawn;
    LocalEnemyManager manager;
    public float coolDownTime;
    float coolDowntimer;
    bool inCoolDown;
    int entityCount;
    



    void Awake()
    {
        LocalEnemyManager.SubcribeSlaves += SubscribeToManager;
        canSpawn = true;
        inCoolDown = false;
        coolDowntimer = Mathf.Infinity;
        entityCount = 0;       
    }

    void OnDestroy()
    {
        LocalEnemyManager.SubcribeSlaves -= SubscribeToManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        //manager = SubcribeToManager();

        
    }

    // Update is called once per frame
    void Update()
    {

        if (inCoolDown && coolDowntimer < coolDownTime) coolDowntimer += Time.deltaTime;
        else if (inCoolDown && coolDowntimer > coolDownTime)
        {
            inCoolDown = false;
            canSpawn = true;
        }


    }

    void SubscribeToManager(LocalEnemyManager manager, string location) //New Version
    {
        if (this.location == location)
        {
            this.manager = manager;
            manager.addSpawn(this);
        }
    }


    public bool CanSpawn()
    {
        return canSpawn;
    }

    public void spawn(GameObject o, string enemyType)
    {
        Vector3 pos = transform.position;
        GameObject a = Instantiate(o, pos, Quaternion.identity);        
        switch (enemyType)
        {
            case "MagicTree":               
                MagicTree mt = a.GetComponent<MagicTree>();
                mt.enrage();
                mt.manager = manager;
                manager.AddEnemy(mt);
                break;
        }

        canSpawn = false;
        coolDowntimer = 0;
        inCoolDown = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            canSpawn = false;
            entityCount++;
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            canSpawn = false;
        }
        

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            entityCount--;
        }

        if (entityCount == 0 && !inCoolDown) canSpawn = true;
    }

}
