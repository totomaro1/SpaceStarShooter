using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Totomaro.Quest;
using Totomaro.Util;
using Totomaro.Stage;
using Totomaro.Core;

namespace Totomaro.Board
{
    public class MonsterGenerator : MonoBehaviour
    {
        Transform monsterContainer;

        public GameObject monsterPrefab;
        public GameObject monsterFirePrefab;
        public GameObject monsterIcePrefab;
        public GameObject monsterGrassPrefab;
        public GameObject monsterLightPrefab;
        public GameObject monsterDarkPrefab;
        public GameObject monsterBossPrefab;

        public GameObject preMonsterPrefab;

        GameObject monster;
        GameObject preMonster;

        public static GameObject[,] monsters = new GameObject[9, 9];
        public static GameObject[,] preMonsters = new GameObject[9, 9];

        //spawnSpan -> 몬스터의 스폰 간격을 지정
        //RegenTimeDown -> 몬스터의 스폰 간격을 점차 줄임
        public static float spawnSpan = 3f;
        public static float regenTime = 0.8f;

        int fireMonsterRange = 0;
        int fire_Ice_MonsterRange = 15;
        int Ice_Grass_MonsterRange = 30;
        int Grass_Light_MonsterRange = 45;
        int Light_Dark_MonsterRange = 60;
        int Dark_Boss_MonsterRange = 75;
        int Boss_MonsterRange = 85;

        float spawnDelta = 0;
        float phaseSpan = 60f;
        float phaseDelta = 0;

        int randomx = 0;
        int randomy = 0;
        int randomMonster = 0;

        public static int countMonster = 0;
        public static int currentMonster = 0;
        public Text currentMonsterText;

        public static bool limitMode = true;
        public static bool noRegen = false;
        int modeCorrection = 0;


        private void Start()
        {
            //LevelBManager.curLevelSelect = Constants.LEVEL_HOT;

            randomx = Random.Range(-4, 5);
            randomy = Random.Range(-4, 5);
            randomMonster = Random.Range(0, 100);
            preMonster = Object.Instantiate(preMonsterPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
            preMonsters[randomy + 4, randomx + 4] = preMonster;

            if(limitMode) modeCorrection = 3;
            else modeCorrection = 2;

            if(LevelBManager.curLevelSelect == Constants.LEVEL_EASY)
            {
                spawnSpan = 6f;
                regenTime = 0.8f;

                fireMonsterRange = 0;
                fire_Ice_MonsterRange = 2;
                Ice_Grass_MonsterRange = 4;
                Grass_Light_MonsterRange = 6;
                Light_Dark_MonsterRange = 8;
                Dark_Boss_MonsterRange = 10;
                Boss_MonsterRange = 10;

                MonsterBehaviour.span = 4f;
            }

            if (LevelBManager.curLevelSelect == Constants.LEVEL_NORMAL)
            {
                spawnSpan = 3f;
                regenTime = 0.8f;

                fireMonsterRange = 0;
                fire_Ice_MonsterRange = 5;
                Ice_Grass_MonsterRange = 10;
                Grass_Light_MonsterRange = 15;
                Light_Dark_MonsterRange = 20;
                Dark_Boss_MonsterRange = 25;
                Boss_MonsterRange = 25;

                MonsterBehaviour.span = 3f;
            }

            if (LevelBManager.curLevelSelect == Constants.LEVEL_HARD)
            {
                spawnSpan = 1.5f;
                regenTime = 0.8f;

                fireMonsterRange = 0;
                fire_Ice_MonsterRange = 10;
                Ice_Grass_MonsterRange = 20;
                Grass_Light_MonsterRange = 30;
                Light_Dark_MonsterRange = 40;
                Dark_Boss_MonsterRange = 50;
                Boss_MonsterRange = 55;

                MonsterBehaviour.span = 2f;
            }

            if (LevelBManager.curLevelSelect == Constants.LEVEL_HOT)
            {
                spawnSpan = 0.7f;
                regenTime = 0.8f;

                fireMonsterRange = 0;
                fire_Ice_MonsterRange = 15;
                Ice_Grass_MonsterRange = 30;
                Grass_Light_MonsterRange = 45;
                Light_Dark_MonsterRange = 60;
                Dark_Boss_MonsterRange = 75;
                Boss_MonsterRange = 85;

                MonsterBehaviour.span = 1.5f;
            }
        }

        void Update()
        {
            this.spawnDelta += Time.deltaTime / 100 * (100 - currentMonster * modeCorrection);
            this.phaseDelta += Time.deltaTime;
            //몬스터를 주기적으로 필드 상의 랜덤한 위치에 스폰시킨다.
            if (this.spawnDelta > spawnSpan && noRegen == false)
            {
                this.spawnDelta = 0;

                if (fireMonsterRange <= randomMonster && randomMonster < fire_Ice_MonsterRange)
                {
                    monster = Object.Instantiate(monsterFirePrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.GetComponent<MonsterBehaviour>().monsterType = Constants.FIRE_MONSTER;
                    monster.GetComponent<MonsterBehaviour>().monsterTypeCurrent = Constants.FIRE_MONSTER;
                }
                else if (fire_Ice_MonsterRange <= randomMonster && randomMonster < Ice_Grass_MonsterRange)
                {
                    monster = Object.Instantiate(monsterIcePrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.GetComponent<MonsterBehaviour>().monsterType = Constants.ICE_MONSTER;
                    monster.GetComponent<MonsterBehaviour>().monsterTypeCurrent = Constants.ICE_MONSTER;
                }
                else if (Ice_Grass_MonsterRange <= randomMonster && randomMonster < Grass_Light_MonsterRange)
                {
                    monster = Object.Instantiate(monsterGrassPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.GetComponent<MonsterBehaviour>().monsterType = Constants.GRASS_MONSTER;
                    monster.GetComponent<MonsterBehaviour>().monsterTypeCurrent = Constants.GRASS_MONSTER;
                }
                else if (Grass_Light_MonsterRange <= randomMonster && randomMonster < Light_Dark_MonsterRange)
                {
                    monster = Object.Instantiate(monsterLightPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.GetComponent<MonsterBehaviour>().monsterType = Constants.LIGHT_MONSTER;
                    monster.GetComponent<MonsterBehaviour>().monsterTypeCurrent = Constants.LIGHT_MONSTER;
                }
                else if (Light_Dark_MonsterRange <= randomMonster && randomMonster < Dark_Boss_MonsterRange)
                {
                    monster = Object.Instantiate(monsterDarkPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.GetComponent<MonsterBehaviour>().monsterType = Constants.DARK_MONSTER;
                    monster.GetComponent<MonsterBehaviour>().monsterTypeCurrent = Constants.DARK_MONSTER;
                }
                else if (Dark_Boss_MonsterRange <= randomMonster && randomMonster < Boss_MonsterRange)
                {
                    monster = Object.Instantiate(monsterBossPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.GetComponent<MonsterBehaviour>().monsterType = Constants.BOSS_MONSTER;
                    monster.GetComponent<MonsterBehaviour>().monsterTypeCurrent = Constants.BOSS_MONSTER;
                }
                else
                {
                    monster = Object.Instantiate(monsterPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.GetComponent<MonsterBehaviour>().monsterType = Constants.BASIC_MONSTER;
                    monster.GetComponent<MonsterBehaviour>().monsterTypeCurrent = Constants.BASIC_MONSTER;
                }
                monsters[randomy + 4, randomx + 4] = monster;

                preMonsters[randomy + 4, randomx + 4] = null;

                do
                {
                    randomx = Random.Range(-4, 5);
                    randomy = Random.Range(-4, 5);
                }
                while (monsters[randomy + 4, randomx + 4] != null);

                randomMonster = Random.Range(0, 100);
                preMonster.transform.position = new Vector3(randomx, randomy, 0);
                preMonsters[randomy + 4, randomx + 4] = preMonster;
            }

            //스폰 간격 감소
            if (this.phaseDelta > this.phaseSpan)
            {
                this.phaseDelta = 0;
                spawnSpan *= regenTime;
            }

            //몬스터 숫자 계산
            for(int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if(monsters[i, j] != null)
                    {
                        countMonster += 1;
                    }
                }
            }

            //현재몬스터계산
            currentMonster = countMonster;
            if(limitMode) currentMonsterText.text = currentMonster + " / 30";
            else currentMonsterText.text = currentMonster + " / 80";
            countMonster = 0;
        }
    }
}