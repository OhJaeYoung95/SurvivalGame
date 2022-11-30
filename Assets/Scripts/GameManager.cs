using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;        // �÷��̾� ������ ���� Ȱ��ȭ ����

    public static bool isOpenInventory = false;     // �κ��丮 Ȱ��ȭ ����

    void Update()
    {
        if (isOpenInventory )        // �κ��丮 Ȱ��ȭ��
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
    }

    void Start()
    {
        // ���콺 Ŀ�� ��ױ�, Cursor.visible = false �� ����
        Cursor.lockState = CursorLockMode.Locked;
        // ���콺 Ŀ�� ��Ȱ��ȭ(�����)
        Cursor.visible = false;
    }

}
