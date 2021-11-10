using Ninez.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ninez.Core;

namespace Ninez.Board
{
    public class SelectManager : MonoBehaviour
    {

        public Text characterNameText;
        public Text backgroundText; //캐릭터 배경 텍스트
        public Text characterText;

        public Sprite fireCharacterImage;
        public Sprite iceCharacterImage;
        public Sprite grassCharacterImage;
        public Sprite lightCharacterImage;
        public Sprite darkCharacterImage;
        public Sprite basicCharacterImage;

        public SpriteRenderer basicSpriteRenderer;
        public SpriteRenderer fireSpriteRenderer;
        public SpriteRenderer iceSpriteRenderer;
        public SpriteRenderer grassSpriteRenderer;
        public SpriteRenderer lightSpriteRenderer;
        public SpriteRenderer darkSpriteRenderer;

        public Image characterImage;

        public bool fireCharacter = false;
        public bool iceCharacter = false;
        public bool grassCharacter = false;
        public bool lightCharacter = false;
        public bool darkCharacter = false;
        public bool basicCharacter = false;

        void Start()
        {
            SelectBManager ui = GameObject.FindGameObjectWithTag("ManagerUi").GetComponent<SelectBManager>();
            ui.init();
        }

        void Update()
        {
            if (SelectBManager.CharacterSelect == Constants.BASIC_CHARACTER)
            {
                characterNameText.text = "전생 인간";
                backgroundText.text = "걸어가면서 스마트폰을 하다가 전봇대에 머리를 박고 차원 이동된 지극히 평범한 사람입니다.";
                characterText.text = "모든 스킬 게이지 요구량이 20% 감소합니다.";
                characterImage.sprite = basicSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.FIRE_CHARACTER)
            {
                characterNameText.text = "다혈질";
                backgroundText.text = "몸 속에 화가 가득 담겨있습니다.\n스치기만 해도 뼈도 못추릴 것입니다.";
                characterText.text = "불 속성 스킬의 범위가 1칸 증가합니다.";
                characterImage.sprite = fireSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.ICE_CHARACTER)
            {
                characterNameText.text = "냉혈한";
                backgroundText.text = "피도 눈물도 없습니다.\n하지만 자기 사람에게는 따뜻하겠지.";
                characterText.text = "빙결의 지속시간이 2배 증가하지만\n적의 빙결이 풀리면 제거됩니다.";
                characterImage.sprite = iceSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.GRASS_CHARACTER)
            {
                characterNameText.text = "자연인";
                backgroundText.text = "혼잡한 도시 생활을 벗어나 산으로 갔습니다.\n지나가는 멧돼지를 잡아다 통구이 방송을 하는게 취미입니다.";
                characterText.text = "속박의 지속시간이 3배 증가합니다.";
                characterImage.sprite = grassSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.LIGHT_CHARACTER)
            {
                characterNameText.text = "사이비 교주";
                backgroundText.text = "성직자였으나 가난에 타락했습니다.\n방식은 잘못됐지만 실력은 진짜입니다.";
                characterText.text = "빛 속성 스킬의 회복력이 2배 향상됩니다.";
                characterImage.sprite = lightSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.DARK_CHARACTER)
            {
                characterNameText.text = "방구석 여포";
                backgroundText.text = "글 쓰는게 인생의 전부입니다.\n개똥철학을 빛보다 빠른 타자속도로 전파합니다.";
                characterText.text = "공포의 지속시간이 2배 증가합니다.";
                characterImage.sprite = darkSpriteRenderer.sprite;
            }
        }
    }
}
