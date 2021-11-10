using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ninez.Util;
using UnityEngine.SceneManagement;

namespace Ninez.Board
{
    public class PauseBManager : MonoBehaviour
    {
        public GameObject pauseUI;

        GameObject pnButton_LEFT;
        GameObject pnButton_RIGHT;
        GameObject pnButton_UP;
        GameObject pnButton_DOWN;

        void Start()
        {
            pnButton_LEFT = GameObject.Find("Button_LEFT");
            pnButton_RIGHT = GameObject.Find("Button_RIGHT");
            pnButton_UP = GameObject.Find("Button_UP");
            pnButton_DOWN = GameObject.Find("Button_DOWN");
        }

        public void PauseButton()
        {
            pauseUI.SetActive(true);

            Time.timeScale = 0;

            pnButton_LEFT.SetActive(false);
            pnButton_RIGHT.SetActive(false);
            pnButton_UP.SetActive(false);
            pnButton_DOWN.SetActive(false);
        }

        public void Resume()
        {
            pauseUI.SetActive(false);

            Time.timeScale = 1;

            pnButton_LEFT.SetActive(true);
            pnButton_RIGHT.SetActive(true);
            pnButton_UP.SetActive(true);
            pnButton_DOWN.SetActive(true);
        }
    }
}
