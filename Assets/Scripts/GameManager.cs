using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;        // �÷��̾� ������ ���� Ȱ��ȭ ����

    public static bool isOpenInventory = false;     // �κ��丮 Ȱ��ȭ ����
    public static bool isOpenCraftManual = false;   // ���� �޴�â Ȱ��ȭ ����
    public static bool isOpenArchemyTable = false;  // ���� ���̺� â Ȱ��ȭ ����
    public static bool isOnComputer = false;     // ��ǻ�� â Ȱ��ȭ ����

    public static bool isNight = false;         // ������ �������� ���� ����
    public static bool isWater = false;         // ���������� ���� ����

    public static bool isPause = false;         // ����޴� Ȱ��ȭ ����

    private WeaponManager theWM;
    private bool flag = false;      // ���ӿ��� ��� ����ִ����� ���� ����

    void Update()
    {
        // �κ��丮 Ȱ��ȭ��, ��ǻ�� ���� On��, ���� �޴�â Ȱ��ȭ��, ���� ���̺� â Ȱ��ȭ��, ����޴� Ȱ��ȭ��
        if (isOpenInventory || isOnComputer || isOpenCraftManual || isOpenArchemyTable || isPause)
        {
            // ���콺 Ŀ�� �������, Cursor.visible = true �� ����
            Cursor.lockState = CursorLockMode.None;
            // ���콺 Ŀ�� ��Ȱ��ȭ(�����)
            Cursor.visible = true;
            canPlayerMove = false;  // ������ ���� ��Ȱ��ȭ
        }
        else                        // �κ��丮 ��Ȱ��ȭ��
        {
            // ���콺 Ŀ�� ��ױ�, Cursor.visible = false �� ����
            Cursor.lockState = CursorLockMode.Locked;
            // ���콺 Ŀ�� ��Ȱ��ȭ(�����)
            Cursor.visible = false;
            canPlayerMove = true;   // ������ ���� Ȱ��ȭ
        }

        if (isWater)        // �����϶�
        {
            if (!flag)      // ���⸦ ������� �ʾҴٸ�
            {
                StopAllCoroutines();
                StartCoroutine(theWM.WeaponInCoroutine());      // ���� ����ֱ�
                flag = true;        // ���� �������
            }
        }
        else                // �� ���� �ƴҶ�
        {
            if (flag)       // ���⸦ ����־��ٸ�
            {
                flag = false;   // ���� ������� ����
                theWM.WeaponOut();          // ���� ������
            }
        }
        
    }

    void Start()
    {
        // ���콺 Ŀ�� ��ױ�, Cursor.visible = false �� ����
        Cursor.lockState = CursorLockMode.Locked;
        // ���콺 Ŀ�� ��Ȱ��ȭ(�����)
        Cursor.visible = false;
        theWM = FindObjectOfType<WeaponManager>();
    }

}
