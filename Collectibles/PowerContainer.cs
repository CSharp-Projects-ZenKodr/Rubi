using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerContainer : Targetable
{
    public float rechargeTime;
    float rechargeTimer;
    bool recharging;
    public float chargeInterval;
    float chargeTimer;

    public delegate void OnDestruction(string location);
    public static event OnDestruction onDeathStart;

    public delegate void OnDeath(string location);
    public static event OnDeath onDeath;

    Animator animator;

    protected override void Awake()
    {
        recharging = false;
        rechargeTimer = 0;
        chargeTimer = 0;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!recharging)
        {
            if (health > 0)
            {
                if (rechargeTimer < rechargeTime) rechargeTimer += Time.deltaTime;
                else
                {
                    rechargeTimer = 0;
                    recharging = true;
                }
            }
        } else
        {
            if (chargeTimer < chargeInterval) chargeTimer += Time.deltaTime;
            else 
            {
                chargeTimer = 0;
                health -= 1;
                if (health < 1)
                {
                    resetRecharging();
                }
            }
        }  
        
    }

    public override void GetDamage(Attack.ATTACK a)
    {
        health++;
        resetRecharging();

        checkHealth();
    }


    void checkHealth()
    {
        if (health == 10)
        {
            health = 0;
            startDeath();
        }
    }

    void removeChildren()
    {
        for (var i = 0; i < gameObject.transform.childCount; i++)
        {
            var a = gameObject.transform.GetChild(i);
            a.transform.parent = null;
        }
    }

    void resetRecharging()
    {
        recharging = false;
        rechargeTimer = 0;
        chargeTimer = 0;
    }

    void startDeath()
    {
        animator.SetBool("canExplode", true);
        removeChildren();
        onDeathStart?.Invoke(location);
        
     }

    void die()
    {
        onDeath(location);
        Destroy(gameObject);
    }
}
