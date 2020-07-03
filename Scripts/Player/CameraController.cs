using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;
    Vector3 offset_;
    [Range(1, 2)]
    public float rotationSpeed = 1f;
    [Range(1, 5)]
    public float inspectSpeed = 2.5f;
    public bool rotateAroundPlayer = true;
    public bool inControl = true;

    [Range(5f, 15f)]
    public float smoothFactor = 10f;
    float side_ = 1;

    [Range(5, 20)]
    public float currentDistance = 5;
    public float maxDistance = 20;
    float lockOnOffSet_ = 1.2f; //Extra distance for the offset during lockon
    Vector3 targetOffSet_;

    [Header("Layer(s) to include")]
    public LayerMask CamOcclusion;                //the layers that will be affected by collision

    Vector3 camPos_;
    Vector3 target_;
    Vector3 lastCamPos_;

    void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
        updateOffset();
        targetOffSet_ = player.transform.position + new Vector3(0, 2, 0);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate() //garanteed to run after after all items have been proceed in update
    {
        
        if (!player.getInspect())
        {
            targetOffSet_ = targetOffSet_ = player.transform.position + new Vector3(0, 4, 0);

            if (!player.lockedOn) freeCameraControl();
            else lockedOnControl();


            transform.forward = target_ - transform.position;


            if (inControl) smoothMethodPos();
            transform.LookAt(target_);

            
        }
        else FirstPersonControl();


  
    }

    public void updateOffset()
    {
        Vector3 cameraPoint = player.transform.position + Vector3.up * currentDistance - player.transform.forward * (currentDistance + maxDistance * 0.5f);

        offset_ = cameraPoint - player.transform.position;
    }

    void freeCameraControl()
    {
        cameraInput();

        if (Input.GetButtonDown("Switch"))
        {
            Vector3 cameraPoint = player.transform.position + Vector3.up * currentDistance - player.transform.forward * (currentDistance + maxDistance * 0.5f);

            offset_ = cameraPoint - player.transform.position;
        }

        camPos_ = player.transform.position + offset_;

        occludeRay(ref targetOffSet_);

        smoothMethodTarget(targetOffSet_);

    }

    void cameraInput()
    {
        if (rotateAroundPlayer)
        {
            float Horizontal = Input.GetAxis("Mouse X");
            float Vertical = Input.GetAxis("Mouse Y");


            Quaternion camTurnAngle = Quaternion.AngleAxis((Horizontal) * rotationSpeed, Vector3.up);

           // camTurnAngle *= Quaternion.AngleAxis(Vertical * rotationSpeed, Vector3.right);

            offset_ = camTurnAngle /* camYTurnAngle */ * offset_;

        }
    }

    void lockedOnControl()
    {
        Targetable enemy = player.GetTarget();
        Vector3 middlePoint;
        Vector3 direction;

        if (enemy != null)
        {
            middlePoint = (enemy.transform.position + player.transform.position) / 2;

            direction = enemy.transform.position - player.transform.position;

            if (Input.GetButtonDown("Switch"))
            {
                side_ *= -1;
            }

            float angle = Vector3.Dot(direction * side_, offset_);

            Quaternion camTurnAngle = Quaternion.AngleAxis(angle * Time.deltaTime, Vector3.up);

            offset_ = camTurnAngle * offset_;
        } else
        {
            middlePoint = player.transform.position + player.transform.forward;
            middlePoint += new Vector3(0, 2, 0);

            direction = middlePoint - player.transform.position;

            cameraInput();
        }

        camPos_ = player.transform.position + offset_ * lockOnOffSet_;

        // target_ = middlePoint;
        smoothMethodTarget(middlePoint);
                               
    }



    void smoothMethodPos()
    {
        transform.position = Vector3.Slerp(transform.position, camPos_, smoothFactor * Time.deltaTime);
    }    

    void occludeRay(ref Vector3 targetFollow)
    {
        #region prevent wall clipping
        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();
        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Linecast(targetFollow, targetOffSet_ + offset_, out wallHit, CamOcclusion))
        {
            //the smooth is increased so you detect geometry collisions faster.
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.
            camPos_ = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPos_.y, wallHit.point.z + wallHit.normal.z * 0.5f);
        }
        #endregion
    }

    void smoothMethodTarget(Vector3 newTarget)
    {
        target_ = Vector3.Slerp(target_, newTarget, smoothFactor * Time.deltaTime);
    }

    private float Yaw_;
    private float Pitch_;

    void FirstPersonControl()
    {      

        camPos_ = player.getInpsectorPos().position;

        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Yaw_ += input.x * (Mathf.Rad2Deg * inspectSpeed) * Time.deltaTime;
        Pitch_ -= input.y * (Mathf.Rad2Deg * inspectSpeed) * Time.deltaTime;
        Pitch_ = Mathf.Clamp(Pitch_, -45, 45); 

        transform.localRotation = Quaternion.AngleAxis(Yaw_, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(Pitch_, Vector3.right);


        smoothMethodPos();
    }

    public void setUpInspect(Vector3 d) //set pitch and Yaw
    {
        Pitch_ = 0; //reset Pitch to 0

        Yaw_ = SignedAngleBetween(Vector3.forward, new Vector3(d.x, 0, d.z), Vector3.up); //Reset Yaw to what character is looking at

        transform.localRotation = Quaternion.AngleAxis(Yaw_, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(Pitch_, Vector3.left);

    }


    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        // angle in [0,360] 
        //float angle360 =  (signed_angle + 180) % 360;

        return signed_angle;
    }



}
