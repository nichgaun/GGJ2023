using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    private void Start()
    {
        volumeSlider.value = DataManager.Volume;
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnSliderChange(float value)
    {
        DataManager.Volume = value;
    }
}
