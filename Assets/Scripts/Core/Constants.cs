using UnityEngine;

namespace Ninez.Core
{
    public static class Constants
    {
        public static float BLOCK_ORG = 0.5f;           //블럭의 출력 원점
        public static float SWIPE_DURATION = 0.2f;      //블럭 스와이프 애니메이션 시간
        public static float BLOCK_DESTROY_SCALE = 0.3f; //블럭이 삭제될 때 줄어드는 크기

        //상태이상 색깔
        public static Color MONSTER_BEFORESPAWN = Color.black;
        public static Color MONSTER_NORMAL = Color.white;
        public static Color MONSTER_FIRE = Color.red;
        public static Color MONSTER_FREEZE = new Color(0, 0.75f, 1, 1);
        public static Color MONSTER_BIND = new Color(0, 1, 0.2f, 1);
        public static Color MONSTER_FEAR = new Color(0.4f, 0, 1, 1);

        //몬스터 종류
        public static float NORMAL_MONSTER = 0f;
        public static float FIRE_MONSTER = 0.01f;
        public static float ICE_MONSTER = 0.02f;
        public static float GRASS_MONSTER = 0.03f;
        public static float LIGHT_MONSTER = 0.04f;
        public static float DARK_MONSTER = 0.05f;
        public static float X_MONSTER = 0.06f;
        public static float O_MONSTER = 0.07f;

        //플레이어 무브 표시
        public static int PLAYERMOVE_LEFT = 2;
        public static int PLAYERMOVE_RIGHT = 0;
        public static int PLAYERMOVE_UP = 1;
        public static int PLAYERMOVE_DOWN = 3;

        //Block.Breed 표시
        public static int BLOCK_FIRE = 0;
        public static int BLOCK_ICE = 1;
        public static int BLOCK_GRASS = 2;
        public static int BLOCK_LIGHT = 3;
        public static int BLOCK_DARK = 4;

        //캐릭터 선택
        public static int BASIC_CHARACTER = 0;
        public static int FIRE_CHARACTER = 1;
        public static int ICE_CHARACTER = 2;
        public static int GRASS_CHARACTER = 3;
        public static int LIGHT_CHARACTER = 4;
        public static int DARK_CHARACTER = 5;

        
    }
}