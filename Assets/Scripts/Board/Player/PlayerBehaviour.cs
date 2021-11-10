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
        public static Vector3 currentTransformScale;

        //Player 애니메이션
        Animator animator;
        Vector3 moveVelocity = Vector3.zero;

        //Skill 관련
        public GameObject skillEffectPrefab;
        GameObject skillEffect;

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

                if (direction == Constants.PLAYERMOVE_LEFT)
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

                if (direction == Constants.PLAYERMOVE_RIGHT)
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

                if (direction == Constants.PLAYERMOVE_UP)
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

                if (direction == Constants.PLAYERMOVE_DOWN)
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

            //스킬

            if (inputSkillFire)
            {
                inputSkillFire = false;

                int SkillRange = 1;

                //불 캐릭터 특권
                if (SelectBManager.CharacterSelect == Constants.FIRE_CHARACTER) SkillRange = 2;

                GameObject buttonGauge;
                buttonGauge = GameObject.Find("Button_Fire");
                if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
                {
                    buttonGauge.GetComponent<Image>().fillAmount = 0f;
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
                                    if (monster.GetComponent<Renderer>().material.color != Constants.MONSTER_FREEZE 
                                        && monster.transform.localScale.z != Constants.FIRE_MONSTER)
                                    {
                                        damageMonsters.Add(monster);
                                        Manager.instance.AddScore(100);
                                    }
                                }
                                skillEffect = Instantiate(skillEffectPrefab, new Vector3(col - 4, row - 4, 0), Quaternion.identity);
                                skillEffect.GetComponent<Renderer>().material.color = Constants.MONSTER_FIRE;
                                Destroy(skillEffect, 0.5f);
                            }
                        }
                    }
                    damageMonsters.ForEach((monster) => MonsterBehaviour.Destroy(monster));
                }
            }

            if (inputSkillIce)
            {
                inputSkillIce = false;

                GameObject buttonGauge;
                buttonGauge = GameObject.Find("Button_Ice");
                if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
                {
                    buttonGauge.GetComponent<Image>().fillAmount = 0f;
                    SoundManager.instance.PlayOneShot(Clip.UseSkill);

                    int currentPlayerPositionRow = StageController.p_row;
                    int currentPlayerPositionCol = StageController.p_col;

                    List<GameObject> damageMonsters = new List<GameObject>();
                    GameObject monster;

                    for (int row = currentPlayerPositionRow - 2; row <= currentPlayerPositionRow + 2; row++)
                    {
                        for (int col = currentPlayerPositionCol - 2; col <= currentPlayerPositionCol + 2; col++)
                        {
                            if (0 <= row && row < 9 && 0 <= col && col < 9)
                            {
                                monster = MonsterGenerator.monsters[row, col];
                                if (monster)
                                {
                                    if (monster.GetComponent<Renderer>().material.color != Constants.MONSTER_FREEZE
                                        && monster.transform.localScale.z != Constants.ICE_MONSTER)
                                    {
                                        if (monster.transform.localScale == new Vector3(0.7f, 0.7f, monster.transform.localScale.z))
                                        {
                                            damageMonsters.Add(monster);
                                            Manager.instance.AddScore(100);
                                            MonsterGenerator.monsters[row, col] = null;
                                        }
                                        else
                                        {
                                            monster.transform.localScale = new Vector3(0.7f, 0.7f, monster.transform.localScale.z);
                                            monster.GetComponent<Renderer>().material.color = Constants.MONSTER_FREEZE;
                                        }
                                    }
                                }
                                skillEffect = Instantiate(skillEffectPrefab, new Vector3(col - 4, row - 4, 0), Quaternion.identity);
                                skillEffect.GetComponent<Renderer>().material.color = Constants.MONSTER_FREEZE;
                                Destroy(skillEffect, 0.5f);
                            }
                        }
                    }
                    damageMonsters.ForEach((monster) => MonsterBehaviour.Destroy(monster));
                }
            }

            if (inputSkillGrass)
            {
                inputSkillGrass = false;

                GameObject buttonGauge;
                buttonGauge = GameObject.Find("Button_Grass");
                if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
                {
                    buttonGauge.GetComponent<Image>().fillAmount = 0f;
                    SoundManager.instance.PlayOneShot(Clip.UseSkill);
                    int currentPlayerPositionRow = StageController.p_row;
                    int currentPlayerPositionCol = StageController.p_col;

                    List<GameObject> damageMonsters = new List<GameObject>();

                    for (int i = 0; i < 9; i++)
                    {
                        int row = currentPlayerPositionRow;
                        int col = currentPlayerPositionCol;
                        GameObject monster = MonsterGenerator.monsters[row, i];
                        if (monster)
                        {
                            if (monster.GetComponent<Renderer>().material.color != Constants.MONSTER_FREEZE
                                && monster.transform.localScale.z != Constants.GRASS_MONSTER)
                            {
                                if (monster.transform.localScale == new Vector3(0.7f, 0.7f, monster.transform.localScale.z))
                                {
                                    damageMonsters.Add(monster);
                                    Manager.instance.AddScore(100);
                                    MonsterGenerator.monsters[row, i] = null;
                                }
                                else
                                {
                                    monster.transform.localScale = new Vector3(0.7f, 0.7f, monster.transform.localScale.z);
                                    monster.GetComponent<Renderer>().material.color = Constants.MONSTER_BIND;
                                }
                            }
                        }

                        monster = MonsterGenerator.monsters[i, col];
                        if (monster)
                        {
                            if (monster.GetComponent<Renderer>().material.color != Constants.MONSTER_FREEZE
                                && monster.transform.localScale.z != Constants.GRASS_MONSTER)
                            {
                                if (monster.transform.localScale == new Vector3(0.7f, 0.7f, monster.transform.localScale.z))
                                {
                                    damageMonsters.Add(monster);
                                    Manager.instance.AddScore(100);
                                    MonsterGenerator.monsters[i, col] = null;
                                }
                                else
                                {
                                    monster.transform.localScale = new Vector3(0.7f, 0.7f, monster.transform.localScale.z);
                                    monster.GetComponent<Renderer>().material.color = Constants.MONSTER_BIND;
                                }
                            }
                        }

                        skillEffect = Instantiate(skillEffectPrefab, new Vector3(i - 4, row - 4, 0), Quaternion.identity);
                        skillEffect.GetComponent<Renderer>().material.color = Constants.MONSTER_BIND;
                        Destroy(skillEffect, 0.5f);

                        skillEffect = Instantiate(skillEffectPrefab, new Vector3(col - 4, i - 4, 0), Quaternion.identity);
                        skillEffect.GetComponent<Renderer>().material.color = Constants.MONSTER_BIND;
                        Destroy(skillEffect, 0.5f);
                    }

                    damageMonsters.ForEach((monster) => MonsterBehaviour.Destroy(monster));
                }
            }

            if (inputSkillLight)
            {
                inputSkillLight = false;

                GameObject buttonGauge;
                buttonGauge = GameObject.Find("Button_Light");
                if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
                {
                    buttonGauge.GetComponent<Image>().fillAmount = 0f;
                    SoundManager.instance.PlayOneShot(Clip.LifeUP);

                    GameObject lifeControl = GameObject.Find("LifeControl");
                    lifeControl.GetComponent<LifeControl>().increaseHp();

                    //빛 캐릭터 특권
                    if (SelectBManager.CharacterSelect == Constants.LIGHT_CHARACTER)
                    {
                        lifeControl.GetComponent<LifeControl>().increaseHp();
                    }
                }
            }

            if (inputSkillDark)
            {
                inputSkillDark = false;

                GameObject buttonGauge;
                buttonGauge = GameObject.Find("Button_Dark");
                if (buttonGauge.GetComponent<Image>().fillAmount >= 0.99f)
                {
                    buttonGauge.GetComponent<Image>().fillAmount = 0f;
                    SoundManager.instance.PlayOneShot(Clip.UseSkill);

                    GameObject monster;

                    for (int row = 0; row < 9; row++)
                    {
                        for (int col = 0; col < 9; col++)
                        {
                            monster = MonsterGenerator.monsters[row, col];
                            if (monster)
                            {
                                if (monster.GetComponent<Renderer>().material.color != Constants.MONSTER_FREEZE
                                    && monster.transform.localScale.z != Constants.DARK_MONSTER)
                                {
                                    monster.GetComponent<Renderer>().material.color = Constants.MONSTER_FEAR;
                                }
                            }
                        }
                    }
                }
            }

            
            //충돌 시 깜빡임
            if (playerDamaged)
            {
                StartCoroutine(PlayerBlink());
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

