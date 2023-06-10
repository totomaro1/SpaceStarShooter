using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Totomaro.Stage;
using Totomaro.Core;
using Totomaro.Util;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Totomaro.Board
{
    public class LevelBManager : MonoBehaviour
    {
        GameObject select;

        //SelectManager levelManager;

        public Image easyImage;
        public Image normalImage;
        public Image hardImage;
        public Image hotImage;

        public static int curLevelSelect = 0;

        void Start()
        {
            curLevelSelect = Constants.LEVEL_EASY;
        }

        public void LevelClickEasy()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            curLevelSelect = Constants.LEVEL_EASY;
            MonsterGenerator.spawnSpan = 15f;
            MonsterGenerator.regenTime = 0.8f;
            MonsterBehaviour.span = 5f;
            easyImage.fillAmount = 1;
            normalImage.fillAmount = 0;
            hardImage.fillAmount = 0;
            hotImage.fillAmount = 0;
        }

        public void LevelClickNormal()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            curLevelSelect = Constants.LEVEL_NORMAL;
            MonsterGenerator.spawnSpan = 10f;
            MonsterGenerator.regenTime = 0.6f;
            MonsterBehaviour.span = 3f;
            easyImage.fillAmount = 0;
            normalImage.fillAmount = 1;
            hardImage.fillAmount = 0;
            hotImage.fillAmount = 0;
        }

        public void LevelClickHard()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            curLevelSelect = Constants.LEVEL_HARD;
            MonsterGenerator.spawnSpan = 6f;
            MonsterGenerator.regenTime = 0.4f;
            MonsterBehaviour.span = 2f;
            easyImage.fillAmount = 0;
            normalImage.fillAmount = 0;
            hardImage.fillAmount = 1;
            hotImage.fillAmount = 0;
        }

        public void LevelClickHot()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            curLevelSelect = Constants.LEVEL_HOT;
            MonsterGenerator.spawnSpan = 3f;
            MonsterGenerator.regenTime = 0.2f;
            MonsterBehaviour.span = 1f;
            easyImage.fillAmount = 0;
            normalImage.fillAmount = 0;
            hardImage.fillAmount = 0;
            hotImage.fillAmount = 1;
        }

        public void GoButton()
        {
            LoadingSceneManager.LoadScene("PlayScene");
        }

        public void BackButton()
        {
            SceneManager.LoadScene("CharacterSelectScene");
        }
    }
}