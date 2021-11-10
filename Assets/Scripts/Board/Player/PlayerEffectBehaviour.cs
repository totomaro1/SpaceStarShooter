using System.Collections;
using System.Collections.Generic;
using Ninez.Board;
using Ninez.Core;
using Ninez.Stage;
using Ninez.Util;

using UnityEngine;
using UnityEngine.UI;

namespace Ninez.Board
{
    public class PlayerEffectBehaviour : MonoBehaviour
    {
        GameObject monster;

        SpriteRenderer m_SpriteRenderer;
        ActionManager m_ActionManager;

        BlockPos m_BlockPos;
        ButtonManager m_Button;

        float randommove;
        float moveDirection;

        float span = 3f;
        float delta = 0;

        public bool inputLeft = false;
        public bool inputRight = false;
        public bool inputUp = false;
        public bool inputDown = false;
        public bool inputKick = false;
        public bool inputMove = false;
        public bool inputSkillFire = false;
        public bool inputSkillIce = false;
        public bool inputSkillGrass = false;
        public bool inputSkillLight = false;
        public bool inputSkillDark = false;

        public static bool playerDamaged = false;

        public int direction = 0; // 캐릭터의 방향 left = 2, right = 0, up = 1, down = 3
        public int PrevDirection = 0;


        //PlayerMove 관련
        public bool isMoving { get; set; }

        Vector3 movement;
        Queue<Vector3> m_MovementQueue = new Queue<Vector3>();

        public int playerCol;
        public int playerRow;

        //Player 깜빡임 관련

        public bool isBlink { get; set; }
        Vector3 currentTransformScale;

        void Start()
        {
            ButtonManager ui = GameObject.FindGameObjectWithTag("ManagerUi").GetComponent<ButtonManager>();
            ui.init();
            
        }

        void Update()
        {
            if (inputLeft)
            {
                transform.localScale = new Vector3(1, 1, 1);
                currentTransformScale = gameObject.transform.localScale;
                inputLeft = false;
                PrevDirection = direction;

                if (direction == Constants.PLAYERMOVE_LEFT)
                {
                    int nCol = PlayerGenerator.playerx + 4;
                    int nRow = PlayerGenerator.playery + 4;
                    //x축 , 맵 밖으로 나가는것 막기
                    if (0 < nCol)
                    {
                        PlayerMove(new Vector3(-1, 0, 1));
                    }
                    else this.delta = this.span;
                }
            }
            else if (inputRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                inputRight = false;
                PrevDirection = direction;

                if (direction == Constants.PLAYERMOVE_RIGHT)
                {
                    int nCol = PlayerGenerator.playerx + 4;
                    int nRow = PlayerGenerator.playery + 4;
                    //x축 , 맵 밖으로 나가는것 막기
                    if (nCol < 9)
                    {
                        //Debug.Log($"----- {nCol} -> {nCol - 1}, {nRow} -> {nRow}");

                        PlayerMove(new Vector3(1, 0, 1));
                    }
                    else this.delta = this.span;
                }
            }
            else if (inputUp)
            {
                inputUp = false;
                PrevDirection = direction;

                if (direction == Constants.PLAYERMOVE_UP)
                {
                    int nCol = PlayerGenerator.playerx + 4;
                    int nRow = PlayerGenerator.playery + 4;
                    //x축 , 맵 밖으로 나가는것 막기
                    if (nRow < 9)
                    {
                        //Debug.Log($"----- {nCol} -> {nCol}, {nRow} -> {nRow + 1}");

                        PlayerMove(new Vector3(0, 1, 1));
                    }
                    else this.delta = this.span;
                }
            }
            else if (inputDown)
            {
                inputDown = false;
                PrevDirection = direction;

                SoundManager.instance.PlayOneShot(Clip.CharacterButton);

                if (direction == Constants.PLAYERMOVE_DOWN)
                {
                    int nCol = PlayerGenerator.playerx + 4;
                    int nRow = PlayerGenerator.playery + 4;
                    //x축 , 맵 밖으로 나가는것 막기
                    if (0 < nRow)
                    {
                        //Debug.Log($"----- {nCol} -> {nCol}, {nRow} -> {nRow - 1}");

                        PlayerMove(new Vector3(0, -1, 1));
                    }
                    else this.delta = this.span;
                }
            }

        }

        public void PlayerMove(Vector2 vtMoveDistance)
        {
            m_MovementQueue.Enqueue(new Vector3(vtMoveDistance.x, vtMoveDistance.y, 1));
            

            if (!isMoving)
            {
                StartCoroutine(DoActionPlayerMove());
            }
        }

        IEnumerator DoActionPlayerMove(float acc = 1.0f)
        {
            isMoving = true;

            while (m_MovementQueue.Count > 0)
            {
                Vector2 vtDestination = m_MovementQueue.Dequeue();
                float duration = 1;
                yield return CoStartMoveSmooth(vtDestination, duration * acc);
            }

            isMoving = false;

            yield break;
        }
        IEnumerator CoStartMoveSmooth(Vector2 vtMoveDistance ,float duration)
        {
            Vector2 to = new Vector3(transform.position.x + vtMoveDistance.x , transform.position.y + vtMoveDistance.y);
            
            yield return Util.Action2D.MoveTo(transform, to, duration/4);
        }
    }
}

