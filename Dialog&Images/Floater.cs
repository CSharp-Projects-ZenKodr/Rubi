using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public float lifeTime;
    protected float lifeTimer;
    protected float alpha;
    protected Vector3 dir;
    public float speed;
    [Range(0.01f, 1.0f)]
    public float rotateSpeed;
    public float fadeSpeed;

    public GameObject cam;
    protected Vector3 cameraDir;




    // Start is called before the first frame update
    protected virtual void Start()
    {
        lifeTimer = 0;

        cam = GameObject.FindGameObjectWithTag("Camera");

        cameraDir = transform.position - cam.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraDir), 1); //rotates Text

    }

    // Update is called once per frame
    protected virtual void Update()
    {

        cameraDir = transform.position - cam.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraDir), rotateSpeed * Time.deltaTime); //rotates Text slowly




    }

    public GameObject getCam()
    {
        return cam;
    }

}
