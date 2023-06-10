using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Totomaro.Core;
using Totomaro.Stage;

namespace Totomaro.Board
{
    public class SkillPortionGenerator : MonoBehaviour
    {
        public GameObject skillPortionPrefab;

        public Sprite fireSprite;
        public Sprite iceSprite;
        public Sprite grassSprite;
        public Sprite lightSprite;
        public Sprite darkSprite;


        public Text skillInformation;

        GameObject skillPortion;
        GameObject buttonGauge;
        GameObject player;

        public static GameObject[] skillGameObject = new GameObject[3];

        PlayerBehaviour playerMove;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            buttonGauge = GameObject.Find("Button_Fire");
            player = GameObject.FindGameObjectWithTag("Player");

            if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
            {
                buttonGauge.GetComponent<Image>().fillAmount = 0f;

                SkillPortion(Constants.SKILLPORTION_FIRE);
            }

            buttonGauge = GameObject.Find("Button_Ice");
            if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
            {
                buttonGauge.GetComponent<Image>().fillAmount = 0f;
                SkillPortion(Constants.SKILLPORTION_ICE);
            }

            buttonGauge = GameObject.Find("Button_Grass");
            if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
            {
                buttonGauge.GetComponent<Image>().fillAmount = 0f;
                SkillPortion(Constants.SKILLPORTION_GRASS);
            }

            buttonGauge = GameObject.Find("Button_Light");
            if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
            {
                buttonGauge.GetComponent<Image>().fillAmount = 0f;
                SkillPortion(Constants.SKILLPORTION_LIGHT);
            }

            buttonGauge = GameObject.Find("Button_Dark");
            if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
            {
                buttonGauge.GetComponent<Image>().fillAmount = 0f;
                SkillPortion(Constants.SKILLPORTION_DARK);
            }
        }

        void SkillPortion(float kindSkill)
        {
            if(!skillGameObject[2])
            {
                skillPortion = Object.Instantiate(skillPortionPrefab, new Vector3(7.25f, 0, 0), Quaternion.identity);
                skillPortion.transform.localScale = new Vector3(0.7f, 0.7f, kindSkill);

                if (kindSkill == Constants.SKILLPORTION_FIRE) skillPortion.GetComponent<SpriteRenderer>().sprite = fireSprite;
                if (kindSkill == Constants.SKILLPORTION_ICE) skillPortion.GetComponent<SpriteRenderer>().sprite = iceSprite;
                if (kindSkill == Constants.SKILLPORTION_GRASS) skillPortion.GetComponent<SpriteRenderer>().sprite = grassSprite;
                if (kindSkill == Constants.SKILLPORTION_LIGHT) skillPortion.GetComponent<SpriteRenderer>().sprite = lightSprite;
                if (kindSkill == Constants.SKILLPORTION_DARK) skillPortion.GetComponent<SpriteRenderer>().sprite = darkSprite;

                skillGameObject[2] = skillPortion;

                SkillPushSort();
            }
        }

        public void SkillPushSort()
        {
            for (int i = 1; 0 <= i; i--)
            {
                if (!skillGameObject[i] && skillGameObject[i + 1])
                {
                    skillGameObject[i] = skillGameObject[i + 1];
                    skillGameObject[i + 1] = null;
                    skillGameObject[i].transform.position = new Vector3(i + 5.25f, 0, 0);
                }
            }

            InpormationSkillText();
        }

        public void SkillPullSort()
        {
            for (int i = 0; i <= 1; i++)
            {
                if (!skillGameObject[i] && skillGameObject[i + 1])
                {
                    skillGameObject[i] = skillGameObject[i + 1];
                    skillGameObject[i + 1] = null;
                    skillGameObject[i].transform.position = new Vector3(i + 5.25f, 0, 0);
                }
            }
            InpormationSkillText();
        }

        void InpormationSkillText()
        {
            if (skillGameObject[0])
            {
                if (skillGameObject[0].transform.localScale.z == Constants.SKILLPORTION_FIRE)
                {
                    skillInformation.text = "불속성 스킬\n\n해당 방향 부채꼴\n2 데미지";
                }

                if (skillGameObject[0].transform.localScale.z == Constants.SKILLPORTION_ICE)
                {
                    skillInformation.text = "얼음속성 스킬\n\n주위 몬스터에게\n1 데미지 + 빙결";
                }

                if (skillGameObject[0].transform.localScale.z == Constants.SKILLPORTION_GRASS)
                {
                    skillInformation.text = "풀속성 스킬\n\n해당 방향 직사각형\n1 데미지 + 속박";
                }

                if (skillGameObject[0].transform.localScale.z == Constants.SKILLPORTION_LIGHT)
                {
                    skillInformation.text = "빛속성 스킬\n\n플레이어의\n라이프 회복";
                }

                if (skillGameObject[0].transform.localScale.z == Constants.SKILLPORTION_DARK)
                {
                    skillInformation.text = "어둠속성 스킬\n\n모든 몬스터에게\n공포";
                }
            }
            else skillInformation.text = "";
        }
    }
}




/*
GameObject buttonGauge;
buttonGauge = GameObject.Find("Button_Fire");
if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
{
    buttonGauge.GetComponent<Image>().fillAmount = 0f;
    randomx = Random.Range(-4, 5);
    randomy = Random.Range(-4, 5);
    skillportion = Object.Instantiate(skillPortionPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
    skillportion.transform.localScale= new Vector3(1f, 1f, Constants.SKILLPORTION_FIRE);
    skillportion.GetComponent<Renderer>().material.color = Constants.COLOR_BURN;
}

buttonGauge = GameObject.Find("Button_Ice");
if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
{
    buttonGauge.GetComponent<Image>().fillAmount = 0f;
    randomx = Random.Range(-4, 5);
    randomy = Random.Range(-4, 5);
    skillportion = Object.Instantiate(skillPortionPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
    skillportion.transform.localScale = new Vector3(1f, 1f, Constants.SKILLPORTION_ICE);
    skillportion.GetComponent<Renderer>().material.color = Constants.COLOR_FREEZE;
}

buttonGauge = GameObject.Find("Button_Grass");
if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
{
    buttonGauge.GetComponent<Image>().fillAmount = 0f;
    randomx = Random.Range(-4, 5);
    randomy = Random.Range(-4, 5);
    skillportion = Object.Instantiate(skillPortionPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
    skillportion.transform.localScale = new Vector3(1f, 1f, Constants.SKILLPORTION_GRASS);
    skillportion.GetComponent<Renderer>().material.color = Constants.COLOR_BIND;
}

buttonGauge = GameObject.Find("Button_Light");
if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
{
    buttonGauge.GetComponent<Image>().fillAmount = 0f;
    randomx = Random.Range(-4, 5);
    randomy = Random.Range(-4, 5);
    skillportion = Object.Instantiate(skillPortionPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
    skillportion.transform.localScale = new Vector3(1f, 1f, Constants.SKILLPORTION_LIGHT);
    skillportion.GetComponent<Renderer>().material.color = Constants.COLOR_LIGHT;
}

buttonGauge = GameObject.Find("Button_Dark");
if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
{
    buttonGauge.GetComponent<Image>().fillAmount = 0f;
    randomx = Random.Range(-4, 5);
    randomy = Random.Range(-4, 5);
    skillportion = Object.Instantiate(skillPortionPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
    skillportion.transform.localScale = new Vector3(1f, 1f, Constants.SKILLPORTION_DARK);
    skillportion.GetComponent<Renderer>().material.color = Constants.COLOR_FEAR;
}
*/