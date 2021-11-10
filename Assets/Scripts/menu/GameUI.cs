using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Ninez.Board;

namespace Ninez.Stage
{
    public class GameUI : MonoBehaviour
    {
        public Image fadePlane;
        public GameObject gameOverUI;

        bool limitModeFlag = true;

        void Start()
        {
            //캐릭터 사망 판별 필요
            limitModeFlag = true;
        }

        void Update()
        {
            if (LifeControl.lifeGauge <= 0)
            {
                OnGameOver();
            }

            if (MonsterGenerator.currentMonster >= 20 && MonsterGenerator.limitMode == true && limitModeFlag == true)
            {
                LifeControl.lifeGauge = 0;
                limitModeFlag = false;
            }
        }

        public void OnGameOver()
        {
            StartCoroutine(Fade(Color.clear, Color.black, 1));
            gameOverUI.SetActive(true);
        }

        IEnumerator Fade(Color from, Color to, float time)
        {
            float speed = 1 / time;
            float percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime * speed;
                fadePlane.color = Color.Lerp(from, to, percent);
                yield return null;
            }
        }
        public void OverMainMenu()
        {
            LifeControl.lifeGauge = 6;
            StageController.p_col = 4;
            StageController.p_row = 8;
            PlayerGenerator.playerx = 0;
            PlayerGenerator.playery = 4;
            MonsterGenerator.currentMonster = 0;
            MonsterGenerator.countMonster = 0;
            PlayerBehaviour.playerDamaged = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("TitleScene");
        }

        public void OverQuit()
        {
            Application.Quit();
        }
    }
}