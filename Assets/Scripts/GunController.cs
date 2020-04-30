using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public float muzzleVelocity = 1200f;
    public float damage = 5f;
    public float ammoCapacity = 5f;
    public float ammoInGun = 5f;
    public float totalAmmo = 20f;
    public GameObject bullet;
    public PlayerStats playerStats;
    static List<GameObject> bulletList = new List<GameObject>();
    public bool isEquipped = false;
    void Start()
    {
        ammoCapacity = 5f;
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        for (int i = 0; i < 200; i++)
        {
            GameObject bulletObj = (GameObject)Instantiate(bullet);
            bulletObj.SetActive(false);
            bulletList.Add(bulletObj);
        }
    }

    void Update()
    {
        if (isEquipped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (ammoInGun > 0)
                {
                    GunFire();
                    ammoInGun--;
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GunReload();
            }
        }
    }

    public void GunEquipped()
    {
        isEquipped = true;
        totalAmmo = playerStats.totalAmmo;
    }
    public void GunDropped()
    {
        isEquipped = false;
 
    }
    public void GunReload()
    {
        if (ammoInGun == 0)
        {
            if (totalAmmo >= ammoCapacity)
            {
                ammoInGun += ammoCapacity;
                totalAmmo -= ammoCapacity;
            }
            else if (totalAmmo > 0)
            {
                ammoInGun += totalAmmo;
                totalAmmo -= totalAmmo;
            }
        }
        else if(ammoInGun < ammoCapacity)
        {
            float difference = ammoCapacity - ammoInGun;
            totalAmmo -= difference;
            ammoInGun += difference;
        }
        playerStats.totalAmmo = totalAmmo;
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
