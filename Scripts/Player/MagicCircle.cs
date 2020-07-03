using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    public Transform player;
    ParticleSystem system;
    Vector3 pos;
    Vector3 previousPos;
    public float simulationSpeedOnCancel;
    public float time;
    public bool end;
    public SpellSelector selector;


    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        previousPos = pos;
        system = GetComponent<ParticleSystem>();
        end = false;
        
    }

    void OnDestroy()
    {
        selector.circles.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        previousPos = pos;
        pos = player.position;
        if (end == true)
        {
            var main = system.main;
            main.simulationSpeed = simulationSpeedOnCancel;
        }

        
    }
}
