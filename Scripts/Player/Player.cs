using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    public bool alive { get; private set; }

    Targetable target_;
    public bool lockedOn;

    public float maxLockOnDistance;
    bool casting_;
    SpellSelector selector_;
    Animator animator_;
    bool Inspect_;
    public bool inControl;
    public bool fallen;
    public GameObject cam;

    bool invincible_;

    public float damageRecoverTime;
    float damageRecoveryTimer;
    bool recovering_;

    int maxHealth_;

    //PersistentManager PersistentManager;

    public Stats stats { private set; get; }

    public delegate void PlayerRevive();
    public static event PlayerRevive onRevive;

    public delegate void PlayerDeath();
    public static event PlayerDeath onDeath;

    public delegate void UpdateHealthEvent(int h,int maxH);
    public static event UpdateHealthEvent onHealthUpdate;

    public delegate void UpdateOrbsEvent(int orbs);
    public static event UpdateOrbsEvent onOrbsUpdate;

    List<Targetable> targetables;

    protected override void Awake()
    {
        alive = true;

        target_ = null;
        lockedOn = false;
        selector_ = GetComponent<SpellSelector>();
        casting_ = false;
        animator_ = GetComponent<Animator>();
        inControl = true;
        fallen = false;

        invincible_ = false;
        recovering_ = false;

        targetables = new List<Targetable>();
    }

    float reviveAllowDelayTime = 2f;
    float reviveAllowDelayTimer = 0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        Spawn();

        health = stats.maxHealth;
        maxHealth_ = health;


        onHealthUpdate(health, maxHealth_);
        onOrbsUpdate(stats.orbs);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (alive)
        {

        if (lockedOn && target_ == null) //If its locking on
        {
            if (!searchEnemy()) { lockedOn = false; target_ = null; }
            else {  }
        } else if (lockedOn) //if its already Locked on
        {
            updateTargatables();

               if (Input.GetButtonDown("SwitchTarget"))
                {
                    switchTarget();
                }

            //Locked onLogic
           if (checkTargetDistance(target_)) { lockedOn = false; target_ = null; targetables.Clear(); }
        }
        else //if its not locked on
        {
            lockedOn = false;
            target_ = null;
        }

        } else
        {
            if (reviveAllowDelayTimer < reviveAllowDelayTime) reviveAllowDelayTimer += Time.deltaTime;
            else
            {
                if (Input.GetButtonDown("Action"))
                {
                    Revive();
                }
            }

        }


        if (recovering_ && damageRecoveryTimer < damageRecoverTime) damageRecoveryTimer += Time.deltaTime;
        else recovering_ = false;


    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Card"))
        {
            other.gameObject.SetActive(false);
        }

    }

    bool searchEnemy() //Search for an enemy to Lock on
    {
        Targetable[] enemies = FindObjectsOfType<Targetable>(); //find all Enemies in scene
        if (enemies.Length == 0) { return false; } //if no enemy is found, return false

        Targetable closest = null; //last Closest Enemy to Player
        float? oldDistance = null; //last shortest distance

        foreach (Targetable e in enemies) //search the array of enemies found
        {
            float newDistanceFromPlayer = Vector3.Distance(e.transform.position, this.transform.position); //calculate distance from player
            
            if (newDistanceFromPlayer < maxLockOnDistance) //if this enemy is in range
            {
                targetables.Add(e);

                if (closest != null) //if its not the first enemy that's close enough
                {
                    if (newDistanceFromPlayer < oldDistance) closest = e;  //if new distance is less than older
                }
                else
                {
                    closest = e; oldDistance = newDistanceFromPlayer;
                }
            }
        }

            if (closest != null) //if closest if is found, lock on will initiate
            {
                target_ = closest;
                return true;
            }
            else return false;
        }

    bool checkTargetDistance(Targetable target)
    {
        if (Vector3.Distance(target.transform.position, this.transform.position) > maxLockOnDistance) //calculate distance from player
        {
            return true;
        }
        else return false;
    }

    void updateTargatables()
    {
        targetables.Clear();
        Targetable[] enemies = FindObjectsOfType<Targetable>(); //find all Enemies in scene
        if (enemies.Length == 0) { return; } //if no enemy is found, return false

        foreach (var e in enemies)
        {
            float newDistanceFromPlayer = Vector3.Distance(e.transform.position, this.transform.position); //calculate distance from player

            if (newDistanceFromPlayer < maxLockOnDistance) //if this enemy is in range
            {
               targetables.Add(e);
            }
        }
    }

    void switchTarget()
    {
        int listSize = targetables.Count;
        int nextIndex = 0;

        if (listSize > 1)
        {
            int currentIndex = 0;

            for (int i = 0; i < listSize; i++)
            {
                if (targetables[i] == target_) { currentIndex = i; break; }
            }

            nextIndex = currentIndex + 1;
            if (nextIndex + 1 > listSize) nextIndex = 0;

            Debug.Log(nextIndex);

            target_ = targetables[nextIndex];
        }
    }
    

    public Targetable GetTarget() => target_; //Gets the target on witch the player is locked on


    public override void GetDamage(Attack.ATTACK a)
    {
        if (!recovering_ || !invincible_)
        {
            PlayerController c = GetComponent<PlayerController>();
            c.resetOutsideForce();
            c.OutsideForce(a.damageDirection, (float)a.force);
            if (a.element == Attack.Element.PHYSICAL) spawnSFX();
            selector_.cancelCast();

            lowerHealth(a.damageTaken);
        }
    }

    public void launch(Vector3 dir, float force)
    {
        PlayerController c = GetComponent<PlayerController>();
        c.resetOutsideForce();
        c.OutsideForce(dir, force);
    }

    void GetLaunched(Vector3 dir, float force)
    {
        PlayerController c = GetComponent<PlayerController>();
        c.resetOutsideForce();
        c.OutsideForce(dir, force);
        selector_.cancelCast();
    }

    protected void spawnSFX()
    {
        Vector3 pos = transform.position + transform.up * 2;
        GameObject a = Instantiate(SFX, pos, Quaternion.identity);

        FloatingSoundEffects sfx = a.GetComponent<FloatingSoundEffects>();
        sfx.cam = cam.gameObject;
     }


    public void endCast()
    {
        casting_ = false;
    }
    public void startCast()
    {
        casting_ = true;
    }

    public bool getCasting() => casting_;

    public Vector3 getShootingDirection()
    {
        if (Inspect_) return cam.transform.forward;
        else return transform.forward;
    }

    public bool getCoolDown() => selector_.getCooldown();


    public SpellSelector getSelector() => selector_;

    public Transform getCirclePos()
    {
        return gameObject.transform.Find("MagicCircle");
    }

    public Transform getShotPos()
    {
        return gameObject.transform.Find("ShotPlace");
    }

    public Animator getAnimator()
    {
        return animator_;
    }


    public bool getInspect() => Inspect_;

    public void InspectOn()
    {
        Inspect_ = true;
        
    }

    public void InspectOff()
    {
        Inspect_ = false;
    }

    public Transform getInpsectorPos()
    {
        return gameObject.transform.Find("Inspector");
    }


    public void unlockPower(string power)
    {
        switch (power)
        {
            case "Double Jump":

                break;
            default:
                throw new System.Exception("Power Doesn't Exist");
        }
    }




    void sendStats()
    {

    }


    public void increaseOrbs(int value)
    {
        if (stats.orbs < 999999999)
        {
            stats.orbs += value;

            PersistentManager.updateCurrentStats(stats);
            onOrbsUpdate(stats.orbs);
        }
    }

    public void gainPower(string power)
    {
        stats.gainPower(power);

        PersistentManager.updateCurrentStats(stats);
    }

    public void fall(bool toDeath)
    {
        if (!fallen)
        {
            fallen = true;
            if (toDeath) alive = false;
            inControl = false;

            animator_.SetBool("hasFallen", true);

                CameraController c = cam.GetComponent<CameraController>();
                c.inControl = false;
        }        
    }

    public void GetUp()
    {
        animator_.SetBool("hasFallen", false);

        CameraController c = cam.GetComponent<CameraController>();
        c.inControl = true;
    }

    public void resumeControl() {
        fallen = false;
        inControl = true;
    }

    void lowerHealth(int damage)
    {
        health -= damage;
        onHealthUpdate(health, maxHealth_);

        if (health <= 0) die();
    }

    public void die()
    {
        alive = false;
        inControl = false;
        animator_.SetBool("hasFallen", true);
        onDeath();
    }

    public void Spawn()
    {
        stats = new Stats(PersistentManager.currentstats);
        Debug.Log("created Stats for player, double jump is: " + stats.doubleJump);


        PersistentManager.saveStats(stats);
    }

    public void Revive()
    {
        PersistentManager.reloadStats();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
