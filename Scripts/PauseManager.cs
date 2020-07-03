using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    bool paused;
    bool slowMo;


    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        slowMo = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Pause")) //Jumping when space is pressed
        {

            if (paused)
            {
                Time.timeScale = 1;
                paused = false;
            }
            else { Time.timeScale = 0; paused = true; }



        }
        if (Input.GetButtonDown("SlowMo")) //Jumping when space is pressed
        {
            if (slowMo)
            {
                Time.timeScale = 1;
                slowMo = false;
            }
            else { Time.timeScale = 0.2f; slowMo = true; }
        }

        if (Input.GetButtonDown("Exit"))
        {            
            SceneManager.LoadScene("Tittle Screen");

        }



    }


    public bool getPaused() => paused;
}
