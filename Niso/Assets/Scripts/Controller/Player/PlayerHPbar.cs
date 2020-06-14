using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPbar : ProcessingController
{
    [SerializeField]
    Image HPbar;
 
    public delegate void Die();

    public Die dieEvent;

    void Start()
    {
        SetValue(maxValue);
        HPbar.color = Color.red;
        HPbar = GetComponent<Image>();
    }

    public void SetHP(int hp)
    {
        maxValue = hp;
        currentValue = hp;
        EditValue(hp);
        HPbar.color = Color.green;
    }

    public void ChangeHP(int hp)
    {
        EditValue(currentValue - hp);
    }

    protected override void OnUpdate(float currentValue)
    {
        HPbar.fillAmount = (float)currentValue / maxValue;
        if (currentValue == 0)
        {
            if (dieEvent != null)
            {
                dieEvent();
            }
        }
    }

  
}
