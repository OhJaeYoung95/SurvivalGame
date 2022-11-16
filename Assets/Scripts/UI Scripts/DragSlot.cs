using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;        // �ڱ� �ڽ��� ����ϱ� ���ؼ�

    public Slot dragSlot;

    [SerializeField]
    private Image imageItem;            // ������ �̹���

    void Start()
    {
        instance = this;                    // DragSlot instance ��ü�� �ڱ� �ڽ��� ����
    }

    public void DragSetImage(Image _itemImage)          // �巡�� ���ϴ� ������ �̹��� Ȱ��ȭ
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)              // ���� Ȱ��ȭ, ��Ȱ��ȭ
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
