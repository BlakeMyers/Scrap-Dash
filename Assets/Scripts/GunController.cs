using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    float muzzleVelocity = 1200f;
    public GameObject bullet;
    static List<GameObject> bulletList = new List<GameObject>();
    public bool isEquipped = false;
    void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject bulletObj = (GameObject)Instantiate(bullet);
            bulletObj.SetActive(false);
            bulletList.Add(bulletObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEquipped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GunFire();
            }
        }
    }

    public void GunEquipped()
    {
        isEquipped = true;

    }
    public void GunDropped()
    {
        isEquipped = false;
 
    }
    public void GunFire()
    {
        for(int i = 0; i < bulletList.Count; i++)
        {
            if (!bulletList[i].activeInHierarchy)
            {
                bulletList[i].transform.position = transform.GetChild(0).position;
                bulletList[i].transform.rotation = transform.rotation;
                bulletList[i].SetActive(true);
                Rigidbody bulletRB = bulletList[i].GetComponent<Rigidbody>();
                bulletRB.AddForce(bulletList[i].transform.forward * muzzleVelocity);
                break;
            }
        }
    }
}
