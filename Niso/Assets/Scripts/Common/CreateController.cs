using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateController : SingletonMono<CreateController>
{
    [SerializeField]
    GameObject bleedPrefab;
    [SerializeField]
    GameObject lightingPrefab;
    [SerializeField]
    GameObject hitPrefab;
    [SerializeField]
    GameObject detectPrefab;
    [SerializeField]
    GameObject bloomPrefab;
    [SerializeField]
    GameObject shurikenPrefab;
    [SerializeField]
    GameObject fireEffect;
   
    public void CreateBleed(Vector3 pos)
    {
        Instantiate(bleedPrefab,pos,Quaternion.identity);
    }
    public void CreateShuriken(Vector3 pos)
    {
        Instantiate(shurikenPrefab,pos,Quaternion.identity);
    }
    public void CreateLighting(Vector3 pos)
    {
        Instantiate(lightingPrefab,pos,Quaternion.identity);
    }
    public void CreateHit(Vector3 pos)
    {
        Instantiate(hitPrefab,pos,Quaternion.identity);
    }
    public GameObject CreateDetect(Vector3 pos)
    {
       return Instantiate(detectPrefab,pos,Quaternion.identity);
    }
    public void CreateBloom(Vector3 pos)
    {
        Instantiate(bloomPrefab,pos,Quaternion.identity);
    }
    public GameObject FireEffect(Vector3 pos)
    {
        return Instantiate(fireEffect,pos,Quaternion.Euler(-90,0,0));
    }
}
