using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    int frameCount;
    void Start()
    {
        frameCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if (frameCount == 20)
        {
            Destroy(this.gameObject);
        }
    }
}
