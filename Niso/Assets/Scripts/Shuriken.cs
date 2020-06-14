using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 lastPos;
    private void Start() {
        StartCoroutine( Destroy());
        Vector3 playerPos = PlayerController.Instance.transform.position;
        lastPos = playerPos;
    }
    void Update()
    {
        
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
