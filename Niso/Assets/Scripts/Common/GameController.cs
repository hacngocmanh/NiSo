using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TOPPICNAME
{
    public const string PLAYERDIE = "PLAYERDIE";
}
public class GameController : MonoBehaviour
{
   
    CameraController cameraScript;
    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    GameObject panel;
    private void Awake()
    {
        Observer.Instance.AddObserver(TOPPICNAME.PLAYERDIE, GameOver);
        
    }
    void Start()
    {
        panel.SetActive(false);
        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraController>();
        PlayerController.Instance.transform.position = spawnPoint.transform.position;
    }
    void Update()
    {

    }
   public void Replay()
    {
        SceneManager.LoadScene("SampleScene");
    }
    void GameOver(object obj)
    {
        panel.SetActive(true);
        cameraScript.enabled = false;
    }

    private void OnDestroy() {
        Observer.Instance.RemoveObserver(TOPPICNAME.PLAYERDIE,GameOver);
    }
}
