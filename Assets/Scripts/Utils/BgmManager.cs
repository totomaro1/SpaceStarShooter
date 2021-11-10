using Ninez.Board;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninez.Util
{
    public class BgmManager : MonoBehaviour
    {
        public AudioSource efxSource;
        public static BgmManager instance;
        public static AudioSource[] sfx;

        /**
         * 시작시 컴포넌트를 구해서 singleton으로 저장한다.
         * 어디에서나 SoundManager.PlayOnShot()으로 사운드를 플레이할 수 있다.
         */
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void StopPlaying()
        {
            Destroy(gameObject);
        }
    }
}

