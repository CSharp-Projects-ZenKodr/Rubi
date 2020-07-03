using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedStars : MonoBehaviour
{
    public float liveTime;
    float liveTimer_;

    // Start is called before the first frame update
    void Start()
    {
        liveTimer_ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (liveTimer_ < liveTime) liveTimer_ += Time.deltaTime;
        else
        {
            Destroy(gameObject);
        }



    }
}
