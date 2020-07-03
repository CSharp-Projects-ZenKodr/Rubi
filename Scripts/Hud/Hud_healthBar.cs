using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud_healthBar : MonoBehaviour
{
    int health;
    //Text textDisplay;
    Image Greenbar;
    Image redBar;

    float greenBarSize;
    float targetRedBarSize;
    float currentRedBarSize;
    float maxRedSize;

    public float redDecreaseSpeed;
    public float redDecreaseDelay;
    float delayTimer;
    bool hit;

    bool firstTime;

    void Awake()
    {
        Image[] bars;
        bars = GetComponentsInChildren<Image>();

        foreach (Image b in bars)
        {
            if (b.gameObject.name == "Red Bar")
            {
                redBar = b;
            }
            else if (b.gameObject.name == "Green Bar") Greenbar = b;                
        }

        firstTime = true;
        delayTimer = 0;

        Player.onHealthUpdate += getHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
               
    }

    void OnDestroy()
    {
        Player.onHealthUpdate -= getHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetRedBarSize < currentRedBarSize) delayTimer += Time.deltaTime;

        if (delayTimer > redDecreaseDelay)
        {
            currentRedBarSize -= redDecreaseSpeed * Time.deltaTime;
            redBar.rectTransform.localScale = new Vector3(currentRedBarSize, 1, 1);
            if (targetRedBarSize >= currentRedBarSize) { delayTimer = 0; }
        }


    }

    void getHealth(int h, int maxHealth)
    {
        health = h;
        CalcBarSize(health, maxHealth);

        Greenbar.rectTransform.localScale = new Vector3(greenBarSize, 1, 1);

        if (firstTime)
        {
            redBar.rectTransform.localScale = new Vector3(targetRedBarSize, 1, 1);
            firstTime = false;
        } 
    }

    void CalcBarSize(int health, int maxHealth)
    {
        greenBarSize = (float)((1f * health) / maxHealth);
        targetRedBarSize = greenBarSize;

        if (firstTime)
        {
            currentRedBarSize = targetRedBarSize;
            maxRedSize = currentRedBarSize;
        }
    }
}
