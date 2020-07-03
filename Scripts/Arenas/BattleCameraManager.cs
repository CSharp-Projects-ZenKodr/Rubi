using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraManager : CameraManager
{
    LocalEnemyManager lem;




    protected override void Awake()
    {
        

        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        
        lem = GetComponent<LocalEnemyManager>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GetMainCamera();
        base.Start();
    }

    void GetMainCamera()
    {
        cameras.Add(0, GameObject.FindGameObjectWithTag("Camera"));
        totalCameras_++;
    }

    public override void trigger1()
    {
        base.trigger1();

        lem.activateDoors(false);
        
    }

    public override void trigger2()
    {
        base.trigger2();


        lem.deactivateDoors(false);
    }

    public override void InitiateScene(string location, int scene)
    {
        base.InitiateScene(location, scene);
        StartCamera(scene);
    }

    public override void EndScene(string location)
    {
        base.EndScene(location);

        lem.setEnemySleep(false);
    }

    public override void EndScene(string location, int scene)
    {
        base.EndScene(location, scene);

        lem.setEnemySleep(false);
    }

    public void StartCamera(int scene)
    {
        localCamera cam = cameras[scene].GetComponent<localCamera>();

        try
        {
            cam.animator.SetBool("isReady", true);
        } catch
        {

        }
    }

}
