using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchemyToolTip : MonoBehaviour
{
    [SerializeField]
    private Text txt_NeedItemName;          // 텍스트 - 필요한 아이템
    [SerializeField]
    private Text txt_NeedItemNumber;        // 텍스트 - 필요한 아이템 개수

    [SerializeField]
    private GameObject go_BaseToolTip;      // UI 툴팁창

    private void Clear()        // 툴팁창 텍스트 초기화
    {
        txt_NeedItemName.text = "";
        txt_NeedItemNumber.text = "";
    }

    public void ShowToolTip(string[] _needItemName, int[] _needItemNumber)
    {
        Clear();        // 툴팁창 텍스트 초기화
        go_BaseToolTip.SetActive(true);     // UI 툴팁창 활성화

        for (int i = 0; i < _needItemName.Length; i++)
        {
            txt_NeedItemName.text += _needItemName[i] + "\n";
            txt_NeedItemNumber.text += "x " + _needItemNumber[i] + "\n";
        }
    }

    public void HideToolTip()
    {
        Clear();            // 툴팁창 텍스트 초기화
        go_BaseToolTip.SetActive(false);    // UI 툴팁창 비활성화
    }
}
