using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ninez.Quest;
using Ninez.Util;
using Ninez.Stage;
using Ninez.Core;

namespace Ninez.Board
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

        public GameObject preMonsterPrefab;

        GameObject monster;
        GameObject preMonster;

        public static GameObject[,] monsters = new GameObject[9, 9];
        public static GameObject[,] preMonsters = new GameObject[9, 9];

        //spawnSpan -> 몬스터의 스폰 간격을 지정
        //RegenTimeDown -> 몬스터의 스폰 간격을 점차 줄임
        public static float spawnSpan = 1f;
        public static float regenTime = 0f;
        public static int randomSpawn = 15;
        float spawnDelta = 0;
        float phaseSpan = 60f;
        float phaseDelta = 0;


        int randomx = 0;
        int randomy = 0;
        int randomMonster = 0;

        public static int countMonster = 0;
        public static int currentMonster = 0;
        public Text currentMonsterText;

        public static bool limitMode = false;
        int modeCorrection = 2;

        private void Start()
        {
            randomx = Random.Range(-4, 5);
            randomy = Random.Range(-4, 5);
            randomMonster = Random.Range(0, randomSpawn);
            preMonster = Object.Instantiate(preMonsterPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
            preMonsters[randomy + 4, randomx + 4] = preMonster;

            if(limitMode) modeCorrection = 1;
            else modeCorrection = 0;
        }

        void Update()
        {
            this.spawnDelta += Time.deltaTime / 100 * (100 - currentMonster * modeCorrection);
            this.phaseDelta += Time.deltaTime;
            //몬스터를 주기적으로 필드 상의 랜덤한 위치에 스폰시킨다.
            if (this.spawnDelta > spawnSpan)
            {
                this.spawnDelta = 0;

                if (randomMonster == 0)
                {
                    monster = Object.Instantiate(monsterFirePrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.transform.localScale = new Vector3(1f, 1f, Constants.FIRE_MONSTER);
                }
                else if (randomMonster == 1)
                {
                    monster = Object.Instantiate(monsterIcePrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.transform.localScale = new Vector3(1f, 1f, Constants.ICE_MONSTER);
                }
                else if (randomMonster == 2)
                {
                    monster = Object.Instantiate(monsterGrassPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.transform.localScale = new Vector3(1f, 1f, Constants.GRASS_MONSTER);
                }
                else if (randomMonster == 3)
                {
                    monster = Object.Instantiate(monsterLightPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.transform.localScale = new Vector3(1f, 1f, Constants.LIGHT_MONSTER);
                }
                else if (randomMonster == 4)
                {
                    monster = Object.Instantiate(monsterDarkPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.transform.localScale = new Vector3(1f, 1f, Constants.DARK_MONSTER);
                }
                else
                {
                    monster = Object.Instantiate(monsterPrefab, new Vector3(randomx, randomy, 0), Quaternion.identity);
                    monster.transform.localScale = new Vector3(1f, 1f, Constants.NORMAL_MONSTER);
                }
                monsters[randomy + 4, randomx + 4] = monster;

                preMonsters[randomy + 4, randomx + 4] = null;
                do
                {
                    randomx = Random.Range(-4, 5);
                    randomy = Random.Range(-4, 5);
                }
                while (monsters[randomy + 4, randomx + 4] != null);

                randomMonster = Random.Range(0, randomSpawn);
                preMonster.transform.position = new Vector3(randomx, randomy, 0);
                preMonsters[randomy + 4, randomx + 4] = preMonster;

                
            }

            //스폰 간격 감소
            if (this.phaseDelta > this.phaseSpan)
            {
                this.phaseDelta = 0;
                if (spawnSpan - regenTime > 1f)
                {
                    spawnSpan -= regenTime;
                }
                else spawnSpan = 1f;
            }

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
            if(limitMode) currentMonsterText.text = currentMonster + " / 20";
            else currentMonsterText.text = "" + currentMonster;
            countMonster = 0;
        }
    }
}