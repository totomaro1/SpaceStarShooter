using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Totomaro.Quest;
using Totomaro.Util;
using Totomaro.Stage;
using Totomaro.Core;

namespace Totomaro.Board
{
    public class PlayerGenerator : MonoBehaviour
    {
        Transform playerContainer;
        public GameObject BasicPlayerPrefab;
        public GameObject FirePlayerPrefab;
        public GameObject IcePlayerPrefab;
        public GameObject GrassPlayerPrefab;
        public GameObject LightPlayerPrefab;
        public GameObject DarkPlayerPrefab;

        public GameObject PlayerEffectPrefab;

        public static GameObject[,] players = new GameObject[9, 9];

        public static int playerx = 0;
        public static int playery = 0;

        //플레이어의 스폰 위치를 지정하고 생성
        private void Start()
        {
            if (SelectBManager.CharacterSelect == Constants.FIRE_CHARACTER)
            {
                GameObject player = Object.Instantiate(FirePlayerPrefab, new Vector3(playerx, playery, 0), Quaternion.identity);
                PlayerSetting(player);
            }
            else if (SelectBManager.CharacterSelect == Constants.ICE_CHARACTER)
            {
                GameObject player = Object.Instantiate(IcePlayerPrefab, new Vector3(playerx, playery, 0), Quaternion.identity);
                PlayerSetting(player);
            }
            else if (SelectBManager.CharacterSelect == Constants.GRASS_CHARACTER)
            {
                GameObject player = Object.Instantiate(GrassPlayerPrefab, new Vector3(playerx, playery, 0), Quaternion.identity);
                PlayerSetting(player);
            }
            else if (SelectBManager.CharacterSelect == Constants.LIGHT_CHARACTER)
            {
                GameObject player = Object.Instantiate(LightPlayerPrefab, new Vector3(playerx, playery, 0), Quaternion.identity);
                PlayerSetting(player);
            }
            else if (SelectBManager.CharacterSelect == Constants.DARK_CHARACTER)
            {
                GameObject player = Object.Instantiate(DarkPlayerPrefab, new Vector3(playerx, playery, 0), Quaternion.identity);
                PlayerSetting(player);
            }
            else
            {
                GameObject player = Object.Instantiate(BasicPlayerPrefab, new Vector3(playerx, playery, 0), Quaternion.identity);
                PlayerSetting(player);
            }
        }

        void PlayerSetting(GameObject player)
        {
            //GameObject playerEffect = Object.Instantiate(PlayerEffectPrefab, new Vector3(playerx, playery, 0), Quaternion.identity);
            players[playerx, playery] = player;
            player.transform.localScale = new Vector3(1, 1, 1);
            PlayerBehaviour.currentTransformScale = player.transform.localScale;
        }
    }
}
