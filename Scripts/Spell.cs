using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Attack
{

    public Entity target;
    public float speed;
    protected Element kElement_;
    public float LifeTime;
    protected float lifeTimer_;


    // Start is called before the first frame update
    void Start()
    {
        lifeTimer_ = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (lifeTimer_ < LifeTime) lifeTimer_ += Time.deltaTime;
        else
        {
            Destroy(gameObject);
        }

    }

    protected void SendAttack(GameObject o)
    {
        ATTACK a;
        a.damageTaken = damage;
        a.element = kElement_;
        a.force = force;

        Vector3 dir = transform.forward + transform.up;

        a.damageDirection = dir * damage;


        //o.SendMessage("GetDamage", a);

        Entity e = o.gameObject.GetComponent<Entity>();
        e.GetDamage(a);
    }


    public void selectedTarget(Entity t)
    {
        if (t == null) Debug.Log("Target arrived null");
        else Debug.Log("Target arrived");

        target = t;
    }


    protected virtual void Explode()
    {
        Destroy(gameObject);
    }
}
