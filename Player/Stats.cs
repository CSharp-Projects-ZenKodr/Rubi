using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{    
    public bool doubleJump { private set; get; }
    public bool dodge { private set; get; }
    public int orbs;
    public int maxHealth;

    public Stats(int orbs, bool doubleJump, bool Dodge)
    {
        this.doubleJump = doubleJump;
        this.dodge = Dodge;
        this.orbs = orbs;
    }

    public Stats()
    {
        doubleJump = false;
        dodge = false;
        orbs = 0;
        this.maxHealth = 100;
    }

    public Stats(Stats s)
    {
        this.doubleJump = s.doubleJump;
        this.dodge = s.dodge;
        this.orbs = s.orbs;
        this.maxHealth = s.maxHealth;

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateAllStats(Stats s)
    {
        orbs = s.orbs;
        doubleJump = s.doubleJump;
        dodge = s.dodge;
        maxHealth = s.maxHealth;
    }

    public void gainPower(string power)
    {
        switch (power)
        {
            case "Double Jump":
                doubleJump = true;
                break;
            case "Dodge":
                dodge = true;
                break;
            default:
                throw new System.Exception(power + "DOES NOT EXISTS");
                break;
        }
    }



}
