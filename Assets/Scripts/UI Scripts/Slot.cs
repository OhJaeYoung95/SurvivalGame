using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
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
    // �̹� ���̾��Ű�� ���� ���� ������ ��ü�� ���������� ������ ������
    // Instantiate()�� �������� ��쿡�� �־����� �ʴ´�.
    // ���̾��Ű�� ���� ���� �����հ�ü�� ���������� �ְ� Aplly All�� �ص�
    // �ٸ� ������ ��ü���� ������� �ʴ´�.
    [SerializeField]
    private RectTransform baseRect;                  // �κ��丮 ����
    private InputNumber theInputNumber;     // ������ ������ ���� â(��ǲ�ʵ�) ��Ʈ���� ���� ����                                        
    private ItemEffectDatabase theItemEffectDatabase;       // ������ ���ȿ�� ����Ʈ�� ���� ����
    [SerializeField]
    private RectTransform quickSlotBaseRect;        // ������ ����
    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        theInputNumber = FindObjectOfType<InputNumber>();
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
                    theItemEffectDatabase.UseItem(item);        // ������ ��� ȿ��
                if(item.itemType == Item.ItemType.Used)         // ������ Ÿ���� �Ҹ�ǰ�̶��
                    SetSlotCount(-1);       // ������ ���� 1 �Ҹ�
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)         // �巡�� ����
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;               // DragSlot instance�� �ڱ� �ڽ��� ����
            DragSlot.instance.DragSetImage(itemImage);          // �巡�� �ϴ� �̹��� Ȱ��ȭ
            DragSlot.instance.transform.position = eventData.position;        // �巡�� �ϴ� ������ ��ġ�� �̺�Ʈ�� �߻��� ��ġ�� �ٲ۴�
        }
    }

    public void OnDrag(PointerEventData eventData)              // �巡�� ��
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;     // �巡�� �ϴ� ������ ��ġ�� �̺�Ʈ�� �߻��� ��ġ�� �ٲ۴�
        }

    }

    public void OnEndDrag(PointerEventData eventData)           // �巡�װ� �����⸸ �ϸ� ȣ���
    {
        // !(�κ��丮 ���� || // ������ ����), �κ��丮�� ������ ������ �ƴ϶��
        if (!(((DragSlot.instance.transform.localPosition.x > baseRect.rect.xMin && DragSlot.instance.dragSlot.transform.localPosition.x < baseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y > baseRect.rect.yMin && DragSlot.instance.dragSlot.transform.localPosition.y < baseRect.rect.yMax))
            ||   
            (DragSlot.instance.transform.localPosition.x > quickSlotBaseRect.rect.xMin && DragSlot.instance.dragSlot.transform.localPosition.x < quickSlotBaseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y < quickSlotBaseRect.transform.localPosition.y -quickSlotBaseRect.rect.yMin 
            && DragSlot.instance.dragSlot.transform.localPosition.y > quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMax)))
        {
            if(DragSlot.instance.dragSlot != null)      // �巡���� ������ ��ĭ�� �ƴ϶��
            {
                theInputNumber.Call();                  //������ ������ ���� â(��ǲ�ʵ�) Ȱ��ȭ      
            }
        }
        else        // �κ��丮 ���� ���̶��
        {
            DragSlot.instance.SetColor(0);              // instance ��ü ��Ȱ��ȭ
            DragSlot.instance.dragSlot = null;          // dragSlot ��ü ����
        }
    }

    public void OnDrop(PointerEventData eventData)              // �ٸ� ���������� �巡�װ� ���������� ȣ��
    {
        if(DragSlot.instance.dragSlot != null)          // �巡�� ���Կ� �������� ������
            ChangeSlot();                               // ���� ����         
    }

    private void ChangeSlot()               // ���� ����
    {
        // ���� ������ ���� �ִ� ���� ������ �ӽ÷� ����
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)      // ���� �ִ����� �������� �ִٸ�
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);      // �ӽ� ���纻 ������ �ٿ��ֱ�, �巡�� �����ߴ� �ڸ��� �ű��
        else                        // ���� �ִ����� �������� ���ٸ�
            DragSlot.instance.dragSlot.ClearSlot();         // ���� �ʱ�ȭ
    }

    public void OnPointerEnter(PointerEventData eventData)     // ���콺�� ���Կ� ���� �ߵ�
    {
        if(item !=null)                 // �������� �ִٸ�
            theItemEffectDatabase.ShowToolTip(item, transform.position);            // ������ ���� Ȱ��ȭ
    }

    public void OnPointerExit(PointerEventData eventData)     // ���콺�� ���Կ��� �������� �� �ߵ�
    {
        theItemEffectDatabase.HideToolTip();            // ������ ���� ��Ȱ��ȭ
    }
}
