using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController : ProcessingController
{
    public delegate void Die();

    public Die dieEvent;

    SpriteRenderer spriteRenderer;


    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        SetValue(maxValue);
        //spriteRenderer.color = Color.red;
    }

    public void SetHP(int hp)
    {
        maxValue = hp;
        currentValue = hp;
        EditValue(hp);
    }

    public void ChangeHP(int hp)
    {
        EditValue(currentValue - hp);
    }

    protected override void OnUpdate(float currentValue)
    {
        //Vector3 newScale = new Vector3((float)currentValue / maxValue, transform.localScale.y, transform.localScale.z);

       // transform.localScale = newScale;
        if(currentValue == 0)
        {
            if(dieEvent != null)
            {
                dieEvent();
            }
        }
    }


}
