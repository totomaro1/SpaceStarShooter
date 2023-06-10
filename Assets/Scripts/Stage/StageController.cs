using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Totomaro.Util;
using Totomaro.Board;

namespace Totomaro.Stage
{
    public class StageController : MonoBehaviour
    {
        bool m_bInit;
        Stage m_Stage;
        InputManager m_InputManager;
        ActionManager m_ActionManager;
        PlayerBehaviour m_player;

        GameObject buttonManager;
        ButtonManager buttonScript;

        public bool inputKick = false;
        public bool inputMove = false;
        public bool kickPosition = false;

        public int direction = 0;
        public static int p_col = 4;
        public static int p_row = 4;
        public static int col = 0;
        public static int row = 0;
        //Event Members
        bool m_bTouchDown;          //입력상태 처리 플래그, 유효한 블럭을 클릭한 경우 true
        BlockPos m_BlockDownPos;    //블럭 인덱스 (보드에 저장된 위치)
        Vector3 m_ClickPos;         //DOWN 위치(보드 기준 Local 좌표)


        [SerializeField] Transform m_Container;
        [SerializeField] GameObject m_CellPrefab;
        [SerializeField] GameObject m_BlockPrefab;
        [SerializeField] GameObject m_MonsterPrefab;
        [SerializeField] GameObject m_PlayerPrefab;

        void Start()
        {
            buttonManager = GameObject.Find("ButtonManager");
            buttonScript = buttonManager.GetComponent<ButtonManager>();

            InitStage();
        }

        private void Update()
        {
            if (!m_bInit)
                return;

            OnInputHandler();
        }

        void InitStage()
        {
            if (m_bInit)
                return;

            m_bInit = true;
            m_InputManager = new InputManager(m_Container);

            BuildStage();
        }

        /*
         * 스테이지를 구성한다.
         * Stage 객체를 할당받고, Stage 구성을 요청한다.
         */
        void BuildStage()
        {
            //1. Stage를 구성한다.
            m_Stage = StageBuilder.BuildStage(nStage: 1);

            m_ActionManager = new ActionManager(m_Container, m_Stage);

            //2. 생성한 stage 정보를 이용하여 씬을 구성한.
            m_Stage.ComposeStage(m_CellPrefab, m_BlockPrefab, m_Container);
        }


        public int playerCol;
        public int playerRow;
        public int direct;
        public bool inputSkill;

        public Swipe ChangeLoc(int direction)
        {
            switch (direction)
            {
                case 0: return Swipe.RIGHT;
                case 1: return Swipe.UP;
                case 2: return Swipe.LEFT;
                case 3: return Swipe.DOWN;
                default: return 0;
            }
        }
        void OnInputHandler()
        {
            //1. Touch Down 
            if (inputKick)
            {
                m_ActionManager.DoSwipeAction(p_row, p_col, ChangeLoc(direction));

                inputKick = false;
                ButtonManager.callKickCommand = false;
            }
            
            if (inputSkill)
            {
                m_ActionManager.DoSkillAction(p_row, p_col);
            }

            if(ButtonManager.callKickCommand == true)
            {
                kickPosition = true;
            }
        }
    }
}
