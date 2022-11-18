using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_Base;     //슬롯 툴팁

    [SerializeField]
    private Text txt_ItemName;      // 툴팁 아이템 이름
    [SerializeField]
    private Text txt_ItemDesc;      // 툴팁 아이템 설명
    [SerializeField]
    private Text txt_ItemHowtoUsed;      // 툴팁 아이템 사용법

    public void ShowToolTip(Item _item, Vector3 _pos)     // 툴팁 활성화
    {
        go_Base.SetActive(true);        // 아이템 툴팁창 활성화
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,        // 슬롯 아이템 정중앙 위치에서 위치 재조정
                            - go_Base.GetComponent<RectTransform>().rect.height * 0.6f,
                            0f);
        go_Base.transform.position = _pos;      // 툴팁창 위치 설정

        txt_ItemName.text = _item.itemName;         // 아이템 이름
        txt_ItemDesc.text = _item.itemDesc;         // 아이템 설명

        if (_item.itemType == Item.ItemType.Equipment)      // 아이템 타입이 장비라면
            txt_ItemHowtoUsed.text = "우클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used)      // 아이템 타입이 사용아이템이면
            txt_ItemHowtoUsed.text = "우클릭 - 먹기";
        else 
            txt_ItemHowtoUsed.text = "";
    }

    public void HideToolTip()               // 툴팁 비활성화
    {
        go_Base.SetActive(false);       // 아이템 툴팁창 비활성화
    }
}
