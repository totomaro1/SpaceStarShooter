using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Totomaro.Util;
using UnityEngine.SceneManagement;

namespace Totomaro.Board
{
    public class TitleButtonBehaviour : MonoBehaviour
    {
        bool isBlink = false;

        void Start()
        {

        }

        void Update()
        {
            StartCoroutine(ButtonBlink());
        }

        public IEnumerator ButtonBlink()
        {
            if (!isBlink)
            {
                isBlink = true;

                WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

                gameObject.transform.localScale = new Vector3(0, 0, 0);
                yield return waitForSeconds;
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                yield return waitForSeconds;

                isBlink = false;
            }
        }
    }
}
