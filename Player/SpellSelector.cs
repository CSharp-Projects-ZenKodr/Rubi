using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSelector : MonoBehaviour
{

    Player player_;
    public GameObject fireSpell;
    public GameObject magicCircle;
    public GameObject physSpell;
    public float castTime;
    float castTimer_;
    float circleTimer_;
    public float coolDownTime;
    float coolDownTimer_;
    bool casting_;
    bool cooldown_;
    public float quickCooldown;

    public List<MagicCircle> circles;

    public enum SpellType
    {
        PHYSICAL,
        FIRE
        
    }
    public SpellType selectedSpell;
    SpellType _nextSpell;

    // Start is called before the first frame update
    void Start()
    {
        player_ = GetComponent<Player>();    
        circleTimer_ = castTime + coolDownTime;
        castTimer_ = castTime;
        coolDownTimer_ = coolDownTime;
        casting_ = false;
        cooldown_ = false;
        selectedSpell = SpellType.FIRE;
        _nextSpell = SpellType.PHYSICAL;
        circles = new List<MagicCircle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (casting_)
        {
            castspell(player_.GetTarget(), _nextSpell);
        } else if (cooldown_)
        {
            CoolDown();
        }

    }

    public void readySpell(Entity target)
    {
        _nextSpell = selectedSpell;
        if (target != null)
        {
            player_.startCast();
            casting_ = true;

            GameObject a = Instantiate(magicCircle, player_.getCirclePos().position, Quaternion.identity);

            MagicCircle c = a.GetComponent<MagicCircle>();
            circles.Add(c);
            c.player = player_.getCirclePos();
            c.selector = this;
        }
    }


    void castspell(Entity target, SpellType nextSpell)
    {
        if (target != null && castTimer_ > 0)
        {
            if (Input.GetButtonUp("Cast")) { cancelCast(); return; }
            castTimer_ -= Time.deltaTime;
        }
        //else if (target == null)
        //{
        //    cancelCast();
        //}
        else
        {
            launchSpell(target, nextSpell);

        }
    }

    void CoolDown()
    {
        if (coolDownTimer_ > 0) coolDownTimer_ -= Time.deltaTime;
        else
        {
            endCooldown();
        }
    }

    public void ShootPhys(Entity target)
    {
        _nextSpell = SpellType.PHYSICAL;
        launchSpell(target, _nextSpell);

    }

    void launchSpell(Entity target, SpellType nextSpell)
    {
        casting_ = false;
        cooldown_ = true;

            switch (nextSpell)
            {
                case SpellType.FIRE:
                    launchFire(target);
                    break;
                case SpellType.PHYSICAL:
                    launchPhsysical(target);
                    break;
            }
    }

    public void cancelCast()
    {
        if (!cooldown_)
        {
            casting_ = false;
            cooldown_ = true;
            coolDownTimer_ = coolDownTime / 2f;
        } else
        {
            casting_ = false;
            cooldown_ = true;
        }

        foreach (var c in circles)
        {
            c.end = true;
        }
    }

    void endCooldown()
    {
        cooldown_ = false;
        player_.endCast();
        circleTimer_ = castTime + coolDownTime;
        castTimer_ = castTime;
        coolDownTimer_ = coolDownTime;
    }

    void launchPhsysical(Entity target)
    {
        Vector3 pos = player_.getShotPos().position;     

        GameObject a = Instantiate(physSpell, pos, Quaternion.identity);
        PhysSpell s = a.GetComponent<PhysSpell>();

        s.direction = player_.getShootingDirection();
        s.target = target;

        coolDownTimer_ = quickCooldown;
    }

    void launchFire(Entity target)
    {
        Vector3 pos = player_.getShotPos().position;

        GameObject a = Instantiate(fireSpell, pos, Quaternion.identity);

        FireSpell s = a.GetComponent<FireSpell>();
        s.direction = player_.getShootingDirection();
        s.target = target;
    }

    public bool getCooldown()
    {
        return cooldown_;
    }
}
