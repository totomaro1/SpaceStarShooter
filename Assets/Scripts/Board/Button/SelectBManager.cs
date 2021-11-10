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
    public class SelectBManager : MonoBehaviour
    {
        GameObject select;

        SelectManager selectManager;

        public static int CharacterSelect;

        public void init()
        {
            select = GameObject.Find("SelectManager");
            selectManager = select.GetComponent<SelectManager>();
            selectManager.basicCharacter = true;
            CharacterSelect = Constants.BASIC_CHARACTER;
        }

        public void ChracterClickFire()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            selectManager.fireCharacter = true;
            CharacterSelect = Constants.FIRE_CHARACTER;
        }

        public void ChracterClickIce()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            selectManager.iceCharacter = true;
            CharacterSelect = Constants.ICE_CHARACTER;
        }

        public void ChracterClickGrass()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            selectManager.grassCharacter = true;
            CharacterSelect = Constants.GRASS_CHARACTER;
        }

        public void ChracterClickLight()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            selectManager.lightCharacter = true;
            CharacterSelect = Constants.LIGHT_CHARACTER;
        }

        public void ChracterClickDark()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            selectManager.darkCharacter = true;
            CharacterSelect = Constants.DARK_CHARACTER;
        }

        public void CharacterClickBasic()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            selectManager.basicCharacter = true;
            CharacterSelect = Constants.BASIC_CHARACTER;
        }

        public void GoButton()
        {
            SceneManager.LoadScene("LevelSelectScene");
        }

        public void BackButton()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}