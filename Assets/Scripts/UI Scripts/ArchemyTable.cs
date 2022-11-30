using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArchemyItem            // ���� ������
{
    public string itemName;         // ���� ������ �̸�
    public string itemDesc;         // ���� ������ ����
    public Sprite itemImage;        // ���� ������ �̹���

    public GameObject go_ItemPrefab;    // ���� ������ ������
}

public class ArchemyTable : MonoBehaviour
{
    private bool isOpen = false;            // ������ ����â Ȱ��ȭ ����(�ߺ� ����)

    [SerializeField]
    private ArchemyItem[] archemyItems;     // ������ �� �ִ� ���� ������ ����

    [SerializeField]
    private Transform tf_BaseUI;            // ���̽� UI
    [SerializeField]
    private Transform tf_PotionAppearPos;   // ���� ���� ��ġ

    // Update is called once per frame
    void Update()
    {
        if (isOpen)     // ������ ����â Ȱ��ȭ��, �ߺ�����
            if (Input.GetKeyDown(KeyCode.Escape))       // ESCŰ�� �����ٸ�
                CloseWindow();      // â �ݱ�
    }

    public void Window()        // ������ ����â Ȱ��ȭ, ��Ȱ��ȭ
    {
        isOpen = !isOpen;       // ��ư�� ó�� true -> false, false -> true
        if (isOpen)             // ������ ����â Ȱ��ȭ��, �ߺ�����
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()       // â ���� �Լ�
    {
        isOpen = true;          // ������ ����â Ȱ��ȭ, �ߺ�����
        GameManager.isOpenArchemyTable = true;      // Ŀ�� Ȱ��ȭ
        tf_BaseUI.localScale = new Vector3(1f, 1f, 1f);     // ũŰ�� 1�� �Ǿ Ȱ��ȭ�� ��ó�� �����
    }

    private void CloseWindow()      // â �ݱ� �Լ�
    {
        isOpen = false;         // ������ ����â ��Ȱ��ȭ, �ߺ�����
        GameManager.isOpenArchemyTable = false;     // Ŀ�� ��Ȱ��ȭ
        tf_BaseUI.localScale = new Vector3(0f, 0f, 0f);     // ũŰ�� 0�� �Ǿ ��Ȱ��ȭ�� ��ó�� �����
    }

    public void ButtonClick(int _buttonNum)         // ����â ��ư Ŭ���� �߻��ϴ� �Լ�
    {
        ProductionComplete(_buttonNum);         // ���� ������ ����Ϸ�
    }

    private void ProductionComplete(int _buttonNum)     // ���� ������ ����Ϸ� �Լ�
    {
        // ���� ���̺� ���� ��ġ�� �ش� ���� ������ ����
        Instantiate(archemyItems[_buttonNum].go_ItemPrefab, tf_PotionAppearPos.position, Quaternion.identity);
    }
}
