using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ninez.Quest;
using Ninez.Util;
using Ninez.Stage;
using Ninez.Core;
using UnityEngine.UI;

namespace Ninez.Board
{
    using IntIntKV = KeyValuePair<int, int>;

    public class Board
    {
        int m_nRow;
        int m_nCol;

        public int maxRow { get { return m_nRow; } }
        public int maxCol { get { return m_nCol; } }

        Cell[,] m_Cells;
        public Cell[,] cells { get { return m_Cells; } }

        Block[,] m_Blocks;
        public Block[,] blocks { get { return m_Blocks; } }

   
        Transform m_Container;
        GameObject m_CellPrefab;
        GameObject m_BlockPrefab;
        GameObject m_MonsterPrefab;
        GameObject m_PlayerPrefab;
        StageBuilder m_StageBuilder;

        BoardEnumerator m_Enumerator;

        public Board(int nRow, int nCol)
        {
            m_nRow = nRow;
            m_nCol = nCol;

            m_Cells = new Cell[nRow, nCol];
            m_Blocks = new Block[nRow, nCol];

            m_Enumerator = new BoardEnumerator(this);
        }



        internal void ComposeStage(GameObject cellPrefab, GameObject blockPrefab, Transform container, StageBuilder stageBuilder)
        {
            //1. 스테이지 구성에 필요한 Cell,Block, Container(Board) 정보를 저장한다. 
            m_CellPrefab = cellPrefab;
            m_BlockPrefab = blockPrefab;
            m_Container = container;
            m_StageBuilder = stageBuilder;

            //2. 3매치된 블럭이 없도록 섞는다.  
            BoardShuffler shuffler = new BoardShuffler(this, true);
            shuffler.Shuffle();

            //3. Cell, Block Prefab을 이용해서 Board에 Cell/Block GameObject를 추가한다. 
            float initX = CalcInitX(0.5f);
            float initY = CalcInitY(0.5f);
            for (int nRow = 0; nRow < m_nRow; nRow++)
            {
                for (int nCol = 0; nCol < m_nCol; nCol++)
                {
                    //3.1 Cell GameObject 생성을 요청한다.GameObject가 생성되지 않는 경우에 null을 리턴한다.
                    Cell cell = m_Cells[nRow, nCol]?.InstantiateCellObj(cellPrefab, container);
                    cell?.Move(initX + nCol, initY + nRow);

                    //3.2 Block GameObject 생성을 요청한다.
                    //    GameObject가 생성되지 않는 경우에 null을 리턴한다. EMPTY 인 경우에 null
                    Block block = m_Blocks[nRow, nCol]?.InstantiateBlockObj(blockPrefab, container);
                    block?.Move(initX + nCol, initY + nRow);
                }
            }
        }


        /**
         * 호출 결과 : 3 매칭된 블럭이 제거된다.
         */
        public IEnumerator Evaluate(Returnable<bool> matchResult)
        {
            //1. 모든 블럭의 매칭 정보(개수, 상태, 내구도)를 계산한 후, 3매치 블럭이 있으면 true 리턴 
            bool bMatchedBlockFound = UpdateAllBlocksMatchedStatus();

            //2. 3매칭 블럭 없는 경우 
            if(bMatchedBlockFound == false)
            {
                matchResult.value = false;
                yield break;
            }

            //3. 3매칭 블럭 있는 경우

            //특수 Match
            SpecialMatchAddExplosion();
            
            //3.1. 첫번째 phase
            //   매치된 블럭에 지정된 액션을 수행한.
            //   ex) 가로줄의 블럭 전체가 클리어 되는 블럭인 경우에 처리 등
            for (int nRow = 0; nRow < m_nRow; nRow++)
            {
                for (int nCol = 0; nCol < m_nCol; nCol++)
                {
                    Block block = m_Blocks[nRow, nCol];

                    block?.DoEvaluation(m_Enumerator, nRow, nCol);
                }
            }
                
            //3.2. 두번째 phase
            //   첫번째 Phase에서 반영된 블럭의 상태값에 따라서 블럭의 최종 상태를 반영한.
            List<Block> clearBlocks = new List<Block>();
            List<GameObject> damageMonsters = new List<GameObject>();
            
            for (int nRow = 0; nRow < m_nRow; nRow++)
            {
                for (int nCol = 0; nCol < m_nCol; nCol++)
                {
                    Block block = m_Blocks[nRow, nCol];

                    if (block != null)
                    {
                        if (block.status == BlockStatus.CLEAR)
                        {
                            DamageToMonster(damageMonsters, nRow, nCol);

                            AddSkillGauge(nRow, nCol);

                            clearBlocks.Add(block);

                            m_Blocks[nRow, nCol] = null;    //보드에서 블럭 제거 (블럭 객체 제거 X)
                        }
                    }
                }
            }

            damageMonsters.ForEach((monster) => MonsterBehaviour.Destroy(monster));

            //3.3 매칭된 블럭을 제거한다. 
            clearBlocks.ForEach((block) => block.Destroy());
            //3.3.1 블럭이 제거되는 동안 잠시 Delay, 블럭 제거가 순식간에 일어나는 것에 약간 지연을 시킴
            yield return new WaitForSeconds(0.2f);

            //3.4 3매칭 블럭 있는 경우 true 설정   
            matchResult.value = true;

            yield break;
        }

        //Monster 제거 메소드
        public void DamageToMonster(List<GameObject> damageMonsters,int nRow, int nCol)
        {
            GameObject monster = MonsterGenerator.monsters[nRow, nCol];
            if (monster)
            {
                float MonsterBreed = 0;

                if (monster.transform.localScale.z == Constants.NORMAL_MONSTER) MonsterBreed = Constants.NORMAL_MONSTER;

                if (monster.transform.localScale.z == Constants.FIRE_MONSTER)
                {
                    if (m_Blocks[nRow, nCol].breed != (BlockBreed)Constants.BLOCK_FIRE) MonsterBreed = Constants.FIRE_MONSTER;
                    else MonsterBreed = Constants.X_MONSTER;
                }

                if (monster.transform.localScale.z == Constants.ICE_MONSTER)
                {
                    if (m_Blocks[nRow, nCol].breed != (BlockBreed)Constants.BLOCK_ICE) MonsterBreed = Constants.ICE_MONSTER;
                    else MonsterBreed = Constants.X_MONSTER;
                }

                if (monster.transform.localScale.z == Constants.GRASS_MONSTER)
                {
                    if (m_Blocks[nRow, nCol].breed != (BlockBreed)Constants.BLOCK_GRASS) MonsterBreed = Constants.GRASS_MONSTER;
                    else MonsterBreed = Constants.X_MONSTER;
                }

                if (monster.transform.localScale.z == Constants.LIGHT_MONSTER)
                {
                    if (m_Blocks[nRow, nCol].breed != (BlockBreed)Constants.BLOCK_LIGHT) MonsterBreed = Constants.LIGHT_MONSTER;
                    else MonsterBreed = Constants.X_MONSTER;
                }

                if (monster.transform.localScale.z == Constants.DARK_MONSTER)
                {
                    if (m_Blocks[nRow, nCol].breed != (BlockBreed)Constants.BLOCK_DARK) MonsterBreed = Constants.DARK_MONSTER;
                    else MonsterBreed = Constants.X_MONSTER;
                }

                //(빙결 상태의 몬스터는 제거할 수 없음)
                if (monster.GetComponent<Renderer>().material.color != Constants.MONSTER_FREEZE && MonsterBreed != Constants.X_MONSTER)
                {
                    //제거 (불 속성은 바로 제거)
                    if (monster.transform.localScale == new Vector3(0.7f, 0.7f, MonsterBreed) ||
                    m_Blocks[nRow, nCol].breed == (BlockBreed)Constants.BLOCK_FIRE)
                    {
                        if (monster != null)
                        {
                            damageMonsters.Add(monster);
                            Manager.instance.AddScore(100);

                            MonsterGenerator.monsters[nRow, nCol] = null; //보드에서 몬스터 제거 (몬스터 객체 제거 X)
                        }
                    }
                    else
                    {
                        //라이프 감소
                        monster.transform.localScale = new Vector3(0.7f, 0.7f, MonsterBreed);
                        GiveMonsterCrowdControl(monster, nRow, nCol);
                    }
                }
            }
        }

        public void GiveMonsterCrowdControl(GameObject monster, int nRow, int nCol)
        {
            //상태이상 빙결
            if (m_Blocks[nRow, nCol].breed == (BlockBreed)Constants.BLOCK_ICE)
            {
                monster.GetComponent<Renderer>().material.color = Constants.MONSTER_FREEZE;
            }

            //상태이상 속박
            if (m_Blocks[nRow, nCol].breed == (BlockBreed)Constants.BLOCK_GRASS)
            {
                monster.GetComponent<Renderer>().material.color = Constants.MONSTER_BIND;
            }

            //상태이상 공포
            if (m_Blocks[nRow, nCol].breed == (BlockBreed)Constants.BLOCK_DARK)
            {
                monster.GetComponent<Renderer>().material.color = Constants.MONSTER_FEAR;
            }
        }

        public void SpecialMatchAddExplosion()
        {
            for (int nRow = 0; nRow < m_nRow; nRow++)
            {
                for (int nCol = 0; nCol < m_nCol; nCol++)
                {
                    Block block = m_Blocks[nRow, nCol];

                    //가로 4Match
                    if (block.questType == BlockQuestType.CLEAR_HORZ)
                    {
                        ActionManager.isSpecialClear = true;
                        for (int col = 0; col < m_nCol; col++)
                        {
                            block = m_Blocks[nRow, col];
                            block.status = BlockStatus.MATCH;
                        }
                    }

                    //세로 4Match
                    if (block.questType == BlockQuestType.CLEAR_VERT)
                    {
                        ActionManager.isSpecialClear = true;
                        for (int row = 0; row < m_nRow; row++)
                        {
                            block = m_Blocks[row, nCol];
                            block.status = BlockStatus.MATCH;
                        }
                    }

                    //5Match
                    if (block.questType == BlockQuestType.CLEAR_LAZER)
                    {
                        ActionManager.isSpecialClear = true;
                        BlockBreed targetBlockBreed = block.breed;
                        for (int row = 0; row < m_nRow; row++)
                        {
                            for (int col = 0; col < m_nCol; col++)
                            {
                                block = m_Blocks[row, col];
                                if (block.breed == targetBlockBreed)
                                {
                                    block.status = BlockStatus.MATCH;
                                }
                            }
                        }
                    }

                    //3x3Match 이상
                    if (block.questType == BlockQuestType.CLEAR_CIRCLE)
                    {
                        ActionManager.isSpecialClear = true;
                        for (int row = nRow - 2; row <= nRow + 2; row++)
                        {
                            for (int col = nCol - 2; col <= nCol + 2; col++)
                            {
                                if (0 <= row && row < m_nRow && 0 <= col && col < m_nCol)
                                {
                                    if(Math.Abs(row - nRow) + Math.Abs(col - nCol) < 3)
                                    {
                                        block = m_Blocks[row, col];
                                        block.status = BlockStatus.MATCH;
                                    }
                                }
                            }
                        }
                    }

                    //5x5Match
                    if (block.questType == BlockQuestType.CLEAR_ALL)
                    {
                        ActionManager.isSpecialClear = true;
                        for (int row = 0; row < m_nRow; row++)
                        {
                            for (int col = 0; col < m_nCol; col++)
                            {
                                block = m_Blocks[row, col];
                                block.status = BlockStatus.MATCH;
                            }
                        }
                    }
                }
            }
        }

        public void AddSkillGauge(int nRow, int nCol)
        {
            GameObject buttonGauge;

            float skillCharge = 0.025f;

            //기본 캐릭터 특권
            if (SelectBManager.CharacterSelect == Constants.BASIC_CHARACTER) skillCharge = 0.03125f;

            if (m_Blocks[nRow, nCol].breed == (BlockBreed)Constants.BLOCK_FIRE)
            {
                buttonGauge = GameObject.Find("Button_Fire");
                buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
            }

            if (m_Blocks[nRow, nCol].breed == (BlockBreed)Constants.BLOCK_ICE)
            {
                buttonGauge = GameObject.Find("Button_Ice");
                buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
            }

            if (m_Blocks[nRow, nCol].breed == (BlockBreed)Constants.BLOCK_GRASS)
            {
                buttonGauge = GameObject.Find("Button_Grass");
                buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
            }

            if (m_Blocks[nRow, nCol].breed == (BlockBreed)Constants.BLOCK_LIGHT)
            {
                buttonGauge = GameObject.Find("Button_Light");
                buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
            }

            if (m_Blocks[nRow, nCol].breed == (BlockBreed)Constants.BLOCK_DARK)
            {
                buttonGauge = GameObject.Find("Button_Dark");
                buttonGauge.GetComponent<Image>().fillAmount += skillCharge;
            }
        }

        /*
         * 모든 블럭의 상태를 현재 블럭 구성 정보를 기준으로 업데이트 한다. (주로 회전 이후 블럭의 각 상태를 업데이트하기 위해 호출된다)
         * ex) 3개이상 매치된 블럭은 매치상태 설정 등
         */
        public bool UpdateAllBlocksMatchedStatus()
        {
            List<Block> matchedBlockList = new List<Block>();
            int nCount = 0;
            for (int nRow = 0; nRow < m_nRow; nRow++)
            {
                for (int nCol = 0; nCol < m_nCol; nCol++)
                {
                    if (EvalBlocksIfMatched(nRow, nCol, matchedBlockList))
                    {
                        nCount++;
                    }
                }
            }

            return nCount > 0;
        }

        /*
         * 지정된 row, col의 블럭이 Match 블럭인지 판단한다.
         * @param matchedBlockList GC 발생을 제거하기 위해 Caller에서 생성해서 전달받는다    
         */
        public bool EvalBlocksIfMatched(int nRow, int nCol, List<Block> matchedBlockList)
        {
            bool bFound = false;

            Block baseBlock = m_Blocks[nRow, nCol];
            if (baseBlock == null)
                return false;

            if (baseBlock.match != Ninez.Quest.MatchType.NONE || !baseBlock.IsValidate() || m_Cells[nRow, nCol].IsObstracle())
                return false;

            //검사하는 자신을 매칭 리스트에 우선 보관한다.
            matchedBlockList.Add(baseBlock);

            //1. 가로 블럭 검색
            Block block;

            //1.1 오른쪽 방향
            for (int i = nCol + 1; i < m_nCol; i++)
            {
                block = m_Blocks[nRow, i];
                if (!block.IsSafeEqual(baseBlock))
                    break;

                matchedBlockList.Add(block);
            }

            //1.2 왼쪽 방향
            for (int i = nCol - 1; i >= 0; i--)
            {
                block = m_Blocks[nRow, i];
                if (!block.IsSafeEqual(baseBlock))
                    break;

                matchedBlockList.Insert(0, block);
            }

            //1.3 매치된 상태인지 판단한다
            //    기준 블럭(baseBlock)을 제외하고 좌우에 2개이상이면 기준블럭 포함해서 3개이상 매치되는 경우로 판단할 수 있다
            if (matchedBlockList.Count >= 3)
            {
                SetBlockStatusMatched(matchedBlockList, true);
                bFound = true;
            }

            matchedBlockList.Clear();

            //2. 세로 블럭 검색
            matchedBlockList.Add(baseBlock);

            //2.1 위쪽 검색
            for (int i = nRow + 1; i < m_nRow; i++)
            {
                block = m_Blocks[i, nCol];
                if (!block.IsSafeEqual(baseBlock))
                    break;

                matchedBlockList.Add(block);
            }

            //2.2 아래쪽 검색
            for (int i = nRow - 1; i >= 0; i--)
            {
                block = m_Blocks[i, nCol];
                if (!block.IsSafeEqual(baseBlock))
                    break;

                matchedBlockList.Insert(0, block);
            }

            //2.3 매치된 상태인지 판단한다
            //    기준 블럭(baseBlock)을 제외하고 상하에 2개이상이면 기준블럭 포함해서 3개이상 매치되는 경우로 판단할 수 있다
            if (matchedBlockList.Count >= 3)
            {
                SetBlockStatusMatched(matchedBlockList, false);
                bFound = true;
            }

            //계산위해 리스트에 저장한 블럭 제거
            matchedBlockList.Clear();

            return bFound;
        }

        /*
         * 리스트에 포함된 전체 블럭의 상태를 MATCH로 변경한다.
         * @param bHorz 매치된 방향 false이면 세로방향, true이면 가로방향    
         */
        void SetBlockStatusMatched(List<Block> blockList, bool bHorz)
        {
            int nMatchCount = blockList.Count;
            blockList.ForEach(block => block.UpdateBlockStatusMatched((MatchType)nMatchCount, bHorz));
        }

        /*
         * 비어있는 블럭 다시 생성해서 전체 보드를 다시 구성한다
         */
        public IEnumerator SpawnBlocksAfterClean(List<Block> movingBlocks)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);//0.5초간 대기
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                for (int nRow = 0; nRow < m_nRow; nRow++)
                {
                    //비어있는 블럭이 있는 경우, 상위 열은 모두 비어있거나, 장애물로 인해서 남아있음.
                    if (m_Blocks[nRow, nCol] == null)
                    {
                        int nTopRow = nRow;
                        int nSpawnBaseY = 0;

                        for (int y = nTopRow; y < m_nRow; y++)
                        {
                            if (m_Blocks[y, nCol] != null || !CanBlockBeAllocatable(y, nCol))
                                continue;

                            Block block = SpawnBlockWithDrop(y, nCol, nSpawnBaseY, nCol);
                            if (block != null)
                                movingBlocks.Add(block);

                            nSpawnBaseY++;
                        }

                        break;
                    }
                }
            }

            yield return waitForSeconds;
        }

        /*
         * 블럭을 생성하고 목적지(nRow, nCol) 까지 드롭한다
         * @param nRow, nCol : 생성후 보드에 저장되는 위치
         * @param nSpawnedRow, nSpawnedCol : 화면에 생성되는 위치, nRow, nCol 위치까지 드롭 액션이 연출된다
         */
        Block SpawnBlockWithDrop(int nRow, int nCol, int nSpawnedRow, int nSpawnedCol)
        {
            float fInitX = CalcInitX(Core.Constants.BLOCK_ORG);
            float fInitY = CalcInitY(Core.Constants.BLOCK_ORG);

            Block block = m_StageBuilder.SpawnBlock().InstantiateBlockObj(m_BlockPrefab, m_Container);
            if (block != null)
            {
                m_Blocks[nRow, nCol] = block;
                block.Move(fInitX + nCol, fInitY + nRow);
                //block.dropDistance = new Vector2(nSpawnedCol - nCol, nSpawnedRow - nRow);
            }

            return block;
        }


        /// <summary>
        /// 퍼즐의 시작 X 위치를 구한다, left - top좌표
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public float CalcInitX(float offset = 0)
        {
            return -m_nCol / 2.0f + offset;   
        }

        //퍼즐의 시작 Y 위치, left - bottom 좌표
        //하단이 (0, 0) 이므로, 
        public float CalcInitY(float offset = 0)
        {
            return -m_nRow / 2.0f + offset;
        }

        /*
         * 지정된 위치가 셔플 가능한 조건인지 체크한다
         * @bLoading true if stage being loading , on playing is false
         */
        public bool CanShuffle(int nRow, int nCol, bool bLoading)
        {
            if (!m_Cells[nRow, nCol].type.IsBlockMovableType())
                return false;

            return true;
        }

        /*
         * Block의 종류(breed)를 변경한다.
         */
        public void ChangeBlock(Block block, BlockBreed notAllowedBreed)
        {
            BlockBreed genBreed;

            while (true)
            {
                genBreed = (BlockBreed)UnityEngine.Random.Range(0, 6); //TODO 스테이지파일에서 Spawn 정책을 이용해야함

                if (notAllowedBreed == genBreed)
                    continue;

                break;
            }

            block.breed = genBreed;
        }

        public bool IsSwipeable(int nRow, int nCol)
        {
            return m_Cells[nRow, nCol].type.IsBlockMovableType();
        }

        /*
         * 블럭이 지정된 위치에 새로 할당 될 수 있는지 체크한다
         */
        bool CanBlockBeAllocatable(int nRow, int nCol)
        {
            if (!m_Cells[nRow, nCol].type.IsBlockAllocatableType())
                return false;

            return m_Blocks[nRow, nCol] == null;
        }
    }
}