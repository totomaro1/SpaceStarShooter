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

        void Start()
        {
            SelectBManager ui = GameObject.FindGameObjectWithTag("ManagerUi").GetComponent<SelectBManager>();
            ui.init();
        }

        void Update()
        {
            if (SelectBManager.CharacterSelect == Constants.BASIC_CHARACTER)
            {
                characterNameText.text = "���� �ΰ�";
                backgroundText.text = "�ɾ�鼭 ����Ʈ���� �ϴٰ� �����뿡 �Ӹ��� �ڰ� ���� �̵��� ������ ����� ����Դϴ�.";
                characterText.text = "��� ��ų ������ �䱸���� 20% �����մϴ�.";
                characterImage.sprite = basicSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.FIRE_CHARACTER)
            {
                characterNameText.text = "������";
                backgroundText.text = "�� �ӿ� ȭ�� ���� ����ֽ��ϴ�.\n��ġ�⸸ �ص� ���� ���߸� ���Դϴ�.";
                characterText.text = "�� �Ӽ� ��ų�� ������ 1ĭ �����մϴ�.";
                characterImage.sprite = fireSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.ICE_CHARACTER)
            {
                characterNameText.text = "������";
                backgroundText.text = "�ǵ� ������ �����ϴ�.\n������ �ڱ� ������Դ� �����ϰ���.";
                characterText.text = "������ ���ӽð��� 2�� ����������\n���� ������ Ǯ���� ���ŵ˴ϴ�.";
                characterImage.sprite = iceSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.GRASS_CHARACTER)
            {
                characterNameText.text = "�ڿ���";
                backgroundText.text = "ȥ���� ���� ��Ȱ�� ��� ������ �����ϴ�.\n�������� ������� ��ƴ� �뱸�� ����� �ϴ°� ����Դϴ�.";
                characterText.text = "�ӹ��� ���ӽð��� 3�� �����մϴ�.";
                characterImage.sprite = grassSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.LIGHT_CHARACTER)
            {
                characterNameText.text = "���̺� ����";
                backgroundText.text = "�����ڿ����� ������ Ÿ���߽��ϴ�.\n����� �߸������� �Ƿ��� ��¥�Դϴ�.";
                characterText.text = "�� �Ӽ� ��ų�� ȸ������ 2�� ���˴ϴ�.";
                characterImage.sprite = lightSpriteRenderer.sprite;
            }

            if (SelectBManager.CharacterSelect == Constants.DARK_CHARACTER)
            {
                characterNameText.text = "�汸�� ����";
                backgroundText.text = "�� ���°� �λ��� �����Դϴ�.\n����ö���� ������ ���� Ÿ�ڼӵ��� �����մϴ�.";
                characterText.text = "������ ���ӽð��� 2�� �����մϴ�.";
                characterImage.sprite = darkSpriteRenderer.sprite;
            }
        }
    }
}
