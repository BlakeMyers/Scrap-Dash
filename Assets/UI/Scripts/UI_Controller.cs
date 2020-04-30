using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HudPanel;
    public GameObject PausePanel;
    public Text TimeText;
    public Text healthText;
    public Text GoalText;
    public Text ammoText;
    bool pausetime = false;
    public float time = 600.0f;
    public GameObject Player;
    float playerHealth;
    float playerReserveAmmo;
    float playerAmmoInGun;
    float playerScrap;
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Pause();
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

        TimeText.text = "Time Remaining: " + time.ToString("0.0");
        ammoText.text = "Ammo: " + playerAmmoInGun.ToString() + "/" + playerReserveAmmo.ToString();
        GoalText.text = "Scrap: " + playerScrap.ToString();
        healthText.text = "Health: " + playerHealth.ToString();
    }

    private void Pause()
    {
        if (!pausetime) {
            pausetime = true;
            PausePanel.SetActive(true);
        }
    }

    public void Resume() {
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
}
