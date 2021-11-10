using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ninez.Stage;
using Ninez.Core;
using UnityEngine.UI;

namespace Ninez.Board
{
    public class ButtonManager : MonoBehaviour
    {
        GameObject player;
        GameObject stat;

        PlayerBehaviour playerScript;
        PlayerEffectBehaviour playerEffectScript;
        StageController playerSent;
        //TouchEvaluator playerCo;

        //kick����
        public static bool callKickCommand = false;

        public GameObject kickButton1;
        public GameObject kickButton2;

        //������ ���� �ּ�
        public void init()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerBehaviour>();
            //player = GameObject.FindGameObjectWithTag("PlayerEffect");
            //playerEffectScript = player.GetComponent<PlayerEffectBehaviour>();
            stat = GameObject.Find("Board");
            playerSent = stat.GetComponent<StageController>();
        }

        //�� ������ ���� ��ư ������ �ִ� ���� ���� �� ���������� �̵��� ������ �������� ĳ������ ���� ����

        public void LeftClick()
        {
            if (callKickCommand)
            {
                playerSent.inputKick = true;
                playerSent.direction = 2;
            }
            else
            {
                playerScript.inputLeft = true; //Ű ���� ��
                playerScript.direction = 2;
                //playerEffectScript.inputLeft = true;
                //playerEffectScript.direction = 2;
            }
        }

        public void RightClick()
        {
            if (callKickCommand)
            {
                playerSent.inputKick = true;
                playerSent.direction = 0;
            }
            else
            {
                playerScript.inputRight = true;
                playerScript.direction = 0;
                //playerEffectScript.inputRight = true;
                //playerEffectScript.direction = 0;


            }
        }

        public void UpClick()
        {
            if (callKickCommand)
            {
                playerSent.inputKick = true;
                playerSent.direction = 1;
            }
            else
            {
                playerScript.inputUp = true;
                playerScript.direction = 1;
                //playerEffectScript.inputUp = true;
                //playerEffectScript.direction = 1;
            }
        }

        public void DownClick()
        {
            if (callKickCommand)
            {
                playerSent.inputKick = true;
                playerSent.direction = 3;
            }
            else
            {
                playerScript.inputDown = true;
                playerScript.direction = 3;
                //playerEffectScript.inputDown = true;
                //playerEffectScript.direction = 3;
            }
        }

        public void KickClick()
        {
            callKickCommand = true;
            kickButton1.SetActive(false);
            kickButton2.SetActive(true);
        }

        public void Kick2Click()
        {
            callKickCommand = false;
            kickButton1.SetActive(true);
            kickButton2.SetActive(false);
        }

        public void SkillClickFire()
        {
            playerScript.inputSkillFire = true;
        }

        public void SkillClickIce()
        {
            playerScript.inputSkillIce = true;
        }

        public void SkillClickGrass()
        {
            playerScript.inputSkillGrass = true;
        }

        public void SkillClickLight()
        {
            playerScript.inputSkillLight = true;
        }

        public void SkillClickDark()
        {
            playerScript.inputSkillDark = true;
        }



    }
}