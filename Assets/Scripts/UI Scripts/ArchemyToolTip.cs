using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchemyToolTip : MonoBehaviour
{
    [SerializeField]
    private Text txt_NeedItemName;          // �ؽ�Ʈ - �ʿ��� ������
    [SerializeField]
    private Text txt_NeedItemNumber;        // �ؽ�Ʈ - �ʿ��� ������ ����

    [SerializeField]
    private GameObject go_BaseToolTip;      // UI ����â

    private void Clear()        // ����â �ؽ�Ʈ �ʱ�ȭ
    {
        txt_NeedItemName.text = "";
        txt_NeedItemNumber.text = "";
    }

    public void ShowToolTip(string[] _needItemName, int[] _needItemNumber)
    {
        Clear();        // ����â �ؽ�Ʈ �ʱ�ȭ
        go_BaseToolTip.SetActive(true);     // UI ����â Ȱ��ȭ

        for (int i = 0; i < _needItemName.Length; i++)
        {
            txt_NeedItemName.text += _needItemName[i] + "\n";
            txt_NeedItemNumber.text += "x " + _needItemNumber[i] + "\n";
        }
    }

    public void HideToolTip()
    {
        Clear();            // ����â �ؽ�Ʈ �ʱ�ȭ
        go_BaseToolTip.SetActive(false);    // UI ����â ��Ȱ��ȭ
    }
}
