using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedOrb : MonoBehaviour
{
    public int Red_Orb_Value;
    public bool playerinRange;
    Player p;
    private Collider[] colliders;
    private int colliderNumber;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponents<Collider>();
        colliderNumber = 0;
        p = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerinRange)
        {
            Vector3 dir = p.transform.position - transform.position;
            dir.Normalize();
            transform.position += dir * speed * Time.deltaTime;
        }



    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colliderNumber++;

            if (colliderNumber == 1)
            {
                playerinRange = true;
                p = other.gameObject.GetComponent<Player>();
            }
            if (colliderNumber == 2)
            {
                p.increaseOrbs(Red_Orb_Value);
                Destroy(gameObject);
            }


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colliderNumber = 0;
            playerinRange = false;
            p = null;
        }
    }

}
