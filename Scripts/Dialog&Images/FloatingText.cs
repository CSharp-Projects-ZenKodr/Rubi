using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : Floater
{

    public string text;
    MeshRenderer mesh;
    TextMesh text3d;   
    public Color color;
    float r, g, b;


    public int fontSizeMultiplier = 1;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        alpha = 1;
        text3d = GetComponent<TextMesh>();

        text3d.text = text;
        text3d.fontSize *= fontSizeMultiplier;
       
        text3d.color = color; //reads color
        r = color.r; //Extracts color
        g = color.g;
        b = color.b;
        
        text3d.alignment = TextAlignment.Center;


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();



        transform.position += dir * speed * Time.deltaTime;

        if (lifeTimer < lifeTime) lifeTimer += Time.deltaTime;
        else
        {
            if (alpha >= 0) //Fades Text
            {
                alpha -= fadeSpeed * Time.deltaTime;
                //text3d.color = new Color(r, g, b, alpha);
            }
            else
                Destroy(this.gameObject);
        }


        if (lifeTimer < lifeTime) lifeTimer += Time.deltaTime;
        else
        {
            if (alpha >= 0) //Fades Text
            {
                alpha -= fadeSpeed * Time.deltaTime;
                text3d.color = new Color(r, g, b, alpha);
            }
            else
                Destroy(this.gameObject);
        }

    }
}
