using System.Collections;
using System.Collections.Generic;
using Ninez.Board;
using Ninez.Core;
using UnityEngine;
using Ninez.Util;

namespace Ninez.Board
{
    public class MonsterBehaviour : MonoBehaviour
    {
        GameObject player;

        SpriteRenderer m_SpriteRenderer;

        float randommove;
        float moveDirection;
        public static float span = 2f;
        float delta = 0;
        float deltaFreeze = 0;
        float deltaBind = 0;
        float deltaFear = 0;

        //MonsterMove 관련
        public bool isMoving { get; set; }
        Queue<Vector3> m_MovementQueue = new Queue<Vector3>();

        //애니메이션
        Animator animator;

        void Start()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            this.player = GameObject.FindGameObjectWithTag("Player");
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            this.delta += Time.deltaTime;

            MonsterCrowdControlTime();

            if (this.delta > span)
            {
                this.delta = 0;
                moveDirection = Random.Range(0, 2);
                randommove = (Random.Range(0, 2) == 0)? -1 : 1;
                //0일때 x축 이동, 1일때 y축 이동
                if (gameObject.GetComponent<Renderer>().material.color == Constants.MONSTER_FREEZE) randommove = 0;
                if (gameObject.GetComponent<Renderer>().material.color == Constants.MONSTER_BIND) randommove = 0;

                if (moveDirection == 0)
                {
                    int nCol = (int)(transform.position.x + 4f);
                    int nRow = (int)(transform.position.y + 4f);

                    //x축 , 맵 밖으로 나가는것 막기
                    if (0 <= nCol + (int)randommove && nCol + (int)randommove < 9)
                    {
                        //몬스터 겹치지 않게 하기
                        if (MonsterGenerator.monsters[nRow, nCol + (int)randommove] == null 
                            && MonsterGenerator.preMonsters[nRow, nCol + (int)randommove] == null)
                        {
                            MonsterGenerator.monsters[nRow, nCol + (int)randommove] = MonsterGenerator.monsters[nRow, nCol];
                            MonsterGenerator.monsters[nRow, nCol] = null;
                            MonsterMove(new Vector3(randommove, 0, 1));
                        }
                        else this.delta = span;
                    }
                    else this.delta = span;
                }
                else
                {
                    int nCol = (int)(transform.position.x + 4f);
                    int nRow = (int)(transform.position.y + 4f);
                    //y축 , 맵 밖으로 나가는것 막기
                    if (0 <= nRow + (int)randommove && nRow + (int)randommove < 9)
                    {
                        //몬스터 겹치지 않게 하기
                        if (MonsterGenerator.monsters[nRow + (int)randommove, nCol] == null
                            && MonsterGenerator.preMonsters[nRow + (int)randommove, nCol] == null)
                        {
                            MonsterGenerator.monsters[nRow + (int)randommove, nCol] = MonsterGenerator.monsters[nRow, nCol];
                            MonsterGenerator.monsters[nRow, nCol] = null;
                            MonsterMove(new Vector3(0, randommove, 1));
                        }
                        else this.delta = span;
                    }
                    else this.delta = span;
                }
            }

            //충돌 판정 추가 (공포 상태 중에는 반응 하지 않음)
            if (player && gameObject.GetComponent<Renderer>().material.color != Constants.MONSTER_FEAR)
            {
                Vector2 p1 = transform.position;
                Vector2 p2 = this.player.transform.position;
                Vector2 dir = p1 - p2;
                float d = dir.magnitude;
                float r1 = 0.1f;
                float r2 = 0.1f;

                if (d < r1 + r2)
                {
                    GameObject lifeControl = GameObject.Find("LifeControl");
                    lifeControl.GetComponent<LifeControl>().decreaseHp();

                    SoundManager.instance.PlayOneShot(Clip.PlayerDamage);

                    PlayerBehaviour.playerDamaged = true;
                    Destroy(gameObject);
                }
            }

            //애니메이션 추가
            if (player)
            {
                Vector2 p1 = transform.position;
                Vector2 p2 = this.player.transform.position;
                Vector2 dir = p1 - p2;
                float d = dir.magnitude;
                animator.SetFloat("distance", d);
            }
        }

        public void MonsterCrowdControlTime()
        {
            //상태이상 빙결
            if (gameObject.GetComponent<Renderer>().material.color == Constants.MONSTER_FREEZE)
            {
                float freezeTime = 10f;

                //얼음 캐릭터 특권
                if (SelectBManager.CharacterSelect == Constants.ICE_CHARACTER) freezeTime = 20f;

                if (this.deltaFreeze > freezeTime)
                {
                    gameObject.GetComponent<Renderer>().material.color = Constants.MONSTER_NORMAL;
                    this.deltaFreeze = 0;
                    //얼음 캐릭터 특권
                    if (SelectBManager.CharacterSelect == Constants.ICE_CHARACTER)
                    {
                        Destroy(gameObject);
                        Manager.instance.AddScore(100);
                    }
                        
                }
                else this.deltaFreeze += Time.deltaTime;
            }
            //상태이상 속박
            if (gameObject.GetComponent<Renderer>().material.color == Constants.MONSTER_BIND)
            {
                float bindTime = 10f;

                //풀 캐릭터 특권
                if(SelectBManager.CharacterSelect == Constants.GRASS_CHARACTER) bindTime = 30f;

                if (this.deltaBind > bindTime)
                {
                    gameObject.GetComponent<Renderer>().material.color = Constants.MONSTER_NORMAL;
                    this.deltaBind = 0;
                }
                else this.deltaBind += Time.deltaTime;
            }
            //상태이상 공포
            if (gameObject.GetComponent<Renderer>().material.color == Constants.MONSTER_FEAR)
            {
                float fearTime = 10f;

                //어둠 캐릭터 특권
                if (SelectBManager.CharacterSelect == Constants.DARK_CHARACTER) fearTime = 20f;

                if (this.deltaFear > fearTime)
                {
                    gameObject.GetComponent<Renderer>().material.color = Constants.MONSTER_NORMAL;
                    deltaFear = 0;
                }
                else this.deltaFear += Time.deltaTime;
            }
        }

        public void MonsterMove(Vector2 vtMoveDistance)
        {
            m_MovementQueue.Enqueue(new Vector3(vtMoveDistance.x, vtMoveDistance.y, 1));

            if (!isMoving)
            {
                StartCoroutine(DoActionMonsterMove());
            }
        }

        IEnumerator DoActionMonsterMove(float acc = 1.0f)
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
            yield return Util.Action2D.MoveTo(transform, to, duration);
        }
    }
}

