using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArchemyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private ArchemyTable theArchemy;    // UI ����â Ȱ��ȭ�� ���� ������Ʈ
    [SerializeField]
    private int buttonNum;          // �ش� ����â�� ���� ���� ��ư ��ȣ

    // ���� ��ũ��Ʈ�� ������Ʈ�� ������ �ִ� ������Ʈ�� ���콺 �����Ͱ� ���ö� �߻��ϴ� �Լ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ��ư ��ȣ�� �ش��ϴ� UI ����â Ȱ��ȭ
        theArchemy.ShowToolTip(buttonNum);
    }

    // ���� ��ũ��Ʈ�� ������Ʈ�� ������ �ִ� ������Ʈ���� ���콺 �����Ͱ� ����������  �߻��ϴ� �Լ�
    public void OnPointerExit(PointerEventData eventData)
    {
        // UI ����â ��Ȱ��ȭ
        theArchemy.HideToolTip();

    }
}
