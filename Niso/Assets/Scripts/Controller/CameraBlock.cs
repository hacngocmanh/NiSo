using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlock : MonoBehaviour
{
    [SerializeField] Transform blockLeft;
    [SerializeField] Transform blockRight;
    [SerializeField] Transform blockTop;
    [SerializeField] Transform blockDown;

    Vector3 posPlayer;

    void Start()
    {
    }

    void Update()
    {

        if (PlayerController.Instance.transform.position.x < blockLeft.position.x) return;
        else if (PlayerController.Instance.transform.position.x > blockRight.position.x) return;
        else if (PlayerController.Instance.transform.position.y < blockDown.position.y) return;
        else if (PlayerController.Instance.transform.position.y > blockTop.position.y) return;
        else
        {
            posPlayer = new Vector3(PlayerController.Instance.transform.position.x,
                   PlayerController.Instance.transform.position.y + 5, -10);
            transform.position = Vector3.Lerp(transform.position, posPlayer, .1F);
        }

    }
}
