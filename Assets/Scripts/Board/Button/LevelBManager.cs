using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ninez.Stage;
using Ninez.Core;
using Ninez.Util;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Ninez.Board
{
    public class LevelBManager : MonoBehaviour
    {
        GameObject select;

        SelectManager levelManager;

        public Image easyImage;
        public Image normalImage;
        public Image hardImage;
        public Image hotImage;

        void Start()
        {
            MonsterGenerator.spawnSpan = 15f;
            MonsterGenerator.regenTime = 0.8f;
            MonsterGenerator.randomSpawn = 10;
            MonsterBehaviour.span = 5f;
        }

        public void LevelClickEasy()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            MonsterGenerator.spawnSpan = 15f;
            MonsterGenerator.regenTime = 0.8f;
            MonsterGenerator.randomSpawn = 25;
            MonsterBehaviour.span = 5f;
            easyImage.fillAmount = 1;
            normalImage.fillAmount = 0;
            hardImage.fillAmount = 0;
            hotImage.fillAmount = 0;
        }

        public void LevelClickNormal()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            MonsterGenerator.spawnSpan = 10f;
            MonsterGenerator.regenTime = 0.6f;
            MonsterGenerator.randomSpawn = 20;
            MonsterBehaviour.span = 4f;
            easyImage.fillAmount = 0;
            normalImage.fillAmount = 1;
            hardImage.fillAmount = 0;
            hotImage.fillAmount = 0;
        }

        public void LevelClickHard()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            MonsterGenerator.spawnSpan = 6f;
            MonsterGenerator.regenTime = 0.4f;
            MonsterGenerator.randomSpawn = 15;
            MonsterBehaviour.span = 3f;
            easyImage.fillAmount = 0;
            normalImage.fillAmount = 0;
            hardImage.fillAmount = 1;
            hotImage.fillAmount = 0;
        }

        public void LevelClickHot()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            MonsterGenerator.spawnSpan = 3f;
            MonsterGenerator.regenTime = 0.2f;
            MonsterGenerator.randomSpawn = 10;
            MonsterBehaviour.span = 2f;
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