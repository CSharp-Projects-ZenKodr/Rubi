using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public string location;
    public delegate void SubscriptionHandler(CameraManager manager, string location);
    public static event SubscriptionHandler SubcribeSlaves;

    protected Dictionary<int, GameObject> cameras; //Main Camera is always 0

    protected int totalCameras_;
    protected Player player = null;

    public delegate void TriggerEvent(string location);
    public static event TriggerEvent Trigger1;
    public static event TriggerEvent Trigger2;

    protected virtual void Awake()
    {
        cameras = new Dictionary<int, GameObject>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        SubcribeSlaves(this, location);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        Debug.Log("camera Manager at" + location + " has been destroyed");
        cameras.Clear();
    }

    public void InitiateScene(string location)
    {
        if (location == this.location)
        {
            Debug.Log("Scene Initiated");
            player.inControl = false;
            Camera cam1 = cameras[0].GetComponent<Camera>();
            Camera cam2 = cameras[1].GetComponent<Camera>();

            cam1.enabled = false;
            cam2.enabled = true;

            player.inControl = false;
        }
    }

    public virtual void InitiateScene(string location, int scene)
    {
        if (location == this.location)
        {
            Debug.Log("Scene Initiated");
            player.inControl = false;
            Camera cam1 = cameras[0].GetComponent<Camera>();
            Camera cam2 = cameras[scene].GetComponent<Camera>();

            cam1.enabled = false;
            cam2.enabled = true;
            player.inControl = false;
        }
    }


    public virtual void EndScene(string location)
    {
        if (location == this.location)
        {
            Camera cam1 = cameras[0].GetComponent<Camera>();
            Camera cam2 = cameras[1].GetComponent<Camera>();

            cam1.enabled = true;
            cam2.enabled = false;
            player.inControl = true;


            Debug.Log("Scene Ended");

           // Destroy(cam2.gameObject);
           // cameras.Remove(1);
           
        }
    }

    public virtual void EndScene(string location, int scene)
    {
        if (location == this.location)
        {
            Camera cam1 = cameras[0].GetComponent<Camera>();
            Camera cam2 = cameras[scene].GetComponent<Camera>();

            cam1.enabled = true;
            cam2.enabled = false;
            player.inControl = true;


            Debug.Log("Scene Ended");

           // Destroy(cam2.gameObject);
           // cameras.Remove(scene);
            
        }
    }


    public virtual void addCamera(localCamera c, int id)
    {
        cameras.Add(id, c.gameObject);

        //try
        //{
        //    cameras.Add(id, c.gameObject);
        //}
        //catch
        //{
        //    throw new System.Exception("Duplicate Key! check all cameras in location:" + c.location + " key: " + c.cameraId);
        //}
        totalCameras_++;
    }

    public virtual void trigger1()
    {
        try
        {
            Trigger1(location);
        }
        catch { Debug.Log("Nobody was Subscribed to the Trigger1 Event at " + location); };
    }

    public virtual void trigger2()
    {
        try
        {
            Trigger2(location);
        }
        catch { Debug.Log("Nobody was Subscribed to the Trigger2 Event at " + location); };
    }
}
