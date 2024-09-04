using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingItems : InteractuableItem
{
    public Gradient gradient;
    public float time;

    private SpriteRenderer sr;
    private float timer = 0;

    private bool glow = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (glow)
        {
            if(timer < time)
            {
                timer += Time.deltaTime;
            }

            sr.color = gradient.Evaluate(timer / time);
        }
        else
        {

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            sr.color = gradient.Evaluate(timer / time);
        }
    }

    protected override void StartInteraction()
    {
        glow = true;
    }
    protected override void EndInteraction()
    {
        glow = false;
    }

}
