using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerTooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUI;           // ���� UI

    [SerializeField]
    private Text kitName;               // ŰƮ �̸�
    [SerializeField]
    private Text kitDes;                // ŰƮ ����
    [SerializeField]
    private Text kitNeedItem;           // ���ۿ� �ʿ��� ��� ������ �̸�

    // ���� Ȱ��ȭ, �����ִ� �Լ�
    public void ShowToolTip(string _kitName, string _kitDes, string[] _needItem, int[] _needItemNumber)
    {
        go_BaseUI.SetActive(true);      // ���� UI Ȱ��ȭ

        kitName.text = _kitName;        // �ؽ�Ʈ�� ŰƮ �̸� ����
        kitDes.text = _kitDes;          // �ؽ�Ʈ�� ŰƮ ���� ����

        // �ʿ��� ������ ���� Ž�� 
        for (int i = 0; i < _needItem.Length; i++)
        {
            // (�ؽ�Ʈ ����)������ �̸� x ���ۿ� �ʿ��� �������� ����
            kitNeedItem.text += _needItem[i];
            kitNeedItem.text += " x " + _needItemNumber[i].ToString() + "\n";
        }
    }

    public void HideToolTip()       // ���� ��Ȱ��ȭ, ����� �Լ�
    {
        // �ؽ�Ʈ, UI ��Ȱ��ȭ
        go_BaseUI.SetActive(false);
        kitName.text = "";
        kitDes.text = "";
        kitNeedItem.text = "";
    }

}
