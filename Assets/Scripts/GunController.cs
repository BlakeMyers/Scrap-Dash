using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviourPunCallbacks
{
    public float muzzleVelocity = 1200f;
    public float damage = 5f;
    public float ammoCapacity = 5f;
    public float ammoInGun = 5f;
    public float totalAmmo = 20f;
    public float reserveAmmo = 0f;
    public GameObject bullet;
    public PlayerStats playerStats;
    public bool isEquipped = false;
    void Start()
    {
        ammoCapacity = 5f;
    }

    void Update()
    {
        reserveAmmo = totalAmmo - ammoInGun;
        if (reserveAmmo < 0)
            reserveAmmo = 0;
        if (isEquipped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (ammoInGun > 0)
                {
                    GunFire();
                    ammoInGun--;
                    totalAmmo--;
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
        playerStats = this.transform.root.gameObject.GetComponent<PlayerStats>();
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
              //  totalAmmo -= ammoCapacity;
            }
            else if (totalAmmo > 0)
            {
                ammoInGun += totalAmmo;
               // totalAmmo -= totalAmmo;
            }
        }
        else if(ammoInGun < ammoCapacity)
        {
            float difference = ammoCapacity - ammoInGun;
            if (difference <= reserveAmmo)
                ammoInGun += difference;
            else {
                ammoInGun += reserveAmmo;
            }
            //totalAmmo -= difference;
        }
        playerStats.totalAmmo = totalAmmo;
    }
    public void GunFire()
    {
        GameObject bulletObj = PhotonNetwork.Instantiate(bullet.name, Vector3.zero, Quaternion.identity);
        bulletObj.transform.position = transform.GetChild(0).position;
        bulletObj.transform.rotation = transform.rotation;
        bulletObj.SetActive(true);
        Rigidbody bulletRB = bulletObj.GetComponent<Rigidbody>();
        bulletRB.AddForce(bulletObj.transform.forward * muzzleVelocity);
    }
}
