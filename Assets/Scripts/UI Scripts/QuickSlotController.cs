using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField]
    private Slot[] quickSlots;      // ������ �迭
    [SerializeField]
    private Image[] img_CoolTime;   // ������ ��Ÿ��
    [SerializeField]
    private Transform tf_parent;    // ������ �θ� ��ü

    [SerializeField]
    private Transform tf_ItemPos;   // �������� ��ġ�� �� ��
    public static GameObject go_HandItem;   // �տ� �� ������

    // ��Ÿ�� ����
    [SerializeField]
    private float coolTime;         // ��Ÿ��
    private float currentCoolTime;  // ���� ��Ÿ��
    private bool isCoolTime;        // ��Ÿ�� ����������

    // ������ ���� ����
    [SerializeField]
    private float appearTime;           // �������� �����ϴ� �ð�
    private float currentAppearTime;    // ���� �������� �������ִ� �ð�
    private bool isAppear;              // ������ �ִ���


    private int selectedSlot;       // ���õ� ������ ��ȣ (0~7), 8��

    // �ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_SelectedImage;    // ���õ� �������� �̹���
    [SerializeField]
    private WeaponManager theWeaponManager;     // ���� ��ü�� ���� ����
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();     // ������ �θ� ��ü �ؿ� �ڽ� ��ü���� �Ҵ�
        anim = GetComponent<Animator>();
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
        CoolTimeCalc();
        AppearCalc();
    }

    private void AppearReset()      // ������ ���� �ð� �ʱ�ȭ
    {
        currentAppearTime = appearTime;     // ���� �ð� �ʱ�ȭ
        isAppear = true;                    // ���� �����ϰ� ����
        anim.SetBool("Appear", isAppear);   // �������� ���� �ϴ� �ִϸ��̼� ����
    }

    private void AppearCalc()           // ������ ���� �ð� ���
    {
        if (Inventory.inventoryActivated)       // �κ��丮â�� Ȱ��ȭ �Ǿ�������
            AppearReset();                      // ������â ����
        else
        {
            if (isAppear)            // �������� ������ ������
            {
                currentAppearTime -= Time.deltaTime;        // 1�ʿ� 1 ����, �������� ������ ������ �ִ� �ð� ����
                if (currentAppearTime <= 0)                  // ����ð��� ������
                {
                    isAppear = false;                       // ���� ���ϰ� ����
                    anim.SetBool("Appear", isAppear);       // �������� ������� �ִϸ��̼� ����
                }
            }
        }
    }

    private void CoolTimeReset()        // ��Ÿ�� �ʱ�ȭ
    {
        currentCoolTime = coolTime;     // ��Ÿ�� �ð� �ʱ�ȭ
        isCoolTime = true;              // ��Ÿ�� ����
    }

    private void CoolTimeCalc()     // ��Ÿ�� �ð� ����Լ�
    {
        if(isCoolTime)          // ��Ÿ�� �������϶�
        {
            currentCoolTime -= Time.deltaTime;      // 1�ʿ� 1������
            for (int i = 0; i < img_CoolTime.Length ; i++)
                img_CoolTime[i].fillAmount = currentCoolTime / coolTime;        // ��Ÿ�ӿ� ���� �̹��� ä���
            if (currentCoolTime <= 0)           // ���� ��Ÿ���� 0���� �۾�����
                isCoolTime = false;             // ��Ÿ�� ��
        }
    }

    private void TryInputNumber()       // ������ ����
    {
        if(!isCoolTime)     // ��Ÿ���� �ƴҶ��� ���Լ��� ����
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ChangeSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                ChangeSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                ChangeSlot(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                ChangeSlot(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                ChangeSlot(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                ChangeSlot(5);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                ChangeSlot(6);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                ChangeSlot(7);
        }
    }

    public void IsActivatedQuickSlot(int _num)      // �������� Ȱ��ȭ �Ǿ�������
    {                                               
        if (selectedSlot == _num )    // ���õ� ������ �ѹ��� ��ġ�Ѵٸ�
        {
            Execute();      // �ش� �����Կ� ���ؼ� �������ִ� �Լ�
            return;
        }

        if(DragSlot.instance != null)
        {
            if(DragSlot.instance.dragSlot != null)
            {
                // Slot���� quickSlotNumber�� private�̶� GetQuickSlotNumber()�Լ��� �̿��� ���� �����ͼ� ���Ѵ�
                if (DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot)
                {
                    Execute();      // �ش� �����Կ� ���ؼ� �������ִ� �Լ�
                    return;
                }
            }
        }
    }

    private void ChangeSlot(int _num)       // ������ ��ȣ ��ü
    {
        SelectedSlot(_num);

        Execute();
    }

    private void SelectedSlot(int _num)     // ������ ��ȣ ����
    {
        selectedSlot = _num;        // ���õ� ���� ��ȣ
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;      // ���õ� ���� ��ȣ�� �ش��ϴ� ��ġ�� �̹��� �̵�
    }

    private void Execute()  //�ش� �����Կ� ���ؼ� �������ִ� �Լ�
    {
        CoolTimeReset();                // ��Ÿ�� �ʱ�ȭ
        AppearReset();                  // ������ ����

        if (quickSlots[selectedSlot].item != null)       // ���õ� �����Կ� �������� �ִٸ�
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)      // �����Կ� �������� �����
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));       // �����Կ� �ش��ϴ� ����� ��ü
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used)      // �����Կ� �������� �Ҹ�ǰ�̶��
                ChangeHand(quickSlots[selectedSlot].item);
            else
                ChangeHand();               // �Ǽ����� ��ü
        }
        else                            // ���õ� �����Կ� �������� ���ٸ�
        {
            ChangeHand();               // �Ǽ����� ��ü
        }
    }

    private void ChangeHand(Item _item = null)         // �ո�� ��ü
    {
        StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "�Ǽ�"));   // �Ǽ����� ��ü

        if (_item != null)
            StartCoroutine(HandItemCoroutine());
    }

    IEnumerator HandItemCoroutine()
    {
        // ���ٽ� : ���� ������ �����ؾ��� �Ѿ��
        // () => 
        HandController.isActivate = false;      // �� ��Ȱ��ȭ
        yield return new WaitUntil(() => HandController.isActivate);      // ���� Ȱ��ȭ �ɶ����� ��ٸ���

        go_HandItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, tf_ItemPos.position, tf_ItemPos.rotation);
        go_HandItem.GetComponent<Rigidbody>().isKinematic = true;       // �߷� ����x
        go_HandItem.GetComponent<BoxCollider>().enabled = false;        // �浹 ���� ����
        go_HandItem.tag = "Untagged";           // �±� ����
        go_HandItem.layer = 6;                  // Weapon ���̾� 6��
        go_HandItem.transform.SetParent(tf_ItemPos);        // �տ��� �������� ������ ������ġ ������Ʈ�� �θ�� ����
    }

    public void EatItem()           // ������ �Դ� �Լ�
    {
        CoolTimeReset();                // ��Ÿ�� �ʱ�ȭ
        AppearReset();                  // ������ ����
        quickSlots[selectedSlot].SetSlotCount(-1);      // ���������� ����ϴ� ������ �Ҹ�

        if (quickSlots[selectedSlot].itemCount <= 0)    // ���� ������ 0���� 
            Destroy(go_HandItem);                       // �տ� �� ������ ����
    }

    public bool GetIsCoolTime()         // �ܺο��� ��Ÿ�ӿ��θ� Ȯ���ϱ� ���� ����
    {
        return isCoolTime;
    }
}