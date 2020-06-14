using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        Vector3 playerPos = new Vector3(PlayerController.Instance.transform.position.x,
        PlayerController.Instance.transform.position.y + 2, -10);

        transform.position = Vector3.Lerp(transform.position, playerPos, 0.1f);
    }
}
