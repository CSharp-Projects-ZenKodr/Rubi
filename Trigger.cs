using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    protected bool triggered;
    public string location;

    public delegate void TriggerEvent(string location);
    public static event TriggerEvent trigger;


    protected virtual void Awake()
    {
        triggered = false;
    }

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
            trigger(location);
            triggered = true;

        }
    }
}
