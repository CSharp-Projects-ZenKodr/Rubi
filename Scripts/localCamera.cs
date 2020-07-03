using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localCamera : Subscriber
{
    CameraManager manager;
    public int cameraId;
    public Transform target; //This Must be Set Manually
    public bool hasTarget;
    Camera cam;
    public Animator animator;
    public int Animation;
    

    void Awake()
    {
        CameraManager.SubcribeSlaves += SubscribeToManager;
        cam = GetComponent<Camera>();

        try
        {
            animator = GetComponent<Animator>();

            animator.SetInteger("Animation", Animation);
        } catch
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnDestroy()
    {
        CameraManager.SubcribeSlaves -= SubscribeToManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget)
        {
            transform.LookAt(target);
        }
    }

    void SubscribeToManager(CameraManager manager, string location) //New Version
    {
        if (this.location == location)
        {
            this.manager = manager;
            manager.addCamera(this, cameraId);
        }
    }

    void selfEndScene()
    {
        if (manager == null) Debug.Log("manager at " + location + " was null, Camera id: " + cameraId);
        manager.EndScene(location, cameraId);
    }

    void selfEndSceneWithId()
    {
        manager.EndScene(location, this.cameraId);
    }


    void trigger1()
    {
        manager.trigger1();
    }

    void trigger2()
    {
        manager.trigger2();
    }
}
