using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSoundEffects : Floater
{
    public float maxSize;
    Vector3 scaler_;    

    // Start is called before the first frame update
    protected  override void Start()
    {
        base.Start();

        scaler_ = transform.localScale;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


       // transform.localScale *= (1 + speed * 0.01f);
        if (transform.localScale.magnitude < maxSize) transform.localScale += (scaler_ * (speed * Time.deltaTime));
        else
        {
            if (lifeTimer < lifeTime)
            {
                lifeTimer += Time.deltaTime;
                transform.localScale += (scaler_ * (speed * 0.1f * Time.deltaTime));
                transform.position += Vector3.up * Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
