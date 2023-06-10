using System;
using System.Collections;
using System.Collections.Generic;
using Totomaro.Board;
using Totomaro.Core;
using Totomaro.Stage;
using Totomaro.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Totomaro.Board
{
    public class PlayerBehaviour : MonoBehaviour
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
        public bool inputSkill = false;
        public bool inputSkillFire = false;
        public bool inputSkillIce = false;
        public bool inputSkillGrass = false;
        public bool inputSkillLight = false;
        public bool inputSkillDark = false;

        public static bool playerDamaged = false;

        public int direction = 0; // 캐릭터의 방향 left = 2, right = 0, up = 1, down = 3
        public int PrevDirection = 0;
        public int kickDirection = 0;
        public int skillDirection = 0;

        public GameObject bulletFire;
        public GameObject bulletIce;
        public GameObject bulletGrass;
        public GameObject bulletLight;
        public GameObject bulletDark;

        GameObject bullet;

        public int playerBulletLimit = 0;

        //PlayerMove 관련
        public bool isMoving { get; set; }

        Vector3 movement;
        Queue<Vector3> m_MovementQueue = new Queue<Vector3>();

        public int playerCol;
        public int playerRow;

        //Player 깜빡임 관련

        public static bool isBlink { get; set; }
        public static Vector3 currentTransformScale;

        //Player 애니메이션
        Animator animator;
        Vector3 moveVelocity = Vector3.zero;

        //Skill 관련
        public GameObject skillEffectPrefab;
        GameObject skillEffect;

        StageController m_Board;

        void Start()
        {
            //m_SpriteRenderer = GetComponent<SpriteRenderer>();
            ButtonManager ui = GameObject.FindGameObjectWithTag("ManagerUi").GetComponent<ButtonManager>();
            ui.init();
            animator = GetComponent<Animator>();
            
        }

        //GameObject stat;
        //StageController playerSent;

        //정보를 보낼 주소
        public void init()
        {
            //stat = GameObject.Find("Board");
            //playerSent = stat.GetComponent<StageController>();
        }

        void Update()
        {
            m_BlockPos.col = (int)(transform.position.x + 4f);
            m_BlockPos.row = (int)(transform.position.y + 4f);

            if (!isMoving) animator.SetFloat("velocity", 0);
            else animator.SetFloat("velocity", 2);

            if (inputLeft)
            {
                transform.localScale = new Vector3(1, 1, 1);
                currentTransformScale = gameObject.transform.localScale;
                inputLeft = false;
                PrevDirection = direction;
                //playerSent.direct = 2;

                SoundManager.instance.PlayOneShot(Clip.CharacterButton);

                if (direction == Constants.DIRECTION_LEFT)
                {
                    int nCol = PlayerGenerator.playerx + 4;
                    int nRow = PlayerGenerator.playery + 4;
                    //x축 , 맵 밖으로 나가는것 막기
                    if (0 < nCol)
                    {
                        //Debug.Log($"----- {nCol} -> {nCol - 1}, {nRow} -> {nRow}");
                        PlayerMove(new Vector3(-1, 0, 1));
                        PlayerGenerator.playerx -= 1;

                        m_BlockPos.col -= 1;

                        StageController.p_col += -1;
                    }
                    else this.delta = this.span;
                }
            }
            else if (inputRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                currentTransformScale = gameObject.transform.localScale;
                inputRight = false;
                PrevDirection = direction;
                //playerSent.direct = 0;

                SoundManager.instance.PlayOneShot(Clip.CharacterButton);

                if (direction == Constants.DIRECTION_RIGHT)
                {
                    int nCol = PlayerGenerator.playerx + 4;
                    int nRow = PlayerGenerator.playery + 4;
                    //x축 , 맵 밖으로 나가는것 막기
                    if (nCol + 1 < 9)
                    {
                        //Debug.Log($"----- {nCol} -> {nCol - 1}, {nRow} -> {nRow}");

                        PlayerMove(new Vector3(1, 0, 1));
                        PlayerGenerator.playerx += 1;

                        m_BlockPos.col += 1;

                        StageController.p_col += 1;

                    }
                    else this.delta = this.span;
                }
            }
            else if (inputUp)
            {
                //transform.localScale = new Vector3(1, 1, 1);
                inputUp = false;
                PrevDirection = direction;
                //playerSent.direct = 1;

                SoundManager.instance.PlayOneShot(Clip.CharacterButton);

                if (direction == Constants.DIRECTION_UP)
                {
                    int nCol = PlayerGenerator.playerx + 4;
                    int nRow = PlayerGenerator.playery + 4;
                    //x축 , 맵 밖으로 나가는것 막기
                    if (nRow + 1 < 9)
                    {
                        //Debug.Log($"----- {nCol} -> {nCol}, {nRow} -> {nRow + 1}");

                        PlayerMove(new Vector3(0, 1, 1));
                        PlayerGenerator.playery += 1;

                        m_BlockPos.row += 1;

                        StageController.p_row += 1;
                    }
                    else this.delta = this.span;
                }
            }
            else if (inputDown)
            {
                //transform.localScale = new Vector3(1, -1, 1);
                inputDown = false;
                PrevDirection = direction;
                //playerSent.direct = 2;

                SoundManager.instance.PlayOneShot(Clip.CharacterButton);

                if (direction == Constants.DIRECTION_DOWN)
                {
                    int nCol = PlayerGenerator.playerx + 4;
                    int nRow = PlayerGenerator.playery + 4;
                    //x축 , 맵 밖으로 나가는것 막기
                    if (0 < nRow)
                    {
                        //Debug.Log($"----- {nCol} -> {nCol}, {nRow} -> {nRow - 1}");

                        PlayerMove(new Vector3(0, -1, 1));
                        PlayerGenerator.playery -= 1;

                        m_BlockPos.row -= 1;

                        StageController.p_row += -1;
                    }
                    else this.delta = this.span;
                }
            }

            if (inputSkillFire)
            {
                inputSkillFire = false;

                int SkillRange = 4;

                //불 캐릭터 특권
                if (SelectBManager.CharacterSelect == Constants.FIRE_CHARACTER) SkillRange = 5;

                SoundManager.instance.PlayOneShot(Clip.UseSkill);

                int currentPlayerPositionRow = StageController.p_row;
                int currentPlayerPositionCol = StageController.p_col;

                List<GameObject> damageMonsters = new List<GameObject>();
                GameObject monster;

                for (int row = currentPlayerPositionRow - SkillRange; row <= currentPlayerPositionRow + SkillRange; row++)
                {
                    for (int col = currentPlayerPositionCol - SkillRange; col <= currentPlayerPositionCol + SkillRange; col++)
                    {
                        if (0 <= row && row < 9 && 0 <= col && col < 9)
                        {
                            float rowCal = row - currentPlayerPositionRow;
                            float colCal = col - currentPlayerPositionCol;
                            float absX = Math.Abs(rowCal);
                            float absY = Math.Abs(colCal);
                            float correct = 0;

                            //오른쪽
                            if (skillDirection == Constants.DIRECTION_RIGHT)
                            {
                                absX = Math.Abs(rowCal);
                                absY = Math.Abs(colCal);
                                correct = colCal;
                            } else

                            //왼쪽
                            if (skillDirection == Constants.DIRECTION_LEFT)
                            {
                                absX = Math.Abs(rowCal);
                                absY = Math.Abs(colCal);
                                correct = -colCal;
                            } else

                            //위
                            if (skillDirection == Constants.DIRECTION_UP)
                            {
                                absX = Math.Abs(colCal);
                                absY = Math.Abs(rowCal);
                                correct = rowCal;
                            } else

                            //아래
                            if (skillDirection == Constants.DIRECTION_DOWN)
                            {
                                absX = Math.Abs(colCal);
                                absY = Math.Abs(rowCal);
                                correct = -rowCal;
                            }

                            if (absX < absY && correct > 0)
                            {
                                monster = MonsterGenerator.monsters[row, col];
                                if (monster)
                                {
                                    int monsterType = monster.GetComponent<MonsterBehaviour>().monsterType;
                                    int monsterLife = monster.GetComponent<MonsterBehaviour>().monsterLife;

                                    if (monster.GetComponent<Renderer>().material.color != Constants.COLOR_FREEZE
                                        && monsterType != Constants.FIRE_MONSTER)
                                    {
                                        damageMonsters.Add(monster);
                                        Manager.instance.AddScore(100);
                                        MonsterGenerator.monsters[row, col] = null;
                                    }
                                }
                                skillEffect = Instantiate(skillEffectPrefab, new Vector3(col - 4, row - 4, 0), Quaternion.identity);
                                skillEffect.GetComponent<Renderer>().material.color = Constants.COLOR_BURN;
                                Destroy(skillEffect, 0.5f);
                            }
                        }
                    }
                }
                damageMonsters.ForEach((monster) => MonsterBehaviour.Destroy(monster));
            }

            if (inputSkillIce)
            {
                inputSkillIce = false;

                int SkillRange = 3;

                SoundManager.instance.PlayOneShot(Clip.UseSkill);

                int currentPlayerPositionRow = StageController.p_row;
                int currentPlayerPositionCol = StageController.p_col;

                List<GameObject> damageMonsters = new List<GameObject>();
                GameObject monster;

                for (int row = currentPlayerPositionRow - SkillRange; row <= currentPlayerPositionRow + SkillRange; row++)
                {
                    for (int col = currentPlayerPositionCol - SkillRange; col <= currentPlayerPositionCol + SkillRange; col++)
                    {
                        if (0 <= row && row < 9 && 0 <= col && col < 9)
                        {
                            monster = MonsterGenerator.monsters[row, col];
                            if (monster)
                            {
                                int monsterType = monster.GetComponent<MonsterBehaviour>().monsterType;
                                int monsterLife = monster.GetComponent<MonsterBehaviour>().monsterLife;

                                if (monster.GetComponent<Renderer>().material.color != Constants.COLOR_FREEZE
                                    && monsterType != Constants.ICE_MONSTER)
                                {
                                    if (monsterLife == 1)
                                    {
                                        damageMonsters.Add(monster);
                                        Manager.instance.AddScore(100);
                                        MonsterGenerator.monsters[row, col] = null;
                                    }
                                    else
                                    {
                                        monster.GetComponent<MonsterBehaviour>().MonsterLifeDecrease();
                                        monster.GetComponent<Renderer>().material.color = Constants.COLOR_FREEZE;
                                    }
                                }
                            }
                            skillEffect = Instantiate(skillEffectPrefab, new Vector3(col - 4, row - 4, 0), Quaternion.identity);
                            skillEffect.GetComponent<Renderer>().material.color = Constants.COLOR_FREEZE;
                            Destroy(skillEffect, 0.5f);
                        }
                    }
                }
                damageMonsters.ForEach((monster) => MonsterBehaviour.Destroy(monster));

            }

            if (inputSkillGrass)
            {
                inputSkillGrass = false;

                SoundManager.instance.PlayOneShot(Clip.UseSkill);

                int currentPlayerPositionRow = StageController.p_row;
                int currentPlayerPositionCol = StageController.p_col;

                List<GameObject> damageMonsters = new List<GameObject>();

                if (skillDirection == Constants.DIRECTION_LEFT || skillDirection == Constants.DIRECTION_RIGHT)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int row = currentPlayerPositionRow + j;
                            int col = currentPlayerPositionCol + j;

                            //가로
                            if (0 <= row && row < 9)
                            {
                                GameObject monster = MonsterGenerator.monsters[row, i];
                                if (monster)
                                {
                                    int monsterType = monster.GetComponent<MonsterBehaviour>().monsterType;
                                    int monsterLife = monster.GetComponent<MonsterBehaviour>().monsterLife;

                                    if (monster.GetComponent<Renderer>().material.color != Constants.COLOR_FREEZE
                                        && monsterType != Constants.GRASS_MONSTER)
                                    {
                                        if (monsterLife == 1)
                                        {
                                            damageMonsters.Add(monster);
                                            Manager.instance.AddScore(100);
                                            MonsterGenerator.monsters[row, i] = null;
                                        }
                                        else
                                        {
                                            monster.GetComponent<MonsterBehaviour>().MonsterLifeDecrease();
                                            monster.GetComponent<Renderer>().material.color = Constants.COLOR_BIND;
                                        }
                                    }
                                }

                                skillEffect = Instantiate(skillEffectPrefab, new Vector3(i - 4, row - 4, 0), Quaternion.identity);
                                skillEffect.GetComponent<Renderer>().material.color = Constants.COLOR_BIND;
                                Destroy(skillEffect, 0.5f);
                            }
                        }
                    }
                }

                if (skillDirection == Constants.DIRECTION_UP || skillDirection == Constants.DIRECTION_DOWN)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int row = currentPlayerPositionRow + j;
                            int col = currentPlayerPositionCol + j;

                            //세로
                            if (0 <= col && col < 9)
                            {
                                monster = MonsterGenerator.monsters[i, col];
                                if (monster)
                                {
                                    int monsterType = monster.GetComponent<MonsterBehaviour>().monsterType;
                                    int monsterLife = monster.GetComponent<MonsterBehaviour>().monsterLife;

                                    if (monster.GetComponent<Renderer>().material.color != Constants.COLOR_FREEZE
                                        && monsterType != Constants.GRASS_MONSTER)
                                    {
                                        if (monsterLife == 1)
                                        {
                                            damageMonsters.Add(monster);
                                            Manager.instance.AddScore(100);
                                            MonsterGenerator.monsters[row, i] = null;
                                        }
                                        else
                                        {
                                            monster.GetComponent<MonsterBehaviour>().MonsterLifeDecrease();
                                            monster.GetComponent<Renderer>().material.color = Constants.COLOR_BIND;
                                        }
                                    }
                                }

                                skillEffect = Instantiate(skillEffectPrefab, new Vector3(col - 4, i - 4, 0), Quaternion.identity);
                                skillEffect.GetComponent<Renderer>().material.color = Constants.COLOR_BIND;
                                Destroy(skillEffect, 0.5f);
                            }
                        }
                    }
                }


                damageMonsters.ForEach((monster) => MonsterBehaviour.Destroy(monster));
            }

            if (inputSkillLight)
            {
                inputSkillLight = false;

                SoundManager.instance.PlayOneShot(Clip.LifeUP);

                GameObject lifeControl = GameObject.Find("LifeControl");
                lifeControl.GetComponent<LifeControl>().increaseHp();
                lifeControl.GetComponent<LifeControl>().increaseHp();

                //빛 캐릭터 특권
                if (SelectBManager.CharacterSelect == Constants.LIGHT_CHARACTER)
                {
                    lifeControl.GetComponent<LifeControl>().increaseHp();
                    lifeControl.GetComponent<LifeControl>().increaseHp();
                }
            }

            if (inputSkillDark)
            {
                inputSkillDark = false;

                SoundManager.instance.PlayOneShot(Clip.UseSkill);

                GameObject monster;

                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        monster = MonsterGenerator.monsters[row, col];
                        if (monster)
                        {
                            int monsterType = monster.GetComponent<MonsterBehaviour>().monsterType;

                            if (monster.GetComponent<Renderer>().material.color != Constants.COLOR_FREEZE
                                && monsterType != Constants.DARK_MONSTER)
                            {
                                monster.GetComponent<Renderer>().material.color = Constants.COLOR_FEAR;
                            }
                        }
                    }
                }
            }

            if (inputSkill)
            {
                inputSkill = false;

                if (SkillPortionGenerator.skillGameObject[0])
                {
                    float kindSkill = SkillPortionGenerator.skillGameObject[0].transform.localScale.z;

                    if (kindSkill == Constants.SKILLPORTION_FIRE) inputSkillFire = true;
                    if (kindSkill == Constants.SKILLPORTION_ICE) inputSkillIce = true;
                    if (kindSkill == Constants.SKILLPORTION_GRASS) inputSkillGrass = true;
                    if (kindSkill == Constants.SKILLPORTION_LIGHT) inputSkillLight = true;
                    if (kindSkill == Constants.SKILLPORTION_DARK) inputSkillDark = true;

                    Destroy(SkillPortionGenerator.skillGameObject[0]);
                    SkillPortionGenerator.skillGameObject[0] = null;

                    GameObject.FindGameObjectWithTag("SkillPortionGenerator").GetComponent<SkillPortionGenerator>().SkillPullSort();
                }
            }

            if (inputKick)
            {
                if (playerBulletLimit == 0)
                {
                    inputKick = false;
                    return;
                }

                playerBulletLimit--;

                bullet = Instantiate(bulletFire, transform.position, transform.rotation);
                BulletChange();

                Rigidbody2D rigidBody = bullet.GetComponent<Rigidbody2D>();
                
                if (kickDirection == Constants.DIRECTION_DOWN) rigidBody.AddForce(Vector2.down * 5, ForceMode2D.Impulse);
                if (kickDirection == Constants.DIRECTION_UP) rigidBody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                if (kickDirection == Constants.DIRECTION_LEFT) rigidBody.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
                if (kickDirection == Constants.DIRECTION_RIGHT) rigidBody.AddForce(Vector2.right * 5, ForceMode2D.Impulse);

                inputKick = false;
            }
            
            //충돌 시 무적
            if (playerDamaged)
            {
                StartCoroutine(PlayerBlink());
            }
            
        }

        void BulletChange()
        {
            int bulletBreed = Stage.Stage.bulletBreed;

            float bulletSize = 1f;

            if (bulletBreed == Constants.BLOCK_FIRE)
            {
                bullet.GetComponent<SpriteRenderer>().sprite = bulletFire.GetComponent<SpriteRenderer>().sprite;
                bullet.transform.localScale = new Vector3(bulletSize, bulletSize, Constants.BULLET_FIRE);
            }

            if (bulletBreed == Constants.BLOCK_ICE)
            {
                bullet.GetComponent<SpriteRenderer>().sprite = bulletIce.GetComponent<SpriteRenderer>().sprite;
                bullet.transform.localScale = new Vector3(bulletSize, bulletSize, Constants.BULLET_ICE);
            }

            if (bulletBreed == Constants.BLOCK_GRASS)
            {
                bullet.GetComponent<SpriteRenderer>().sprite = bulletGrass.GetComponent<SpriteRenderer>().sprite;
                bullet.transform.localScale = new Vector3(bulletSize, bulletSize, Constants.BULLET_GRASS);
            }

            if (bulletBreed == Constants.BLOCK_LIGHT)
            {
                bullet.GetComponent<SpriteRenderer>().sprite = bulletLight.GetComponent<SpriteRenderer>().sprite;
                bullet.transform.localScale = new Vector3(bulletSize, bulletSize, Constants.BULLET_LIGHT);
            }

            if (bulletBreed == Constants.BLOCK_DARK)
            {
                bullet.GetComponent<SpriteRenderer>().sprite = bulletDark.GetComponent<SpriteRenderer>().sprite;
                bullet.transform.localScale = new Vector3(bulletSize, bulletSize, Constants.BULLET_DARK);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "SkillPortion")
            {
                if (collision.transform.localScale.z == Constants.SKILLPORTION_FIRE) inputSkillFire = true;
                if (collision.transform.localScale.z == Constants.SKILLPORTION_ICE) inputSkillIce = true;
                if (collision.transform.localScale.z == Constants.SKILLPORTION_GRASS) inputSkillGrass = true;
                if (collision.transform.localScale.z == Constants.SKILLPORTION_LIGHT) inputSkillLight = true;
                if (collision.transform.localScale.z == Constants.SKILLPORTION_DARK) inputSkillDark = true;

                Destroy(collision.gameObject);
            }
        }

        public IEnumerator PlayerBlink()
        {
            if (!isBlink)
            {
                isBlink = true;
                
                WaitForSeconds waitForSeconds = new WaitForSeconds(0.4f);

                for (int i = 0; i < 5; i++)
                {
                    gameObject.transform.localScale = new Vector3(0, 0, 0);
                    yield return waitForSeconds;
                    gameObject.transform.localScale = currentTransformScale;
                    yield return waitForSeconds;
                }
                playerDamaged = false;
                isBlink = false;
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

        IEnumerator DoActionPlayerMove(float acc = 0.8f)
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