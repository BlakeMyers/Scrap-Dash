using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public int armor = 0;
    public float speed = 10.0f;
    public float scrapCount = 0f;
    public float score = 0f;
    public int upgradePoints = 0;
    public float totalAmmo = 30f;
    public float ammoInGun = 5f;
    public float reserveAmmo = 0f;

    public GameObject scrapDrop;
    void Start()
    {
        totalAmmo = 30f;
        //currentHealth = 100f;
        maxHealth = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<ShepardController>().isWeaponEquiped)
        {
            ammoInGun = GetComponentInChildren<GunController>().ammoInGun;
            reserveAmmo = GetComponentInChildren<GunController>().reserveAmmo;
        }

        if(currentHealth <= 0)
        {
            //Respawn(pos);       
        }
    }

    public void CalculateUpgradePoints()
    {
        upgradePoints = Mathf.FloorToInt(scrapCount / 10);
    }
    public void UpdateScore()
    {
        score += Mathf.FloorToInt(scrapCount / 2);
    }
    
    public void IncreaseHealth()
    {
        maxHealth += maxHealth * 0.10f;
        upgradePoints--;
    }
    public void IncreaseSpeed()
    {
        speed += speed * 0.15f;
        upgradePoints--;
    }
    public void MakeArmor()
    {
        if(scrapCount >= 20&&armor<100)
        {
            armor += 10;
            scrapCount -= 20;
            if(armor > 100)
            {
                armor = 100;
            }
        }
    }

    public void UpgradeWeapon()
    {
        GunController playerGun = GetComponentInChildren<GunController>();
        playerGun.ammoCapacity += Mathf.Round(playerGun.ammoCapacity * 0.20f);
        playerGun.damage += Mathf.Round(playerGun.damage * 0.15f);
    } 
    void PickupHealth()
    {
        currentHealth += 25f;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    void PickupAmmo()
    {
        totalAmmo += 25f;
        GetComponentInChildren<GunController>().totalAmmo = totalAmmo;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Scrap")
        {
            scrapCount += 10;
            Destroy(other.gameObject);
        }
        if (other.tag == "SmallScrap")
        {
            scrapCount += 5;
            Destroy(other.gameObject);
        }
        if (other.tag == "LargeScrap")
        {
            scrapCount += 15;
            Destroy(other.gameObject);
        }
        if (other.tag == "Health")
        {
            PickupHealth();
            Destroy(other.gameObject);
        }
        if (other.tag == "Ammo")
        {
            PickupAmmo();
            Destroy(other.gameObject);
        }
    }
    public void DropScrap(Vector3 pos)
    {
        GameObject droppedScrap = (GameObject)Instantiate(scrapDrop, pos, Quaternion.identity);
        scrapCount -= 15;
        if (scrapCount <= 0)
        {
            scrapCount = 0;
        }
        Debug.Log("scrap dropped");
    }

    public void TakeDamage(float damage)
    {
        if (armor > 0)
        {
            armor -= (int)damage;
            if (armor < 0)
            {
                armor = 0;
            }
        }
        else
        {
            currentHealth -= damage;
        }
    }

    public void Respawn(Vector3 pos)
    {
        this.gameObject.transform.position = new Vector3(Random.Range(0f,300f), 5f, Random.Range(0f, 300f));
        DropScrap(pos);
        //Invoke("DropScrap(pos)", 0.1f);
        currentHealth = maxHealth;
    }
}
