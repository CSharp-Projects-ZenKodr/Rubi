using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPower : MonoBehaviour
{
    bool free_;
    public GameObject player;
    public float maxSpeed;
    float speed_;
    public string power; //Power to Gain
    bool caught_;
    public float caughtTime;
    float caughtTimer_;
    bool canGoToPlayer_;
    public float waitTime;
    float waitTimer_ = 0;



    public delegate void OnObtain(string power);
    public static event OnObtain onObtain;

    public delegate void OnDestroy_(string power);
    public static event OnDestroy_ onDestroy_;

    void awake()
    {
        free_ = false;
        caught_ = false;
        canGoToPlayer_ = false;
        speed_ = 0;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        LocalCameraManager.sendPlayer += getPlayer;
        PowerContainer.onDeath += startGoto;
    }

    void OnDestroy()
    {
        LocalCameraManager.sendPlayer -= getPlayer;
        PowerContainer.onDeath -= startGoto;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent == null) {
            free_ = true;
        }

        if (canGoToPlayer_ && !caught_)
        {
            Vector3 dir = player.transform.position - transform.position;
            if (speed_ < maxSpeed) speed_ += Time.deltaTime;
            //dir.Normalize();
            transform.position += dir * speed_ * Time.deltaTime;
        }


        if (caught_)
        {
            if (caughtTimer_ < caughtTime) caughtTimer_ += Time.deltaTime;
            else die();

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            givePlayerPower();


        }
    }

    void startGoto(string location)
    {
        if (location == power)
        {
            canGoToPlayer_ = true;
        }
    }


    void givePlayerPower()
    {
        Player p = player.GetComponentInParent<Player>();
        if (p == null) Debug.Log("P is null");
        p.gainPower(power);
        //onObtain(power);
        caught_ = true;
    }

    void die()
    {
        onDestroy_(power);
        Destroy(gameObject);

    }

    void getPlayer(string location, GameObject player)
    {
        if (location == power)
        {
            this.player = player.transform.Find("ShotPlace").gameObject;
        }
    }


}
