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
    public Text GoalText;
    bool pausetime = false;
    float time = 0.0f;
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
            time += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {

        TimeText.text = "Time: \n" + time.ToString("0.0");
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
