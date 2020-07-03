using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public struct ATTACK
    {
        public int damageTaken;
        public int force;
        public Element element;
        public Vector3 damageDirection;
    }
       
    public enum Element
    {
            FIRE,
            WATER,
            NATURE,
            PHYSICAL
     }

    public int damage;
    public int force;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
