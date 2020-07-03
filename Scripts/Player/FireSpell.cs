using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : Spell
{
    public GameObject explosion;
    float travelDist;
    Vector3 startPos;
    public Vector3 direction;
    public float range;


    // Start is called before the first frame update
    void Start()
    {
        kElement_ = Element.FIRE;

        startPos = transform.position;
        travelDist = 0;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
       
        if (travelDist < range)
        {
            movement();
        } else
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
        // Vector3 dir = target.transform.position - transform.position;
        if (target != null)
        {
            direction = target.transform.position - transform.position;
            direction.Normalize();
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    protected override void Explode()
    {
        GameObject a = Instantiate(explosion, transform.position, Quaternion.identity);


        base.Explode();
    }


}
