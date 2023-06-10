using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Totomaro.Stage;
using Totomaro.Core;
using Totomaro.Util;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Totomaro.Board
{
    public class SelectBManager : MonoBehaviour
    {
        GameObject select;

        public static int CharacterSelect;

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

        public void init()
        {
            select = GameObject.Find("SelectManager");
            CharacterSelect = Constants.BASIC_CHARACTER;
        }

        public void CharacterClickBasic()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.BASIC_CHARACTER;

            characterNameText.text = "전생 인간";
            backgroundText.text = "걸어가면서 스마트폰을 하다가 전봇대에 머리를 박고 차원 이동된 지극히 평범한 사람입니다.";
            characterText.text = "모든 스킬 게이지 요구량이 20% 감소합니다.";
        }


        public void ChracterClickFire()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.FIRE_CHARACTER;

            characterNameText.text = "다혈질";
            backgroundText.text = "몸 속에 화가 가득 담겨있습니다.\n스치기만 해도 뼈도 못추릴 것입니다.";
            characterText.text = "불 속성 스킬의 범위가 1칸 증가합니다.";
        }

        public void ChracterClickIce()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.ICE_CHARACTER;

            characterNameText.text = "냉혈한";
            backgroundText.text = "피도 눈물도 없습니다.\n하지만 자기 사람에게는 따뜻하겠지.";
            characterText.text = "빙결의 지속시간이 2배 증가하지만\n적의 빙결이 풀리면 제거됩니다.";
        }

        public void ChracterClickGrass()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.GRASS_CHARACTER;

            characterNameText.text = "자연인";
            backgroundText.text = "혼잡한 도시 생활을 벗어나 산으로 갔습니다.\n지나가는 멧돼지를 잡아다 통구이 방송을 하는게 취미입니다.";
            characterText.text = "속박의 지속시간이 2배 증가합니다.";
        }

        public void ChracterClickLight()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.LIGHT_CHARACTER;

            characterNameText.text = "사이비 교주";
            backgroundText.text = "성직자였으나 가난에 타락했습니다.\n방식이 잘못됐지만 실력은 진짜입니다.";
            characterText.text = "빛 속성 스킬의 회복력이 2배 향상됩니다.";
        }

        public void ChracterClickDark()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.DARK_CHARACTER;

            characterNameText.text = "방구석 여포";
            backgroundText.text = "글 쓰는게 인생의 전부입니다.\n개똥철학을 빛보다 빠른 타자속도로 전파합니다.";
            characterText.text = "공포의 지속시간이 2배 증가합니다.";
        }


        public void GoButton()
        {
            SceneManager.LoadScene("LevelSelectScene");
        }

        public void BackButton()
        {
            SceneManager.LoadScene("MenuScene");
        }

        void Update()
        {
            if (CharacterSelect == Constants.BASIC_CHARACTER)
            {
                characterImage.sprite = basicSpriteRenderer.sprite;
            }

            if (CharacterSelect == Constants.FIRE_CHARACTER)
            {
                characterImage.sprite = fireSpriteRenderer.sprite;
            }

            if (CharacterSelect == Constants.ICE_CHARACTER)
            {
                characterImage.sprite = iceSpriteRenderer.sprite;
            }

            if (CharacterSelect == Constants.GRASS_CHARACTER)
            {
                characterImage.sprite = grassSpriteRenderer.sprite;
            }

            if (CharacterSelect == Constants.LIGHT_CHARACTER)
            {
                characterImage.sprite = lightSpriteRenderer.sprite;
            }

            if (CharacterSelect == Constants.DARK_CHARACTER)
            {
                characterImage.sprite = darkSpriteRenderer.sprite;
            }
        }
    }
}

