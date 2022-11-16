using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector3 originPos;  // ���콺 �̺�Ʈ�� �ʿ��� ���� ��ġ

    public Item item;           // ȹ���� �������� ������ �������� ���� Item ����
    public int itemCount;       // ȹ���� ������ ����
    public Image itemImage;     // �������� �̹���
    
    // �ʿ��� ������Ʈ
    [SerializeField]
    private Text text_Count;                    // ������ ���� ��Ÿ���ִ� �ؽ�Ʈ
    [SerializeField]
    private GameObject go_CountImage;           // ������ ���� ��Ÿ���°��� �̹��� ���ӿ�����Ʈ

    // ���������� �� ��ü���� �ڱ� �ڽž��� �ִ� �ڽĵ鸸 [SerializeField]�� ������ ������ �� �ִ�
    // ������ �ٸ� ��ü���� ����� �� �� ���⿡ �̿� ���� Start �Լ����� �ʱ�ȭ �ϸ鼭 ��������� �Ѵ�.
    // �̹� ���̾��Ű�� ���� ���� ������ ��ü�� ���������� Instantiate()�� �������� ��쿡 �ش�ȴ�.
    private WeaponManager theWeaponManager;     // �� ������ ���� WeaponManager ����

    void Start()
    {
        originPos = transform.position;
        theWeaponManager = FindObjectOfType<WeaponManager>();
    }
    private void SetColor(float _alpha)         // ������ ���� Ȱ��ȭ, ��Ȱ��ȭ  / ���� ����
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
                                                        // ������ ȹ��
    public void AddItem(Item _item, int _count = 1)     // _count ���� �����ϸ� �⺻���� ��
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;          // itemŬ������ itemImage�� ��������Ʈ�̱⿡ .sprite�� �� ��ȯ

        if(item.itemType != Item.ItemType.Equipment)        // ȹ�� �������� ��� �ƴ϶��
        {
            go_CountImage.SetActive(true);              // ȹ���� ������ ���� �̹��� Ȱ��ȭ
            text_Count.text = itemCount.ToString();     // int Ÿ���� text�� ȣȯ���� �ʾ� string Ÿ������ �� ��ȯ
        }
        else                    // ȹ�� �������� �����
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);            // ������ ���� Ȱ��ȭ
    }

    public void SetSlotCount(int _count)        // ���Ծ����� ���� ����
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    private void ClearSlot()            // ���� �ʱ�ȭ
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)  // �� ��ũ��Ʈ�� ����� ��ü���ٰ� ���콺 ��Ŭ����
        {
            if(item != null)            // �������� �ִٸ�
            {
                if(item.itemType == Item.ItemType.Equipment)        // ��� �������̶��
                {
                    // ����
                    StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName)); // WeaponManager�� ���ⱳü �ڷ�ƾ �Լ� ����
                }
                else                    // ��� �������� �ƴ϶��
                {
                    // �Ҹ�
                    Debug.Log(item.itemName + " �� ����߽��ϴ�");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            transform.position = eventData.position;        // ������ ��ġ�� �̺�Ʈ�� �߻��� ��ġ�� �ٲ۴�
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            transform.position = eventData.position;        // ������ ��ġ�� �̺�Ʈ�� �߻��� ��ġ�� �ٲ۴�
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originPos;
    }

    public void OnDrop(PointerEventData eventData)
    {
    }
}
