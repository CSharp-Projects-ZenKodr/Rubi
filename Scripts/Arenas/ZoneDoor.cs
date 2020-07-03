using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDoor : Subscriber
{
    LocalEnemyManager manager;

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
  
      

        deactivate();
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
            manager.AddDoor(this);
        }
    }

    public void activate()
    {
        gameObject.SetActive(true);
    }

    public void deactivate()
    {
        gameObject.SetActive(false);
    }

}
