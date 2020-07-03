using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEnemyManager : Manager
{
    public List<Enemy> enemies;
    public List<ZoneDoor> doors;
    public List<DoorTrigger> triggers;
    public List<Spawner> spawners;

    public List<GameObject> Types;

    int AreaTotalEnemies_;

    public int[] Waves;
    int currentWave_;
    int currentEnemyCount_;
    public bool cameraOpen;
    public bool cameraClose;

    public delegate void SubscriptionHandler(LocalEnemyManager manager, string location);
    public static event SubscriptionHandler SubcribeSlaves;



    void Awake()
    {
        doors = new List<ZoneDoor>();
        triggers = new List<DoorTrigger>();
        enemies = new List<Enemy>();
        spawners = new List<Spawner>();

        AreaTotalEnemies_ = 0;
        currentWave_ = 0;


        for (int i = 0; i < Waves.Length; i++)
        {
            AreaTotalEnemies_ += Waves[i];
        }

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        SubcribeSlaves(this, location);


    }

    // Update is called once per frame
    void Update()
    {

       

    }

    public void AddEnemy(Enemy enemy) //add an enemy to List
    {        
        enemies.Add(enemy);
        if (currentWave_ == 0)
        {
            AreaTotalEnemies_++;
            currentEnemyCount_++;
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
       enemies.Remove(enemy);
       AreaTotalEnemies_--;
       currentEnemyCount_--;

        if (currentEnemyCount_ == 0 && currentWave_ < Waves.Length) SpawnNextWave();
        if (AreaTotalEnemies_ == 0) deactivateDoors(cameraClose);

    }

    public void AddDoor(ZoneDoor door) //add a Door to List
    {
      //  Debug.Log("door added");

         doors.Add(door);
    }

    public void RemoveDoor(ZoneDoor door)
    {
        
        doors.Remove(door);

    }

    public void AddTrigger(DoorTrigger trigger) //add a Trigger to List
    {
      //  Debug.Log("trigger added");
        triggers.Add(trigger);
    }

    public void RemoveTrigger(DoorTrigger trigger)
    {

       triggers.Remove(trigger);

    }

    public void activateDoors(bool cameras) //Active Doors and destroy triggers
    {
        if (cameras)
        {
         
            activateCamerasForOpening();
            setEnemySleep(true);
        }
        else
        {
            foreach (DoorTrigger t in triggers) t.DestroyTrigger();

            foreach (ZoneDoor d in doors) d.gameObject.SetActive(true);
            checkCurrentWave();
        }        
        
    }

    void activateCamerasForClosing()
    {
        BattleCameraManager manager = GetComponent<BattleCameraManager>();
        manager.InitiateScene(location, 2);
    }

    void activateCamerasForOpening()
    {
        BattleCameraManager manager = GetComponent<BattleCameraManager>();
        manager.InitiateScene(location, 1);
    }

    public void deactivateDoors(bool cameras) //Deactivate doors;
    {
        if (cameras)
        {
            activateCamerasForClosing();
            setEnemySleep(true);
        }
        else
        {
            foreach (ZoneDoor d in doors) d.gameObject.SetActive(false);
        }
    }



    public void enrageEnemies() //Set Enenemies to attack Player
    {
        foreach (Enemy e in enemies) e.enrage();
    }

    public void setEnemySleep(bool sleep) //sets the enemies sleeping or not
    {
        foreach (Enemy e in enemies) e.setSleep(sleep);
    }

    public void addSpawn(Spawner s) //Add a spawner to the List
    {
        spawners.Add(s);
    }

    public void SpawnNextWave() //Spawn a wave
    {
        currentEnemyCount_ = Waves[currentWave_];
        currentWave_++;


        for (int i = 0; i < Waves[currentWave_-1]; i++)
        {
            SpawnOne();
        }        
    }

    void SpawnOne() //Spawn 1 Enemy randomly in a spawner
    {        
        int r = Random.Range(0, spawners.Count);

        if (spawners[r].CanSpawn())
        {
            GameObject o = SelectEnemy();
            spawners[r].spawn(o, o.name);
            return;
        }
        else
        {
            SpawnOne();
        }
    }

    private GameObject SelectEnemy() //Select which Enemy must be Spawned
    {
        return Types[0];

    }


    private void checkCurrentWave()
    {
        if (currentEnemyCount_ == 0) SpawnNextWave();
    }
}
