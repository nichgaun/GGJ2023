using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerLevel : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; //set in editor
    [SerializeField] Slider volumeSlider; //set in editor

    private void Start()
    {
        volumeSlider.value = DataManager.Volume;
    }

    public void OnClickPause()
    {
        pauseMenu.SetActive(true);
    }

    public void OnClickResume()
    {
        pauseMenu.SetActive(false);
    }

    public void OnSliderChange(float value)
    {
        DataManager.Volume = value;
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
