using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;        // 자기 자신을 사용하기 위해서

    public Slot dragSlot;

    [SerializeField]
    private Image imageItem;            // 아이템 이미지

    void Start()
    {
        instance = this;                    // DragSlot instance 객체에 자기 자신을 대입
    }

    public void DragSetImage(Image _itemImage)          // 드래그 당하는 슬롯의 이미지 활성화
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)              // 슬롯 활성화, 비활성화
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
