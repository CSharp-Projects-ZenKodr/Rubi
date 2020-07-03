using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndlevelPrompt : MonoBehaviour
{
    bool active;
    public string nextScene;
    public string location;

    


    void Awake()
    {
        active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CameraManager.Trigger2 += onTriggerEvent;

        deActivate();
    }

    void OnDestroy()
    {
        CameraManager.Trigger2 -= onTriggerEvent;

    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetButtonDown("Action"))
        {
            SceneManager.LoadScene(nextScene);
        }

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            active = true;
        }
    }


    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            active = false;
        }
    }

    void onTriggerEvent(string location)
    {
        if (this.location == location)
        {
            activate();
        }

    }

    void activate()
    {
        gameObject.SetActive(true);
    }

    void deActivate()
    {
        gameObject.SetActive(false);
    }

}
