using UnityEngine;

namespace Totomaro.Core
{
    public static class Constants
    {
        public static float BLOCK_ORG = 0.5f;           //블럭의 출력 원점
        public static float SWIPE_DURATION = 0.2f;      //블럭 스와이프 애니메이션 시간
        public static float BLOCK_DESTROY_SCALE = 0.3f; //블럭이 삭제될 때 줄어드는 크기

        //상태이상 색깔
        //public static Color COLOR_FREEZE = new Color(0, 0.75f, 1, 1);
        //public static Color COLOR_BIND = new Color(0, 1, 0.2f, 1);


        public static Color COLOR_BEFORESPAWN = new Color(0, 0, 0, 1);
        public static Color COLOR_BASIC = new Color(1, 1, 1, 1);
        public static Color COLOR_BURN = new Color(1, 0, 0, 1);
        public static Color COLOR_FREEZE = new Color(0, 0.5f, 1, 1);
        public static Color COLOR_BIND = new Color(0, 1, 0, 1);
        public static Color COLOR_FEAR = new Color(0.5f, 0, 1, 1);
        public static Color COLOR_LIGHT = new Color(1, 1, 0, 1);

        //몬스터 종류
        public static int BASIC_MONSTER = 0;
        public static int FIRE_MONSTER = 1;
        public static int ICE_MONSTER = 2;
        public static int GRASS_MONSTER = 3;
        public static int LIGHT_MONSTER = 4;
        public static int DARK_MONSTER = 5;
        public static int BOSS_MONSTER = 6;
        public static int FREEZE_MONSTER = 7;
        public static int BIND_MONSTER = 8;
        public static int FEAR_MONSTER = 9;
        public static int X_MONSTER = 10;
        public static int O_MONSTER = 11;

        //플레이어 무브 표시
        public static int DIRECTION_LEFT = 2;
        public static int DIRECTION_RIGHT = 0;
        public static int DIRECTION_UP = 1;
        public static int DIRECTION_DOWN = 3;

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

        //총알 종류
        public static float BULLET_FIRE = 0.01f;
        public static float BULLET_ICE = 0.02f;
        public static float BULLET_GRASS = 0.03f;
        public static float BULLET_LIGHT = 0.04f;
        public static float BULLET_DARK = 0.05f;

        //난이도
        public static int LEVEL_EASY = 0;
        public static int LEVEL_NORMAL = 1;
        public static int LEVEL_HARD = 2;
        public static int LEVEL_HOT = 3;

        //스킬포션
        public static float SKILLPORTION_FIRE = 0.01f;
        public static float SKILLPORTION_ICE = 0.02f;
        public static float SKILLPORTION_GRASS = 0.03f;
        public static float SKILLPORTION_LIGHT = 0.04f;
        public static float SKILLPORTION_DARK = 0.05f;
    }
}