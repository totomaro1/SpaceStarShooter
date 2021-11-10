using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ninez.Util;
using UnityEngine.SceneManagement;

namespace Ninez.Board
{
    public class TitleBManager : MonoBehaviour
    {

        void Start()
        {

        }

        public void GoButton()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
