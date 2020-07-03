using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : CameraManager
{

    protected override void Awake()
    {


        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GetMainCamera();
        InitiateScene(location);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetMainCamera()
    {
        cameras.Add(0, GameObject.FindGameObjectWithTag("Camera"));
        totalCameras_++;
    }
}
