using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysSpell : Spell
{
    ParticleSystem emitter;
    float travelDist;
    Vector3 startPos;
    public Vector3 direction;
    public float range;


    // Start is called before the first frame update
    void Start()
    {
        kElement_ = Element.PHYSICAL;

        emitter = transform.GetComponentInChildren<ParticleSystem>();
        startPos = transform.position;
        travelDist = 0;
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        travelDist = Vector3.Magnitude(transform.position - startPos);

        if (travelDist < range)
        {
            movement();

        }
        else
        {
            Explode();
        }

    }


    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy")) SendAttack(other.gameObject);
                
        if (!other.gameObject.CompareTag("Player")) Explode();
    }



    void movement()
    {
        //Vector3 dir = target.transform.position - transform.position;
       // Vector3 dir_ = direction.normalized;
        Vector3 dir_ = direction.normalized;
        transform.right = dir_;


        transform.position += direction * speed * Time.deltaTime;
    }

    protected override void Explode()
    {


        base.Explode();
    }


}
