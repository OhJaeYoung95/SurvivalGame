using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    // �ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_Base;     //���� ����

    [SerializeField]
    private Text txt_ItemName;      // ���� ������ �̸�
    [SerializeField]
    private Text txt_ItemDesc;      // ���� ������ ����
    [SerializeField]
    private Text txt_ItemHowtoUsed;      // ���� ������ ����

    public void ShowToolTip(Item _item, Vector3 _pos)     // ���� Ȱ��ȭ
    {
        go_Base.SetActive(true);        // ������ ����â Ȱ��ȭ
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,        // ���� ������ ���߾� ��ġ���� ��ġ ������
                            - go_Base.GetComponent<RectTransform>().rect.height * 0.6f,
                            0f);
        go_Base.transform.position = _pos;      // ����â ��ġ ����

        txt_ItemName.text = _item.itemName;         // ������ �̸�
        txt_ItemDesc.text = _item.itemDesc;         // ������ ����

        if (_item.itemType == Item.ItemType.Equipment)      // ������ Ÿ���� �����
            txt_ItemHowtoUsed.text = "��Ŭ�� - ����";
        else if (_item.itemType == Item.ItemType.Used)      // ������ Ÿ���� ���������̸�
            txt_ItemHowtoUsed.text = "��Ŭ�� - �Ա�";
        else 
            txt_ItemHowtoUsed.text = "";
    }

    public void HideToolTip()               // ���� ��Ȱ��ȭ
    {
        go_Base.SetActive(false);       // ������ ����â ��Ȱ��ȭ
    }
}
