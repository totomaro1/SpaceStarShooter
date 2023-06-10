using Totomaro.Board;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Totomaro.Core;

namespace Totomaro.Util
{
    public class PlayBgmManager : MonoBehaviour
    {
        public static SoundManager instance;
        public AudioSource[] sfx;

        /**
         * 시작시 컴포넌트를 구해서 singleton으로 저장한다.
         * 어디에서나 SoundManager.PlayOnShot()으로 사운드를 플레이할 수 있다.
         */
        void Start()
        {
            instance = GetComponent<SoundManager>();
            sfx = GetComponents<AudioSource>();

            if (SelectBManager.CharacterSelect == Constants.BASIC_CHARACTER) sfx[0].Play();
            if (SelectBManager.CharacterSelect == Constants.FIRE_CHARACTER) sfx[1].Play();
            if (SelectBManager.CharacterSelect == Constants.ICE_CHARACTER) sfx[2].Play();
            if (SelectBManager.CharacterSelect == Constants.GRASS_CHARACTER) sfx[3].Play();
            if (SelectBManager.CharacterSelect == Constants.LIGHT_CHARACTER) sfx[4].Play();
            if (SelectBManager.CharacterSelect == Constants.DARK_CHARACTER) sfx[5].Play();
        }
    }
}

