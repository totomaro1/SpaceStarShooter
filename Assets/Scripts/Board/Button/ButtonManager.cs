using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Totomaro.Stage;
using Totomaro.Core;
using UnityEngine.UI;

namespace Totomaro.Board
{
    public class ButtonManager : MonoBehaviour
    {
        GameObject player;
        GameObject stat;

        PlayerBehaviour playerMove;
        PlayerEffectBehaviour playerEffectScript;
        StageController playerKick;
        //TouchEvaluator playerCo;

        //kick관련
        public static bool callKickCommand = false;

        public GameObject kickButton1;
        public GameObject kickButton2;

        public GameObject pauseUI;

        //정보를 보낼 주소
        public void init()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerMove = player.GetComponent<PlayerBehaviour>();
            //player = GameObject.FindGameObjectWithTag("PlayerEffect");
            //playerEffectScript = player.GetComponent<PlayerEffectBehaviour>();
            stat = GameObject.Find("Board");
            playerKick = stat.GetComponent<StageController>();
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.A)) //Left
            {
                if (callKickCommand)
                {
                    playerKick.inputKick = true;
                    playerKick.direction = 2;
                }
                else
                {
                    playerMove.inputLeft = true; //키 땠을 때
                    playerMove.direction = 2;
                }
            }

            if (Input.GetKeyDown(KeyCode.D)) //Right
            {

                if (callKickCommand)
                {
                    playerKick.inputKick = true;
                    playerKick.direction = 0;
                }
                else
                {
                    playerMove.inputRight = true;
                    playerMove.direction = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.W)) //위
            {
                if (callKickCommand)
                {
                    playerKick.inputKick = true;
                    playerKick.direction = 1;
                }
                else
                {
                    playerMove.inputUp = true;
                    playerMove.direction = 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.S)) //아래
            {
                if (callKickCommand)
                {
                    playerKick.inputKick = true;
                    playerKick.direction = Constants.DIRECTION_DOWN;
                }
                else
                {
                    playerMove.inputDown = true;
                    playerMove.direction = Constants.DIRECTION_DOWN;
                }
            }
            //
            if (Input.GetButtonDown("Fire1") && !ActionManager.isSwipeRunning && Time.timeScale != 0)
            {
                Invoke("WaitForBulletChange", 0.01f);

                if (player)
                {
                    Vector3 inputMouse = Input.mousePosition;

                    float playerToMouseX = Input.mousePosition.x - (960 + player.transform.position.x * 120);
                    float playerToMouseY = Input.mousePosition.y - (540 + player.transform.position.y * 120);

                    if (playerToMouseY < 0 && Mathf.Abs(playerToMouseY) - Mathf.Abs(playerToMouseX) > 0)
                    {
                        playerKick.inputKick = true;
                        playerKick.direction = Constants.DIRECTION_DOWN;
                        playerMove.kickDirection = Constants.DIRECTION_DOWN;
                    }

                    if (playerToMouseY > 0 && Mathf.Abs(playerToMouseY) - Mathf.Abs(playerToMouseX) > 0)
                    {
                        playerKick.inputKick = true;
                        playerKick.direction = Constants.DIRECTION_UP;
                        playerMove.kickDirection = Constants.DIRECTION_UP;
                    }

                    if (playerToMouseX < 0 && Mathf.Abs(playerToMouseX) - Mathf.Abs(playerToMouseY) > 0)
                    {
                        playerKick.inputKick = true;
                        playerKick.direction = Constants.DIRECTION_LEFT;
                        playerMove.kickDirection = Constants.DIRECTION_LEFT;
                    }

                    if (playerToMouseX > 0 && Mathf.Abs(playerToMouseX) - Mathf.Abs(playerToMouseY) > 0)
                    {
                        playerKick.inputKick = true;
                        playerKick.direction = Constants.DIRECTION_RIGHT;
                        playerMove.kickDirection = Constants.DIRECTION_RIGHT;
                    }

                    playerMove.playerBulletLimit = 1;
                }
            }

            if (Input.GetButtonDown("Fire2") && Time.timeScale != 0 && player)
            {
                playerMove.inputSkill = true;

                Vector3 inputMouse = Input.mousePosition;

                float playerToMouseX = Input.mousePosition.x - (960 + player.transform.position.x * 120);
                float playerToMouseY = Input.mousePosition.y - (540 + player.transform.position.y * 120);

                if (playerToMouseY < 0 && Mathf.Abs(playerToMouseY) - Mathf.Abs(playerToMouseX) > 0)
                {
                    playerMove.skillDirection = Constants.DIRECTION_DOWN;
                }

                if (playerToMouseY > 0 && Mathf.Abs(playerToMouseY) - Mathf.Abs(playerToMouseX) > 0)
                {
                    playerMove.skillDirection = Constants.DIRECTION_UP;
                }

                if (playerToMouseX < 0 && Mathf.Abs(playerToMouseX) - Mathf.Abs(playerToMouseY) > 0)
                {
                    playerMove.skillDirection = Constants.DIRECTION_LEFT;
                }

                if (playerToMouseX > 0 && Mathf.Abs(playerToMouseX) - Mathf.Abs(playerToMouseY) > 0)
                {
                    playerMove.skillDirection = Constants.DIRECTION_RIGHT;
                }

                playerMove.playerBulletLimit = 1;
            }


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseUI.SetActive(true);

                Time.timeScale = 0;
            }
        }

        public void Resume()
        {
            pauseUI.SetActive(false);

            Time.timeScale = 1;

        }

        void WaitForBulletChange()
        {
            playerMove.inputKick = true;
        }

        void WaitForSkillChange()
        {
            playerMove.inputSkill = true;
        }
    }
}