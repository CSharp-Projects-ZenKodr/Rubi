using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            Player p = c.gameObject.GetComponent<Player>();

            if (!p.fallen)
            {
                p.fall(true);
            }
         }
        else if (c.gameObject.CompareTag("Enemy"))
            {
            Enemy e = c.gameObject.GetComponent<Enemy>();

            e.die();

        }
    }
}
