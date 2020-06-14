using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public GameObject timeLine;
    GameObject game;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //PlayerController.Instance.GetComponent<PlayerController>().enabled = false;
        timeLine.SetActive(true);
        StartCoroutine(TimeLine());
    }
    IEnumerator TimeLine()
    {
        yield return new WaitForSeconds(5);
        this.gameObject.SetActive(false);
        timeLine.SetActive(false);
       // PlayerController.Instance.GetComponent<PlayerController>().enabled = true;
    }

}
