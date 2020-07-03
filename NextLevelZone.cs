using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelZone : MonoBehaviour
{
    CameraController cam_;
    Vector3 enterPos_;
    Vector3 CurrentPos_;
    Vector3 camEnterPos_;
    float currentDistance_;
    bool PlayerIn_;
    bool CamIn_;

    public float TransitionDistance;
    public string nextScene;


    void Awake()
    {
        PlayerIn_ = false;
        cam_ = null;
        CamIn_ = false;
        resetDistance();
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
            PlayerEnter(c);
        }
        if (c.gameObject.CompareTag("Camera"))
        {
            if (cam_ == null) cam_ = c.GetComponent<CameraController>();
            camEnter();
        }

        Debug.Log(c.tag);
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            computeDistance(c);

            if (currentDistance_ > TransitionDistance)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player left");
            PlayerLeave();
        }
    }

    void computeDistance(Collider c)
    {
        if (cam_ != null && CamIn_ && !cam_.player.getInspect()) cam_.transform.position = camEnterPos_;
        CurrentPos_ = c.transform.position;

        currentDistance_ = Vector3.Magnitude(CurrentPos_ - enterPos_);
    }

    void resetDistance()
    {
        enterPos_ = Vector3.zero;
        CurrentPos_ = Vector3.zero;
        currentDistance_ = 0;
    }

    void camEnter()
    {
        camEnterPos_ = cam_.transform.position;
        CamIn_ = true;
        cam_.inControl = false;
    }

    void CamLeave()
    {
        camEnterPos_ = Vector3.zero;
        CamIn_ = false;
        cam_.inControl = true;
    }

    void PlayerEnter(Collider c)
    {
        PlayerIn_ = true;
        enterPos_ = c.transform.position;
        CurrentPos_ = c.transform.position;
        currentDistance_ = 0;
    }

    void PlayerLeave()
    {
        PlayerIn_ = false;
        CamLeave();
        resetDistance();
    }

}
