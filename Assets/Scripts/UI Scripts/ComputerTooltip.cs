using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerTooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUI;           // 툴팁 UI

    [SerializeField]
    private Text kitName;               // 키트 이름
    [SerializeField]
    private Text kitDes;                // 키트 설명
    [SerializeField]
    private Text kitNeedItem;           // 제작에 필요한 재료 아이템 이름

    // 툴팁 활성화, 보여주는 함수
    public void ShowToolTip(string _kitName, string _kitDes, string[] _needItem, int[] _needItemNumber)
    {
        go_BaseUI.SetActive(true);      // 툴팁 UI 활성화

        kitName.text = _kitName;        // 텍스트에 키트 이름 대입
        kitDes.text = _kitDes;          // 텍스트에 키트 설명 대입

        // 필요한 아이템 전부 탐색 
        for (int i = 0; i < _needItem.Length; i++)
        {
            // (텍스트 대입)아이템 이름 x 제작에 필요한 재료아이템 개수
            kitNeedItem.text += _needItem[i];
            kitNeedItem.text += " x " + _needItemNumber[i].ToString() + "\n";
        }
    }

    public void HideToolTip()       // 툴팁 비활성화, 숨기는 함수
    {
        // 텍스트, UI 비활성화
        go_BaseUI.SetActive(false);
        kitName.text = "";
        kitDes.text = "";
        kitNeedItem.text = "";
    }

}
