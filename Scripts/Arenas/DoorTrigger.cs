using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : Subscriber
{
    LocalEnemyManager manager;
    bool used = false;

    void Awake()
    {
        LocalEnemyManager.SubcribeSlaves += SubscribeToManager;
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

    }

    void SubscribeToManager(LocalEnemyManager manager, string location) //New Version
    {
        if (this.location == location)
        {
            this.manager = manager;
            manager.AddTrigger(this);
        }
    }

    void OnTriggerEnter(Collider c)
    {  
        if (c.gameObject.CompareTag("Player"))
        {
            if (!used)
            {
                manager.activateDoors(manager.cameraOpen);
                used = true;
            }
        }
    }

    public void DestroyTrigger()
    {     
        Destroy(gameObject);
    }

}
