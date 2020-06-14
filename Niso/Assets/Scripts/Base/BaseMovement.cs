using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BaseController : MonoBehaviour
{
    [SerializeField] protected float timer;
    [SerializeField] LeanTweenType type;
    int id;
    protected void MoveTo(Vector3 pos, Action action = null)
    {
        LeanTween.move(gameObject, pos, timer).setEase(type).setOnComplete(action);
        
    }
    protected void MoveUpdate(Vector3 pos)
    {
        LeanTween.cancel(id);
        id = LeanTween.move(gameObject, pos, timer).setEase(type).id;
    }
     protected void UpdateValue(float _firstValue,float _lastValue,Action<float> updateValue,Action onComplete)
     {
        LeanTween.cancel(id);
        id = LeanTween.value(gameObject,updateValue,_firstValue,_lastValue,timer).setEase(type).setOnComplete(onComplete).id;
     }
      protected virtual void Move(Vector3 direction,float speed)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + direction * speed, 0.1f);
    }
}
