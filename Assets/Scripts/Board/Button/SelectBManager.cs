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
        public Text backgroundText; //ĳ���� ��� �ؽ�Ʈ
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

            characterNameText.text = "���� �ΰ�";
            backgroundText.text = "�ɾ�鼭 ����Ʈ���� �ϴٰ� �����뿡 �Ӹ��� �ڰ� ���� �̵��� ������ ����� ����Դϴ�.";
            characterText.text = "��� ��ų ������ �䱸���� 20% �����մϴ�.";
        }


        public void ChracterClickFire()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.FIRE_CHARACTER;

            characterNameText.text = "������";
            backgroundText.text = "�� �ӿ� ȭ�� ���� ����ֽ��ϴ�.\n��ġ�⸸ �ص� ���� ���߸� ���Դϴ�.";
            characterText.text = "�� �Ӽ� ��ų�� ������ 1ĭ �����մϴ�.";
        }

        public void ChracterClickIce()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.ICE_CHARACTER;

            characterNameText.text = "������";
            backgroundText.text = "�ǵ� ������ �����ϴ�.\n������ �ڱ� ������Դ� �����ϰ���.";
            characterText.text = "������ ���ӽð��� 2�� ����������\n���� ������ Ǯ���� ���ŵ˴ϴ�.";
        }

        public void ChracterClickGrass()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.GRASS_CHARACTER;

            characterNameText.text = "�ڿ���";
            backgroundText.text = "ȥ���� ���� ��Ȱ�� ��� ������ �����ϴ�.\n�������� ������� ��ƴ� �뱸�� ����� �ϴ°� ����Դϴ�.";
            characterText.text = "�ӹ��� ���ӽð��� 2�� �����մϴ�.";
        }

        public void ChracterClickLight()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.LIGHT_CHARACTER;

            characterNameText.text = "���̺� ����";
            backgroundText.text = "�����ڿ����� ������ Ÿ���߽��ϴ�.\n����� �߸������� �Ƿ��� ��¥�Դϴ�.";
            characterText.text = "�� �Ӽ� ��ų�� ȸ������ 2�� ���˴ϴ�.";
        }

        public void ChracterClickDark()
        {
            SoundManager.instance.PlayOneShot(Clip.Chomp);
            CharacterSelect = Constants.DARK_CHARACTER;

            characterNameText.text = "�汸�� ����";
            backgroundText.text = "�� ���°� �λ��� �����Դϴ�.\n����ö���� ������ ���� Ÿ�ڼӵ��� �����մϴ�.";
            characterText.text = "������ ���ӽð��� 2�� �����մϴ�.";
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

