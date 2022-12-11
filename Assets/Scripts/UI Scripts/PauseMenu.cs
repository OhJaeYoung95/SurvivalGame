using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUI;       // ����޴�â UI
    [SerializeField]
    private SaveNLoad theSaveNLoad;     // ������ ���̺� �ε带 ���� ������Ʈ

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))     // P Ű�� �����ٸ�
        {
            if (!GameManager.isPause)        // ����޴�â�� ��Ȱ��ȭ�ϋ�
                CallMenu();         // �޴�â Ȱ��ȭ
            else
                CloseMenu();        // �޴�â ��Ȱ��ȭ
        }
    }

    private void CallMenu()     // �޴�â Ȱ��ȭ �Լ�
    {
        GameManager.isPause = true;     // ����޴�â ���� Ȱ��ȭ
        go_BaseUI.SetActive(true);      // ����޴�â UI Ȱ��ȭ
        Time.timeScale = 0f;            // �ð��� �帧 ����
    }

    private void CloseMenu()    // �޴�â ��Ȱ��ȭ �Լ�
    {
        GameManager.isPause = false;    // ����޴�â ���� ��Ȱ��ȭ
        go_BaseUI.SetActive(false);     // ����޴�â UI ��Ȱ��ȭ
        Time.timeScale = 1f;            // �ð��� �帧 ����������
    }

    public void ClickSave()     // ���̺� ��ư Ŭ�� �Լ�
    {
        Debug.Log("���̺�");
        theSaveNLoad.SaveData();        // ������ ����
    }

    public void ClickLoad()     // �ε� ��ư Ŭ�� �Լ�
    {
        Debug.Log("�ε�");
        theSaveNLoad.LoadData();        // ������ �ҷ�����
        CloseMenu();            // �޴�â ��Ȱ��ȭ
    }
    public void ClickExit()     // �������� ��ư Ŭ�� �Լ�
    {
        Debug.Log("��������");
        Application.Quit();     // ��������
    }
}
