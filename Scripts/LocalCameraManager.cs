using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCameraManager : CameraManager
{
    public float endTime;
    float endTimer_ = 0;
    bool endScene_ = false;

    public delegate void SendPlayer(string location, GameObject player);
    public static event SendPlayer sendPlayer;

    protected override void Awake()
    {

        PowerContainer.onDeathStart += InitiateScene;
       // NewPower.onObtain += startEndScene;
        NewPower.onDestroy_ += startEndScene;

        base.Awake();
    }

    void OnDestroy()
    {
        PowerContainer.onDeathStart -= InitiateScene;
        // NewPower.onObtain -= startEndScene;
        NewPower.onDestroy_ -= startEndScene;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (endScene_)
        {
            if (endTimer_ < endTime) endTimer_ += Time.deltaTime;
            else EndScene(location);
        }
    }



    void startEndScene(string location)
    {
        if (location == this.location)
        {
            Debug.Log("Scene ending");
            endScene_ = true;
        }        
    }

    public override void EndScene(string location)
    {
        base.EndScene(location);

        endScene_ = false;        
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "Main Camera")
        {
            cameras.Add(0, c.gameObject);
            totalCameras_++;
        }
        if (c.gameObject.CompareTag("Player"))
        {
            if (player == null)
            {
                player = c.gameObject.GetComponent<Player>();
                sendPlayer(location, c.gameObject);
            }
            //Debug.Log(player);
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.name == "Main Camera")
        {
            cameras.Remove(0);
            totalCameras_--;
        }
    }





}
