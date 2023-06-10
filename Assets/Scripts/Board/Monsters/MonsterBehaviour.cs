using System.Collections;
using System.Collections.Generic;
using Totomaro.Core;
using Totomaro.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Totomaro.Board
{
    public class MonsterBehaviour : MonoBehaviour
    {
        //몬스터 기본 데이터베이스
        public int monsterLife = 2;
        public int monsterType = Constants.BASIC_MONSTER;
        public int monsterTypeCurrent = Constants.BASIC_MONSTER;

        GameObject player;

        SpriteRenderer m_SpriteRenderer;

        float randommove;
        float moveDirection;
        public static float span = 2f;
        float delta = 0;
        float deltaFreeze = 0;
        float deltaBind = 0;
        float deltaFear = 0;

        int curMoveX = 0;
        int curMoveY = 0;

        //MonsterMove 관련
        public bool isMoving { get; set; }
        Queue<Vector3> m_MovementQueue = new Queue<Vector3>();

        //애니메이션
        Animator animator;

        //bullet
        bool isBulletDamaged = false;

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

            //randomMove
            if (this.delta > span)
            {
                this.delta = 0;
                moveDirection = Random.Range(0, 2);
                randommove = (Random.Range(0, 2) == 0)? -1 : 1;
                //0일때 x축 이동, 1일때 y축 이동
                if (gameObject.GetComponent<Renderer>().material.color == Constants.COLOR_FREEZE) randommove = 0;
                if (gameObject.GetComponent<Renderer>().material.color == Constants.COLOR_BIND) randommove = 0;

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
                            if (randommove == -1)
                            {
                                if (transform.localScale.x > 0) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                                if (transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                            }
                            if (randommove == 1)
                            {
                                if (transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                                if (transform.localScale.x < 0) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                            }
                            curMoveX = nRow;
                            curMoveY = nCol + (int)randommove;
                            MonsterGenerator.monsters[nRow, nCol + (int)randommove] = gameObject;
                            //MonsterGenerator.monsters[nRow, nCol + (int)randommove] = MonsterGenerator.monsters[nRow, nCol];
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
                            curMoveX = nRow + (int)randommove;
                            curMoveY = nCol;
                            MonsterGenerator.monsters[nRow + (int)randommove, nCol] = gameObject;
                            //MonsterGenerator.monsters[nRow + (int)randommove, nCol] = MonsterGenerator.monsters[nRow, nCol];
                            MonsterGenerator.monsters[nRow, nCol] = null;
                            MonsterMove(new Vector3(0, randommove, 1));
                        }
                        else this.delta = span;
                    }
                    else this.delta = span;
                }
            }

            //충돌 판정 추가 (공포 상태 중에는 반응 하지 않음)
            if (player && gameObject.GetComponent<Renderer>().material.color != Constants.COLOR_FEAR && !PlayerBehaviour.isBlink)
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
                    if(MonsterGenerator.limitMode == true) lifeControl.GetComponent<LifeControl>().decreaseHp();

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

        public void MonsterLifeIncrease()
        {
            if (monsterLife == 2) return;
            else
            {
                monsterLife += 1;
                transform.localScale = new Vector3(transform.localScale.x / 0.7f, transform.localScale.y / 0.7f, transform.localScale.z);
            }
        }

        public void MonsterLifeDecrease()
        {
            if (monsterLife == 2)
            {
                monsterLife -= 1;
                transform.localScale = new Vector3(transform.localScale.x * 0.7f, transform.localScale.y * 0.7f, transform.localScale.z);
            }
            else MonsterLifeDestroy();
        }

        public void MonsterLifeDestroy()
        {
            monsterLife = 0;
            MonsterGenerator.monsters[curMoveX, curMoveY] = null;
            Manager.instance.AddScore(100);
            Destroy(gameObject);
        }

        public void MonsterCrowdControlTime()
        {
            //상태이상 빙결
            if (gameObject.GetComponent<Renderer>().material.color == Constants.COLOR_FREEZE)
            {
                float freezeTime = 10f;

                monsterType = Constants.BASIC_MONSTER;

                //얼음 캐릭터 특권
                if (SelectBManager.CharacterSelect == Constants.ICE_CHARACTER) freezeTime = 20f;

                if (this.deltaFreeze > freezeTime)
                {
                    this.deltaFreeze = 0;

                    gameObject.GetComponent<Renderer>().material.color = Constants.COLOR_BASIC;
                    monsterType = monsterTypeCurrent;

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
            if (gameObject.GetComponent<Renderer>().material.color == Constants.COLOR_BIND)
            {
                float bindTime = 10f;

                monsterType = Constants.BASIC_MONSTER;

                //풀 캐릭터 특권
                if (SelectBManager.CharacterSelect == Constants.GRASS_CHARACTER) bindTime = 20f;

                if (this.deltaBind > bindTime)
                {
                    this.deltaBind = 0;

                    gameObject.GetComponent<Renderer>().material.color = Constants.COLOR_BASIC;
                    monsterType = monsterTypeCurrent;

                }
                else this.deltaBind += Time.deltaTime;
            }
            //상태이상 공포
            if (gameObject.GetComponent<Renderer>().material.color == Constants.COLOR_FEAR)
            {
                float fearTime = 10f;

                monsterType = Constants.BASIC_MONSTER;

                //어둠 캐릭터 특권
                if (SelectBManager.CharacterSelect == Constants.DARK_CHARACTER) fearTime = 20f;

                if (this.deltaFear > fearTime)
                {
                    deltaFear = 0;

                    gameObject.GetComponent<Renderer>().material.color = Constants.COLOR_BASIC;
                    monsterType = monsterTypeCurrent;

                }
                else this.deltaFear += Time.deltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Bullet" && !isBulletDamaged) // 플레이어 총알
            {

                if (GetComponent<Renderer>().material.color == Constants.COLOR_FREEZE) return;

                isBulletDamaged = true;
                Invoke("BulletInvInvincible", 0.1f);

                GameObject buttonGauge;

                float skillCharge = 0.02f;

                //기본 캐릭터 특권
                if (SelectBManager.CharacterSelect == Constants.BASIC_CHARACTER) skillCharge = 0.025f;

                if (collision.transform.localScale.z == Constants.BULLET_FIRE)
                {
                    if (monsterType == Constants.FIRE_MONSTER || monsterType == Constants.BOSS_MONSTER)
                    {
                        if (monsterLife == 1)
                        {
                            MonsterLifeIncrease();
                        }
                    }
                    else
                    {
                        buttonGauge = GameObject.Find("Button_Fire");
                        buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
                        MonsterLifeDestroy();
                    }

                }

                if (collision.transform.localScale.z == Constants.BULLET_ICE)
                {
                    if (monsterType == Constants.ICE_MONSTER || monsterType == Constants.BOSS_MONSTER)
                    {
                        if (monsterLife == 1)
                        {
                            MonsterLifeIncrease();
                        }
                    }
                    else if (monsterLife == 1)
                    {
                        buttonGauge = GameObject.Find("Button_Ice");
                        buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
                        MonsterLifeDestroy();
                    }
                    else
                    {
                        buttonGauge = GameObject.Find("Button_Ice");
                        buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
                        MonsterLifeDecrease();
                        GetComponent<Renderer>().material.color = Constants.COLOR_FREEZE;
                    }
                }

                if (collision.transform.localScale.z == Constants.BULLET_GRASS)
                {
                    if (monsterType == Constants.GRASS_MONSTER || monsterType == Constants.BOSS_MONSTER)
                    {
                        if (monsterLife == 1)
                        {
                            MonsterLifeIncrease();
                        }
                    }
                    else if (monsterLife == 1)
                    {
                        buttonGauge = GameObject.Find("Button_Grass");
                        buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
                        MonsterLifeDestroy();
                    }
                    else
                    {
                        buttonGauge = GameObject.Find("Button_Grass");
                        buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
                        MonsterLifeDecrease();
                        GetComponent<Renderer>().material.color = Constants.COLOR_BIND;
                    }
                }

                if (collision.transform.localScale.z == Constants.BULLET_LIGHT)
                {
                    if (monsterType == Constants.LIGHT_MONSTER || monsterType == Constants.BOSS_MONSTER)
                    {
                        if (monsterLife == 1)
                        {
                            MonsterLifeIncrease();
                        }
                    }
                    else if (monsterLife == 1)
                    {
                        buttonGauge = GameObject.Find("Button_Light");
                        buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
                        MonsterLifeDestroy();
                    }
                    else
                    {
                        buttonGauge = GameObject.Find("Button_Light");
                        buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
                        MonsterLifeDecrease();
                    }
                }

                if (collision.transform.localScale.z == Constants.BULLET_DARK)
                {
                    if (monsterType == Constants.DARK_MONSTER || monsterType == Constants.BOSS_MONSTER)
                    {
                        if (monsterLife == 1)
                        {
                            MonsterLifeIncrease();
                        }
                    }
                    else if (monsterLife == 1)
                    {
                        buttonGauge = GameObject.Find("Button_Dark");
                        buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
                        MonsterLifeDestroy();
                    }
                    else
                    {
                        buttonGauge = GameObject.Find("Button_Dark");
                        buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
                        MonsterLifeDecrease();
                        GetComponent<Renderer>().material.color = Constants.COLOR_FEAR;
                    }
                }
            }
        }

        void BulletInvInvincible()
        {
            isBulletDamaged = false;
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

