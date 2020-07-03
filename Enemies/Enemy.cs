using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Targetable
{
    
    protected Player player;     
    protected float outOfReachTimer_; //Out of reach Timer;
    public float OORtreshholdTime;
    protected Vector3 spawn_;
    public GameObject text3d;
    public float speechInterval;
    protected float speechTimer_;
    protected float lastTimeSpoken_;
    protected float distanceFromPlayer_;
    public float radarDistance;
    protected bool spoken_;
    public bool inAttackRange;
    public float speed; //How fast the entity Moves
    protected CharacterController controller_; //Character controller
    protected Vector3 verticalDir_;
    public float rotationSpeed;
    public float gravityScale;
    protected int colliderNumber_;
    protected Vector3 target_;
    protected NavMeshAgent agent_;
    public int stunTresHold;
    protected int stunHealth_;
    protected float stunTimer_;
    public float stunTime; //Time of Stun
    public float stunInterval; //Time between stuns
    protected float stunIntervalTimer_;
    protected bool canBeStunned_; //can or not be stunned
    public GameObject stunObject;
    protected Transform emotion1T_; //transform to spawn emoticons and text;
    protected float searchTimer_;
    public float searchInterval;
    public LocalEnemyManager manager = null;


    public enum StatusEffect
    {
        NULL,
        BURNED
    }
    protected StatusEffect kEffect_;
    protected float tickTimer_;
    protected int tickTreshHold_;
    public int maxTicks;
    public float tickTime;
    public float burnDamage;

    protected bool Enraged;
    protected bool sleeping;

    override protected void Awake()
    {
        Enraged = false;
        sleeping = false;

        LocalEnemyManager.SubcribeSlaves += SubscribeToManager;
        Player.onDeath += EnemyDeath;
        Player.onRevive += EnemyRevive;

    }



    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        player = searchClosestEnemy();
        //manager = SubcribeToManager();

        controller_ = GetComponent<CharacterController>();


        outOfReachTimer_ = 0;
        spawn_ = transform.position;
        speechTimer_ = 0f;
        lastTimeSpoken_ = 0;
        distanceFromPlayer_ = Vector3.Distance(transform.position, player.transform.position);
        spoken_ = false;
        verticalDir_ = Vector3.zero;
        colliderNumber_ = 0;
        target_ = spawn_;
        try
        {
            agent_ = GetComponent<NavMeshAgent>();
            agent_.SetDestination(spawn_);
            agent_.updatePosition = false;
            agent_.updateRotation = false;
        } catch { //Incase of no mesh agent
        }

        stunHealth_ = 0;
        stunTimer_ = 0;
        stunIntervalTimer_ = 0;
        canBeStunned_ = true;

        try
        {
            emotion1T_ = transform.Find("Emotion1_Pos").transform;
        }
        catch { }
        tickTimer_ = 0f;



    }

    protected virtual void OnDestroy()
    {
        LocalEnemyManager.SubcribeSlaves -= SubscribeToManager;
        Player.onDeath -= EnemyDeath;
        Player.onRevive -= EnemyRevive;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        
    }

    protected virtual void Attack()
    {
        if (searchTimer_ < searchInterval) searchTimer_ += Time.deltaTime;
        else
        {
            //player = searchClosestEnemy();
            searchTimer_ = 0;
        }

    }

    protected Player searchClosestEnemy()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length < 1)
        {
            Debug.Log("no players found");
            return null;
        }

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject p in players)
        {
            Vector3 diff = p.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < closestDistance)
            {
                closest = p;
                closestDistance = curDistance;
            }
        }        

        Player player;
        player = closest.GetComponent<Player>(); ;
        return player;
    }


    void SubscribeToManager(LocalEnemyManager manager, string location)
    {
        if (location == this.location)
        {           
            this.manager = manager;
            manager.AddEnemy(this);
        }
    }



    protected void speak(QDialog.Status status) //Talking
    {
        if (Time.timeSinceLevelLoad > lastTimeSpoken_ + 1f)
        {
            createText(status);
            speechTimer_ = 0;
            lastTimeSpoken_ = Time.timeSinceLevelLoad;
        }
    }


    protected void speakWithNoDelay(QDialog.Status status) //speak without having to wait for the previous statment to finish
    {
        createText(status);
        speechTimer_ = 0;
        lastTimeSpoken_ = Time.timeSinceLevelLoad;
    }


    protected void createText(QDialog.Status status)
    {
        QDialog d = GetComponent<QDialog>();
        GameObject t = Instantiate(text3d, emotion1T_.position, Quaternion.identity);

        FloatingText a = t.GetComponent<FloatingText>();
        a.text = d.LoadRandomLine(status);
        a.fontSizeMultiplier = 2;
        a.color = Color.red;
        a.cam = player.GetComponent<Player>().cam.gameObject;
    }

    protected void updatePlayerDistance()
    {
        distanceFromPlayer_ = Vector3.Distance(transform.position, player.transform.position);
    }

    public void die()
    {
        if (manager == null)
        {
            Debug.Log("this Enemy wasn't associated to any manager");
        } else
        {
            manager.RemoveEnemy(this);
        }
        
        Destroy(gameObject);
    }

    public void enrage()
    {
        Enraged = true;
    }

    public void enragedOff()
    {
        Enraged = false;
    }

    public void setSleep(bool sleep)
    {      
        sleeping = sleep;
        agent_.SetDestination(transform.position);
        //agent_.isStopped = sleep;
    }

    public override void GetDamage(Attack.ATTACK a)
    {
        base.GetDamage(a);
    }


    public void EnemyDeath()
    {
        setSleep(true);
    }

    public void EnemyRevive()
    {
        setSleep(false);
    }
}
