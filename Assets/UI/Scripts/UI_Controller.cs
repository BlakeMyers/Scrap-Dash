using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HudPanel;
    public GameObject PausePanel;
    public GameObject UpgradePanel;
    public Text TimeText;
    public Text healthText;
    public Text GoalText;
    public Text ammoText;
    public Text UpgradeText;
    bool pausetime = false;
    public float time = 600.0f;
    public GameObject Player;
    float playerHealth;
    float playerReserveAmmo;
    float playerAmmoInGun;
    float playerScrap;
    int UpgradePoints;
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if(!PausePanel.activeSelf)
             UpgradePanel.SetActive(true);
        }
        if (Input.GetKeyDown("escape"))
        {
            if (!UpgradePanel.activeSelf)
                Pause();
            else {
                UpgradePanel.SetActive(false);
            }
        }
        if (!pausetime) {
            time -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        playerHealth = Player.GetComponent<PlayerStats>().currentHealth;
        playerReserveAmmo = Player.GetComponent<PlayerStats>().reserveAmmo;
        playerAmmoInGun = Player.GetComponent<PlayerStats>().ammoInGun;
        playerScrap = Player.GetComponent<PlayerStats>().scrapCount;
        Player.GetComponent<PlayerStats>().CalculateUpgradePoints();
        UpgradePoints = Player.GetComponent<PlayerStats>().upgradePoints;

        TimeText.text = "Time Remaining: " + time.ToString("0.0");
        ammoText.text = "Ammo: " + playerAmmoInGun.ToString() + "/" + playerReserveAmmo.ToString();
        GoalText.text = "Scrap: " + playerScrap.ToString();
        healthText.text = "Health: " + playerHealth.ToString();
        UpgradeText.text = "Upgrade Points: " + UpgradePoints.ToString();

        if (playerHealth <= 0) {
            Player.GetComponent<PlayerStats>().Respawn(Player.transform.position);
        }
    }

    private void Pause()
    {
        if (!pausetime) {
            pausetime = true;
            PausePanel.SetActive(true);
        }
    }

    public void UpgradeGun() {
        if (UpgradePoints > 0) {
            Player.GetComponent<PlayerStats>().UpgradeWeapon();
            Player.GetComponent<PlayerStats>().scrapCount = Player.GetComponent<PlayerStats>().scrapCount - 10;
        }
    }
    public void UpgradeHealth() {
        if (UpgradePoints > 0)
        {
            Player.GetComponent<PlayerStats>().IncreaseHealth();
            Player.GetComponent<PlayerStats>().scrapCount = Player.GetComponent<PlayerStats>().scrapCount - 10;
        }
    }
    public void UpgradeSpeed()
    {
        if (UpgradePoints > 0)
        {
            Player.GetComponent<PlayerStats>().IncreaseSpeed();
            Player.GetComponent<PlayerStats>().scrapCount = Player.GetComponent<PlayerStats>().scrapCount - 10;
        }
    }
    public void UpgradeArmor() {

            Player.GetComponent<PlayerStats>().MakeArmor();

    }
    public void Resume() {
        UpgradePanel.SetActive(false);
        PausePanel.SetActive(false);
        pausetime = false;
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = (false);
#else
        Application.Quit();
#endif   
    }
    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
