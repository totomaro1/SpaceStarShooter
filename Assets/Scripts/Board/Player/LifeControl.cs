using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Totomaro.Board
{
    public class LifeControl : MonoBehaviour
    {
        GameObject life;

        public static int lifeGauge = 6;

        // Start is called before the first frame update
        void Start()
        {
            this.life = GameObject.Find("Life");
        }

        // Update is called once per frame
        void Update()
        {
            if (lifeGauge <= 0)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                this.life.GetComponent<Image>().fillAmount = 0;

                Destroy(player);
            }
        }

        public void decreaseHp()
        {
            this.life.GetComponent<Image>().fillAmount -= 1/6f;

            lifeGauge--;
        }

        public void increaseHp()
        {
            if (lifeGauge < 6)
            {
                this.life.GetComponent<Image>().fillAmount += 1/6f;
                lifeGauge++;
            }
        }
    }
}
