using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTriggerTeleport : MonoBehaviour
{
    Player p;

    public float TimeToTeleport;
    float TimerToTeleport;

    bool triggered;

    GameObject PlaceToTeleport;

    // Start is called before the first frame update
    void Start()
    {
        PlaceToTeleport = transform.GetChild(0).gameObject;


    }

    // Update is called once per frame
    void Update()
    {
        if (triggered) {
            if (TimerToTeleport < TimeToTeleport) TimerToTeleport += Time.deltaTime;
            else
            {
                if (p != null)
                {
                    p.transform.position = PlaceToTeleport.transform.position;
                    triggered = false;
                    TimerToTeleport = 0;
                }
                else
                {
                    Debug.Log("player was null");
                }
            }
        }


    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            p = c.gameObject.GetComponent<Player>();
            p.fall(false);
            triggered = true;
         }
        else if (c.gameObject.CompareTag("Enemy"))
            {


        }
    }
}
