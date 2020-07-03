using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud_OrbsDisplay : MonoBehaviour
{
    int orbs;
    int displayOrbs;
    Text textDisplay;
    bool justInitiated;

    void Awake() {

        justInitiated = true;

        textDisplay = GetComponent<Text>();
        Player.onOrbsUpdate += getOrbs;
    }

    // Start is called before the first frame update
    void Start()
    {
        
 

        
    }

    void OnDestroy()
    {
        Player.onOrbsUpdate -= getOrbs;
    }

    // Update is called once per frame
    void Update()
    {
        if (displayOrbs < orbs)
        {
            displayOrbs++;
            updateOrbs(displayOrbs);
        }


    }

    void getOrbs(int orbs)
    {
        this.orbs = orbs;

        if (justInitiated) {
            updateOrbs(orbs);
            justInitiated = false;
        }


    }

    void updateOrbs(int orbs)
    {
        string display = orbs.ToString();
        display =  display + " x";

        textDisplay.text = display;
    }
}
