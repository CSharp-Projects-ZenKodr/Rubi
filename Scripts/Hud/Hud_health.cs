using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud_health : MonoBehaviour
{
    int health;
    public string text;
    Text textDisplay;


    void Awake()
    {
        textDisplay = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Player.onHealthUpdate += getHealth;
    }

    void OnDestroy()
    {
        Player.onHealthUpdate -= getHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getHealth(int h,int maxHealth)
    {
        health = h;

        textDisplay.text = text + ": " + health;
    }

}
