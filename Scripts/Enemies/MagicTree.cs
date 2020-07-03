using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTree : Enemy
{
    public enum State
    {
        IDLE,
        PERSUING,
        ATTACKING,
        RETURNING,
        PLAYER_OUT_OF_RANGE,    
        STUNNED
    }
    State kState_;
    State? kPreviousState_;
       
    meleeAttack attack_;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        kState_ = State.IDLE;
        kPreviousState_ = null;

        health = 100;

        attack_ = transform.GetComponentInChildren<meleeAttack>();
        kEffect_ = StatusEffect.NULL;


    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!sleeping)
        {
            updatePlayerDistance();

            states(); //Update States

            Movement(); //UpdateMovement

            Effect();

            if (speechTimer_ < speechInterval) speechTimer_ += Time.deltaTime;
            else if (kState_ == State.IDLE)
            {
                speak(QDialog.Status.IDLE); //Idling talk
            }
            else
            {
                //Put Idle Engage text here
            }
        }

    }

    void OnTriggerEnter(Collider other) //When something enters a trigger
    {
        if (other.gameObject.CompareTag("Player"))
        {            
            inAttackRange = true;
           if (kState_ != State.STUNNED) Attack();
        }
    }


    void OnTriggerExit(Collider other) //When something exists a trigger
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inAttackRange = false;
        }
    }

    protected override void Attack()
    {
        base.Attack();

        if (attack_.attack(50, 400))
        {
            base.Attack();
            speak(QDialog.Status.ATTACK); //Attack talk
        }

        //Attack Logic here

        
    }

    void states() //AI LOGIC
    {
        if (kState_ != State.ATTACKING && kState_ != State.STUNNED && kState_ != State.PERSUING && Enraged)
        {
            updateState(State.PERSUING);
        }

        if (!inAttackRange && distanceFromPlayer_ < radarDistance)
        {
            if (kState_ == State.IDLE && kPreviousState_ == null)
            {
                if (manager != null && !Enraged)
                {
                    manager.enrageEnemies();
                }

                speakWithNoDelay(QDialog.Status.FOUND_PLAYER); //Speak line if its the first time seeing the player
            }
            else if (kState_ == State.IDLE && (kPreviousState_ != null || kPreviousState_ != State.PERSUING))
            {
                speak(QDialog.Status.PLAYER_IN_RANGE_AGAIN);
            }

            if (kState_ != State.STUNNED) updateState(State.PERSUING); //will enter persuit
        }


        if (kState_ != State.IDLE && kPreviousState_ != State.RETURNING && distanceFromPlayer_ > radarDistance)
        {
            outOfReachTimer_ += Time.deltaTime; //It will start a timer that makes the monster lose the player and go back to spawn
        }

        if (outOfReachTimer_ > OORtreshholdTime)
        {
            outOfReachTimer_ = 0;

            speak(QDialog.Status.PLAYER_LOST);
            updateState(State.RETURNING); //Makes the monster go back,
        }


        if (attack_.Engaged())
        {
            updateState(State.ATTACKING);
        }
        else if (!inAttackRange && kState_ == State.ATTACKING && !attack_.Engaged())
        {
            updateState(State.PERSUING);
        }

        if (kState_ == State.ATTACKING && !attack_.Engaged() && inAttackRange)
        {
            Attack();
        }

        if (kState_ == State.STUNNED && stunTimer_ < stunTime) stunTimer_ += Time.deltaTime;
        else if (kState_ == State.STUNNED)
        {
            updateState(State.PERSUING);
            stunTimer_ = 0;
        }


        if (stunIntervalTimer_ < stunInterval && kState_ != State.STUNNED && !canBeStunned_) stunIntervalTimer_ += Time.deltaTime;
        else if (stunIntervalTimer_ > stunInterval && kState_ != State.STUNNED && !canBeStunned_)
        {
            stunIntervalTimer_ = 0;
            canBeStunned_ = true;
        }

    } //End AI LOGIC

    void Movement() //AI MOVEMENT
    {
        float dirY = verticalDir_.y;

        if (kState_ == State.PERSUING) //If the player has been spotted for the first time
        {
            target_ = player.transform.position;
        }
        else if (kState_ == State.RETURNING) //If the player is lost then it will go back to spawn
        {
            target_ = spawn_;
            if (Vector3.Magnitude(spawn_ - transform.position) < 1) {
                updateState(State.IDLE);
            }            
        }
        else if (kState_ == State.IDLE)
        {
            target_ = spawn_;
        }
        
        if (kState_ == State.ATTACKING) //if the enemy is in Attack range, he will stop to Attack
        {
            target_ = player.transform.position;
        }        

        //verticalDir.Normalize();
        verticalDir_.y = dirY;

        if (controller_.isGrounded) //if the enemyis grounded, he will be pulled with less force
        {
            verticalDir_.y = -1;
        }
        else
        {
            verticalDir_.y += (Physics.gravity.y * gravityScale * Time.deltaTime); //Gravity is applied if not grounded
        }

        agent_.speed = speed;

        Vector3 desDir;

        if (kState_ != State.IDLE && kState_ != State.ATTACKING && kState_ != State.STUNNED && controller_.isGrounded)
        {
            agent_.SetDestination(target_);
            desDir = agent_.desiredVelocity.normalized;

            controller_.Move(desDir * speed * Time.deltaTime);
            agent_.velocity = controller_.velocity;
        }
        else {
            agent_.SetDestination(transform.position);
            desDir = Vector3.zero;
            agent_.velocity = desDir;
        }

        moveCharacter();
        rotateCharacter(desDir);

                       
    } //End AI Movement

    void moveCharacter()
    {
        controller_.Move(verticalDir_ * speed * Time.deltaTime);
    }

    void rotateCharacter(Vector3 desDir)
    {
        if (desDir.x != 0 || desDir.z != 0) //Turns the Entity to where they are moving
        {
            Vector3 rotation = new Vector3(desDir.x, 0, desDir.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation), Time.deltaTime * rotationSpeed);
        }
    }

    void Effect() //EFFECTS DAMAGE LOGIC
    {
        switch (kEffect_)
        {
            case StatusEffect.NULL:
                break;
            case StatusEffect.BURNED:
                if (tickTimer_ < tickTime) tickTimer_ += Time.deltaTime;
                else
                {
                    tickTimer_ = 0f;
                    health -= 10;
                    if (tickTreshHold_ < maxTicks) tickTreshHold_++;
                    else
                    {
                        tickTreshHold_ = 0;
                        kEffect_ = StatusEffect.NULL;
                    }
                    checkHealth();
                }
                break;
        }
    }

    void updateState(State newState)
    {
        kPreviousState_ = kState_;
        kState_ = newState;
        spoken_ = false;
    }

    public override void GetDamage(Attack.ATTACK a)
    {
        base.GetDamage(a);

        switch (a.element)
        {
            case global::Attack.Element.FIRE:
                health -= a.damageTaken;
                kEffect_ = StatusEffect.BURNED;
                break;
            case global::Attack.Element.WATER:
                health += a.damageTaken;
                break;
            case global::Attack.Element.PHYSICAL:
                health -= a.damageTaken;
               if (canBeStunned_) stunHealth_ += a.damageTaken;
                if (stunHealth_ >= stunTresHold && canBeStunned_) stun();
                break;
        }

        checkHealth();
    }

    void checkHealth()
    {
        if (health <= 0) die();
    }

    void stun() //stun this
    {
        canBeStunned_ = false;
        stunHealth_ = 0;
        updateState(State.STUNNED);   
       
        GameObject a = Instantiate(stunObject, emotion1T_.position, Quaternion.identity);

        StunnedStars stars = a.GetComponent<StunnedStars>();
        stars.liveTime = stunTime;
    }





}
