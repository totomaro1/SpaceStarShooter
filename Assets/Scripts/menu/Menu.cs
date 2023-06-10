using UnityEngine;
using System.Collections;
using Totomaro.Util;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Totomaro.Board;

public class Menu : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;


    void Start()
    {

    }

    public void ArcadePlay()
    {
        MonsterGenerator.limitMode = false;
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void LimitPlay()
    {
        MonsterGenerator.limitMode = true;
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void SetMasterVolume(float value)
    {
       
    }
}