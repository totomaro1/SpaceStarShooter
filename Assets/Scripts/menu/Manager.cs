using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ninez.Board
{
    public class Manager : MonoBehaviour
    {
        public static Manager instance; //��𼭳� ������ �� �ֵ��� static(����)���� �ڱ� �ڽ��� ������ ������ ����ϴ�.
        public Text ResultScoreText; //������ ǥ���ϴ� Text��ü�� �����Ϳ��� �޾ƿɴϴ�.
        public Text CurrentScoreText; //������ ǥ���ϴ� Text��ü�� �����Ϳ��� �޾ƿɴϴ�.
        private int score = 0; //������ �����մϴ�.
        void Awake()
        {
            if (!instance) //�������� �ڽ��� üũ�մϴ�.
                instance = this; //�������� �ڽ��� �����մϴ�.
        }

        public void AddScore(int num) //������ �߰����ִ� �Լ��� ����� �ݴϴ�.
        {
            score += num; //������ �����ݴϴ�.
            ResultScoreText.text = "Score : " + score; //�ؽ�Ʈ�� �ݿ��մϴ�.
            CurrentScoreText.text = "" + score;
            //�ؽ�Ʈ�� �ݿ��մϴ�.
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}
