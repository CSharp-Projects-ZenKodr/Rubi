using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlat : MonoBehaviour
{
    bool startFromBeginning;
    public bool canPlayerNotMove;

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
        if (!startFromBeginning && c.gameObject.CompareTag("Player"))
        {
                MovingPlatController m = transform.parent.GetComponent<MovingPlatController>();
                m.startMoving();
            if (canPlayerNotMove)
            {
                Player p = c.gameObject.GetComponent<Player>();
                p.inControl = false;
            }
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            c.gameObject.transform.parent = transform.parent.transform;
        }
    }


    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            c.gameObject.transform.parent = null;
        }
    }

    public void setStart(bool startFromBeginning)
    {
        this.startFromBeginning = startFromBeginning;
    }
}
