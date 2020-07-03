using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TittleManager : MonoBehaviour
{
    public string scene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Exit"))
        {
            Application.Quit(0);
        }

        else if (Input.anyKey)
        {
            SceneManager.LoadScene(scene);
        }



    }
}
