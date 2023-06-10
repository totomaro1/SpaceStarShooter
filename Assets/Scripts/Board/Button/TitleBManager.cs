using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Totomaro.Util;
using UnityEngine.SceneManagement;

namespace Totomaro.Board
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
