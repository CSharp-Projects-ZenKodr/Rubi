using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float walkSpeed;
    public float gravityScale;
    public float rotationSpeed;
    Player player_;
    public float jumpForce;
    CharacterController controller_;

    Vector3 dir_;
    float currentSpeed_;
    public Vector3 spawn; //Original spawn position
    public GameObject text3d;
    public GameObject jumpSpell;
    bool jumping_; //if the character is jumping
    Vector3 outsideForce_;
    Vector3 slidingDir_;
    bool sliding_ = false;
    public float slopeForceRayLenght = 1f;
    public float slopeForce = 1f;
    bool canDoubleJump_;
    float mass = 3;
    float inpactSmoothFactor = 4;

    bool canDodge_;
    bool dodging_;
    public float dodgeCoolDownTime;
    float dodgeCoolDown_;
    public float dodgeSpeed;
    Vector3 dodgeVector_;
    float dodgePotency_;
    

    void Awake()
    {
        jumping_ = false;
        canDoubleJump_ = true;
        canDodge_ = true;
        dodging_ = false;
        dodgeCoolDown_ = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        player_ = GetComponent<Player>();
        controller_ = GetComponent<CharacterController>();
        dir_ = Vector3.zero;
        spawn = transform.position;
        currentSpeed_ = runSpeed;
        outsideForce_ = Vector3.zero;
        slidingDir_ = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (player_.alive)
        {
            Controls();

            if (player_.fallen) //Falling Control
            {
                if (controller_.isGrounded && player_.getAnimator().GetBool("hasFallen")) //Make the player get up
                {
                    player_.GetUp();        
                } else if (controller_.isGrounded && player_.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Base.IDLE")) {    //let player resume control              
                    player_.resumeControl();
                }
            }

        } else
        {
            dir_.x = 0; dir_.z = 0;
        }
        moveCharacter(dir_);
        AnimateMovement(dir_);


        if (!canDodge_ && dodgeCoolDown_ < dodgeCoolDownTime) dodgeCoolDown_ += Time.deltaTime;
        else if (!canDodge_ && dodgeCoolDown_ > dodgeCoolDownTime)
        {
            dodgeCoolDown_ = 0;
            canDodge_ = true;
        }
        
    }

    void LateUpdate()
    {
        calcOutsideForce();


        if (dodging_)
        {
            Dodge();
        }
    }

    void Controls()
    {        
        if (!player_.getInspect()) //check if you're locked on
        {
            if (player_.inControl && Input.GetButton("LockOn") ) //Locking On
            {
                player_.lockedOn = true;
            }
            else { player_.lockedOn = false; }

            if (player_.lockedOn) //&& (player_.GetTarget() != null)
            {
                lockOnMovement();
                castSpell();
            }
            else
            {
                freeMovement();
            }
        } else {
            inspectMovement();
            castSpell();
        }

        

        if (player_.getInspect() && player_.inControl && Input.GetButtonUp("Inspect") ) player_.InspectOff();
    }

    void freeMovement() //Movement for when the player is not locked on
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);
        if (!player_.inControl) input = Vector2.zero;
        currentSpeed_ = runSpeed;

        Vector3 camF = player_.cam.transform.forward; //Camera's forward
        Vector3 camR = player_.cam.transform.right;  //Camera's right
        camF.y = 0; //Remove the y
        camR.y = 0;  //Remove the y
        camF = camF.normalized; //Normalize the vector
        camR = camR.normalized; //Normalize the vector

        float dirY = dir_.y;

        dir_ = camR * input.x + input.y * camF;
        dir_.y = dirY;

        if (controller_.isGrounded) //Jumping && grounded things
        {
            jumping_ = false;
            canDoubleJump_ = true;
            dir_.y = -1;

            if (!sliding_ && !player_.getInspect() && player_.inControl && Input.GetButtonDown("Jump")) //Jumping when space is pressed
            {
                Jump();
            }

            if (!sliding_ && transform.parent == null && player_.inControl && Input.GetButtonDown("Inspect"))
            {
                player_.InspectOn();
                dir_ = Vector3.zero;

                player_.cam.GetComponent<CameraController>().setUpInspect(gameObject.transform.forward);
            }
        }
        else
        {
            sliding_ = false;
            if (!dodging_) dir_.y += (Physics.gravity.y * gravityScale * Time.deltaTime); //Gravity is applied if not grounded
            else dir_.y = 0;

            if (player_.stats.doubleJump && player_.inControl && canDoubleJump_ && Input.GetButtonDown("Jump"))
            {
                DoubleJump();
            } 
        }
        if (player_.stats.dodge && player_.inControl && Input.GetButtonDown("Action") && canDodge_) //Dodge Code for Free Movement
        {
            //dodgding_ = true;
            //canDodge_ = false;
            //dodgeCoolDown_ = 0;
            startDodge(dir_);
        }

        rotateCharacter(dir_);        
    }

    void lockOnMovement() //Movement for when the played is locked on
    {
        float dirY = dir_.y;

        currentSpeed_ = walkSpeed;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);
        if (!player_.inControl) input = Vector2.zero;

        if (player_.GetTarget() == null)
        {
            //transform.forward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
            Vector3 camF = player_.cam.transform.forward; //Camera's forward
            Vector3 camR = player_.cam.transform.right;  //Camera's right
            camF.y = 0; //Remove the y
            camR.y = 0;  //Remove the y
            camF = camF.normalized; //Normalize the vector
            camR = camR.normalized; //Normalize the vector

            dir_ = camR * input.x + input.y * camF;

            rotateCharacter(dir_);
        }
        else
        {
            Vector3 monsterDirection = player_.GetTarget().transform.position - transform.position;
            monsterDirection.Normalize();
            transform.forward = new Vector3(monsterDirection.x, 0, monsterDirection.z);

            if (!player_.getCasting()) dir_ = transform.right * input.x + transform.forward * input.y;
            else dir_ = Vector3.zero;
        }                  
    
        dir_.y = dirY;

        if (controller_.isGrounded) //Jumping
        {
            dir_.y = -1;
            jumping_ = false;
            canDoubleJump_ = true;            

            if (!sliding_ && player_.inControl && Input.GetButtonDown("Jump")) //Jumping when space is pressed
            {
                Jump();
            }
        }
        else
        {
            sliding_ = false;
            if (!dodging_) dir_.y += (Physics.gravity.y * gravityScale * Time.deltaTime); //Gravity is applied if not grounded
            else dir_.y = 0;

            if (canDoubleJump_ && player_.inControl &&  Input.GetButtonDown("Jump"))
            {
                DoubleJump();
            }
        }
        if (player_.stats.dodge && player_.inControl && Input.GetButtonDown("Action") && canDodge_) //Dodge Code for Free Movement
        {
            //dodgding_ = true;
            //canDodge_ = false;
            //dodgeCoolDown_ = 0;
            startDodge(dir_);
        }

    }

    void inspectMovement()
    {
        Vector3 d; //direction to rotate to
        d = player_.cam.transform.forward;
        //transform.forward = cam.transform.forward;

        rotateCharacter(d);
    }

    void moveCharacter(Vector3 d)
    {
        if (sliding_) controller_.Move(new Vector3(
        slidingDir_.x * currentSpeed_ * Time.deltaTime,
        0,
        slidingDir_.z * currentSpeed_ * Time.deltaTime
        ));

        //if (dodgding_) controller_.Move(new Vector3(
        //d.x * dodgeSpeed * Time.deltaTime,
        //0,
        //d.z * dodgeSpeed * Time.deltaTime
        //));

        controller_.Move(new Vector3(
        d.x * currentSpeed_ * Time.deltaTime,
        0,
        d.z * currentSpeed_ * Time.deltaTime
        ));

        if (!dodging_)
        controller_.Move(new Vector3(
            0,
            d.y * runSpeed * Time.deltaTime,
            0));

        //if (dodgding_) dodgding_ = false;
    }

    void Jump()
    {
        dir_.y = jumpForce;
        jumping_ = true;
        animatorJump(); //jump Animation
        if (player_.getCasting()) player_.getSelector().cancelCast();
    }

    void DoubleJump()
    {
        canDoubleJump_ = false;
        resetOutsideForce();
        createJumpSpell();
        dir_.y = jumpForce;
    }

    void createJumpSpell()
    {
        Instantiate(jumpSpell, player_.getCirclePos().position, Quaternion.identity);
    }

    void rotateCharacter(Vector3 d)
    {
        if (d.x != 0 || d.z != 0) //Turns the player to where they are moving
        {
            Vector3 rotation = new Vector3(d.x, 0, d.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation), Time.deltaTime * rotationSpeed);           
        }
    }

    void AnimateMovement(Vector3 d) //Movement Animation
    {
        if ((d.x != 0 || d.z != 0) && !jumping_ && player_.inControl) {
            if (currentSpeed_ == runSpeed)
            {
                player_.getAnimator().SetBool("Running", true);
                player_.getAnimator().SetBool("Walking_F", false);
                player_.getAnimator().SetBool("Walking_B", false);
                
            } else if (currentSpeed_ == walkSpeed)
            {
                player_.getAnimator().SetBool("Running", false);

                if (player_.GetTarget() != null) //Locked on Animation
                {
                    if (Input.GetAxis("Vertical") > 0) //Forward Animation
                    {
                        player_.getAnimator().SetBool("Walking_F", true);
                        player_.getAnimator().SetBool("Walking_B", false);
                    }
                    else //BackWard Animation
                    {
                        player_.getAnimator().SetBool("Walking_F", false);
                        player_.getAnimator().SetBool("Walking_B", true);
                    }
                } else //Free Movement Animation
                {
                    player_.getAnimator().SetBool("Walking_F", true);
                    player_.getAnimator().SetBool("Walking_B", false);
                }              
            }
        } else
        {
            setAnimationMovementoff();
        }
    }

    void setAnimationMovementoff() //Turn off movement Animation
    {
        player_.getAnimator().SetBool("Running", false);
        player_.getAnimator().SetBool("Walking_F", false);
        player_.getAnimator().SetBool("Walking_B", false);
    }

    void animatorJump() //Jummping Animation
    {
        player_.getAnimator().SetTrigger("isJumping");
    }

    void castSpell()
    { 
        if (player_.inControl && !player_.getCasting() && !player_.getCoolDown())
        {
            if (Input.GetButtonDown("Cast")) player_.getSelector().readySpell(player_.GetTarget());
            else if (Input.GetButtonDown("QuickCast"))
            {
                player_.getSelector().ShootPhys(player_.GetTarget());
            }
        }
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 n = hit.normal;
            if (Vector3.Angle(n, Vector3.up) > 40f) //r.normal != Vector3.up
            {
                Vector3 v1 = Vector3.Cross(Vector3.up, n);
                Vector3 v2 = Vector3.Cross(v1, Vector3.up);

                slidingDir_ = v2;
                sliding_ = true;
            }
        else
        {
            slidingDir_ = Vector3.zero;
            sliding_ = false;
        }   
    }

    void Respawn()
    {
        transform.position = spawn;
    }

    public void OutsideForce(Vector3 dir , float force)
    {     
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        outsideForce_ += dir.normalized * force / mass;
        //outsideForce_ = force;
    }

    void calcOutsideForce()
    {
        if (outsideForce_.sqrMagnitude > 1f)
        {
            controller_.Move(outsideForce_ * Time.deltaTime);
            outsideForce_ = Vector3.Lerp(outsideForce_, Vector3.zero, inpactSmoothFactor * Time.deltaTime);
        }
        else outsideForce_ = Vector3.zero;
        //Debug.Log(outsideForce_.sqrMagnitude);
    }

    void startDodge(Vector3 dir)
    {
        Vector3 dodgeDir;
        if ((dir.x != 0 || dir.z != 0)) dodgeDir = new Vector3(dir.x, 0, dir.z);
        else dodgeDir = new Vector3( -transform.forward.x, 0, -transform.forward.z); //In case of no input, character goes backwards

        dodging_ = true;
        player_.inControl = false;

        dodgeDir.Normalize();
        dodgeVector_ = dodgeDir * dodgeSpeed;
        dodgePotency_ = dodgeVector_.sqrMagnitude;
        //Debug.Log(dodgeVector_.sqrMagnitude);
        canDodge_ = false;
    }

    void Dodge()
    {
        if (dodgeVector_.sqrMagnitude > dodgePotency_ / 3)
        {
            controller_.Move(dodgeVector_ * Time.deltaTime);
            dodgeVector_ = Vector3.Lerp(dodgeVector_, Vector3.zero, inpactSmoothFactor * Time.deltaTime);

            canDodge_ = false;
            dodgeCoolDown_ = 0;

           // Debug.Log(dodgeVector_.sqrMagnitude);
        }
        else endDodge();
    }

    void endDodge()
    {
        dodging_ = false;        
        canDodge_ = false;
        player_.inControl = true;
        dodgeVector_ = Vector3.zero;

        dodgeCoolDown_ = 0;
    }

    public void resetOutsideForce()
    {
        outsideForce_ = Vector3.zero;
    }

    public CharacterController getController()
    {
        return controller_;
    }

    private bool OnSlope()
    {
        if (jumping_) return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller_.height / 2 * slopeForceRayLenght))
            if (hit.normal != Vector3.up) return true;

        return false;
    }
}

