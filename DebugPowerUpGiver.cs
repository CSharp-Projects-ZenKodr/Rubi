using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPowerUpGiver : MonoBehaviour
{
    public int RedOrbs;
    public bool DoubleJump;
    public bool Dodge;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void givePower(GameObject o)
    {
        Player p = o.GetComponentInParent<Player>();

        if (DoubleJump)
        {            
            p.gainPower("Double Jump");
        }
        if (Dodge)
        {
            p.gainPower("Dodge");
        }

        p.increaseOrbs(RedOrbs);


        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            givePower(other.gameObject);
        }
    }




}
