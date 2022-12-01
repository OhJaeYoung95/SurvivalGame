using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;    // ���� ������ �ִ� �Ÿ�

    // ���º���
    private bool pickupActivated = false;   // ������ ���� ������ �� true
    private bool dissolveActivated = false;     // ��� ��ü ������ �� true
    private bool isDissolving = false;          // ��� ��ü �߿��� true
    private bool fireLookActivated = false;     // ���� �����ؼ� �ٶ� �� true
    private bool lookArchemyTable = false;      // ���� ���̺��� �ٶ� �� true
    private bool lookComputer = false;          // ��ǻ�͸� �ٶ� �� true

    private RaycastHit hitInfo;         // �浹ü ���� ����

    [SerializeField]
    private LayerMask layerMask;        // ������ ���̾�� �����ϵ��� ���̾� ����ũ ����

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Text actionText;            // �׼ǿ� ���̴� �ؽ�Ʈ
    [SerializeField]
    private Inventory theInventory;     // ������ ȹ�濡 ���� �κ��丮
    [SerializeField]
    private WeaponManager theWeaponManager;     // ���� ������ �ٷ�� ���� ����
    [SerializeField]
    private QuickSlotController theQuickSlot;   // ������ ������ �ٷ�� ���� ����
    [SerializeField]
    private Transform tf_MeatDissolveTool;      // ��� ��ü ��
    [SerializeField]
    private ComputerKit theComputer;            // ��ǻ�� ���� On/Off�� ���� ������Ʈ

    [SerializeField]
    private string sound_meat;              // ��� ��ü ȿ���� ���


    // Update is called once per frame
    void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()            // �׼� ����
    {
        if(Input.GetKeyDown(KeyCode.E))     // EŰ�� �����ٸ�
        {
            CheckAction();                  // �׼� Ȯ�� �Լ�
            CanPickUp();                    // ������ ���� �Լ�
            CanMeat();                      // ��� ��ü �Լ�
            CanDropFire();                  // �ҿ� ��� ������ �Լ�
            CanComputerPowerOn();           // ��ǻ�� ���� On ���ִ� �Լ�
            CanArchemyTableOpen();          // ����â �����ִ� �Լ�
        }
    }

    private void CanPickUp()                // ������ �����Լ�
    {
        if(pickupActivated)         // ������ ���� �����ҽ�
        {
            if(hitInfo.transform != null)       // Raycast������ �浹 ��ü�� �ִٸ�
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ���߽��ϴ�");      // ������ ȹ�濩�� �ܼ�â�� �����
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);        // �浹 ��ü ������ �κ��丮�� ȹ��
                Destroy(hitInfo.transform.gameObject);          // ������ ���ӿ�����Ʈ ����
                InfoDisappear();                                // ������ ȹ��� ������ ���� ��Ȱ��ȭ
            }
        }
    }

    private void CanComputerPowerOn()       // ��ǻ�� ���� On ���ִ� �Լ�
    {
        if (lookComputer)         // ��ǻ�͸� �ٶ� ��
        {
            if (hitInfo.transform != null)       // Raycast������ �浹 ��ü�� �ִٸ�
            {
                if(!hitInfo.transform.GetComponent<ComputerKit>().isPowerOn)    // ��ǻ�� ������ ����������
                {
                    hitInfo.transform.GetComponent<ComputerKit>().PowerOn();        // ��ǻ�� ���� On
                    InfoDisappear();                     // ��ǻ�� ���� ���� ��Ȱ��ȭ
                }
            }
        }
    }

    private void CanArchemyTableOpen()       // ����â �����ִ� �Լ�
    {
        if (lookArchemyTable)         // ���� ���̺��� �ٶ� ��
        {
            if (hitInfo.transform != null)       // Raycast������ �浹 ��ü�� �ִٸ�
            {
                hitInfo.transform.GetComponent<ArchemyTable>().Window();      // ������ ����â Ȱ��ȭ, ��Ȱ��ȭ
                InfoDisappear();                     // ������ ����â ���� ��Ȱ��ȭ

            }
        }
    }


    private void CanMeat()      // ��� ��ü �Լ�
    {
        if(dissolveActivated)       // ��� ��ü �����ҋ�
        {       // �����϶� && ������ �׾������� && ��� ��ü���� �ƴҶ�
            if((hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal") 
                && hitInfo.transform.GetComponent<Animal>().isDead && !isDissolving)
            {
                isDissolving = true;    // ��� ��ü��
                InfoDisappear();        // �׼�����â ��Ȱ��ȭ
                StartCoroutine(MeatCoroutine());    // ��� ��ü
            }
        }
    }

    IEnumerator MeatCoroutine()         // ��� ��ü �ڷ�ƾ
    {
        WeaponManager.isChangeWeapon = true;            // ��� ��ü�߿��� ���� ��ü�� �ȵǵ���
        WeaponSway.isActivated = false;                 // ���� ��鸲 ��Ȱ��ȭ
        WeaponManager.currentWeaponAnim.SetTrigger("Weapon_Out");       // ���� �������� �ִϸ��̼�
        PlayerController.isActivated = false;           // �÷��̾� �����Ұ���

        yield return new WaitForSeconds(0.2f);
        
        WeaponManager.currentWeapon.gameObject.SetActive(false);        // ���� ������Ʈ ��Ȱ��ȭ
        tf_MeatDissolveTool.gameObject.SetActive(true);                 // ��� ��ü ���� Ȱ��ȭ ���ÿ� �ִϸ��̼� ����

        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.PlaySE(sound_meat);           // ��� ��ü ȿ���� ���
        yield return new WaitForSeconds(1.8f);

        // �κ��丮�� ������ ȹ��(����������, ������ ����)
        theInventory.AcquireItem(hitInfo.transform.GetComponent<Animal>().GetItem(), hitInfo.transform.GetComponent<Animal>().itemNumber);

        tf_MeatDissolveTool.gameObject.SetActive(false);               // ��� ��ü ���� ��Ȱ��ȭ
        WeaponManager.currentWeapon.gameObject.SetActive(true);        // ���� ������Ʈ Ȱ��ȭ

        PlayerController.isActivated = true;           // �÷��̾� ��������
        WeaponSway.isActivated = true;                 // ���� ��鸲 Ȱ��ȭ
        WeaponManager.isChangeWeapon = false;       // ��� ��ü�� ���� ��ü �����ϵ��� ����
        isDissolving = false;           // ��� ��ü���� �ƴҶ�
    
    }

    private void CanDropFire()      // �ҿ� ��� ������ �Լ�
    {
        if(fireLookActivated)       // ���� ���� �ִٸ�
        {
            if(hitInfo.transform.tag == "Fire" && hitInfo.transform.GetComponent<Fire>().GetIsFire())       // ���� ���� �ְ� && ���� �����ִٸ�
            {
                Slot _selectedSlot = theQuickSlot.GetSelectedSlot();    // ���õ� ������ ������ ������
                if(_selectedSlot.item != null)       // ���õ� �����Կ� �������� �ִٸ�
                {
                    DropAnItem(_selectedSlot);               // ������ ������
                }
            }
        }
    }

    private void DropAnItem(Slot _selectedSlot)           // ������ ������ �Լ�
    {
        switch(_selectedSlot.item.itemType)         // ���õ� �������� ������ Ÿ��
        {
            case Item.ItemType.Used:                // �Ҹ�ǰ�̶��
                if(_selectedSlot.item.itemName.Contains("���"))      // �������̸��� "���"�� ���ԵǾ� �ִٸ�, Contains�� ���ڿ� ��ü�� �޼ҵ�
                {
                    Instantiate(_selectedSlot.item.itemPrefab, hitInfo.transform.position + Vector3.up, Quaternion.identity);       // ������ �����Կ� ���õ� ������ ����
                    theQuickSlot.DecreaseSelectedItem();            // ���õ� ������ ������ �Ҹ� �Լ�
                }
                break;
            case Item.ItemType.Ingredient:          // �����
                break;

        }
    }

    private void CheckAction()            // Ȯ���ϴ� �׼� �Լ�
    {
        // ������ �÷��̾ ��ġ���� �÷��̾ �ٶ󺸴� �������� ��� ���̾� ����ũ�� ����� Ȯ��
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, layerMask))
        {
            if (hitInfo.transform.tag == "Item")                        // �������̸�
                ItemInfoAppear();                           // ������ ���� Ȱ��ȭ
            else if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal")      // �����̸�
                MeatInfoAppear();                           // ��� ��ü ���� Ȱ��ȭ
            else if (hitInfo.transform.tag == "Fire")       // ���̸�
                FireInfoAppear();       //�� ���� �׼�����â Ȱ��ȭ
            else if (hitInfo.transform.tag == "Computer")
                ComputerInfoAppear();   // ��ǻ�� �׼�����â Ȱ��ȭ
            else if (hitInfo.transform.tag == "ArchemyTable")
                ArchemyInfoAppear();    // ���ݼ� �׼�����â Ȱ��ȭ
            else
                InfoDisappear();        // �׼�����â ��Ȱ��ȭ
        }
        else                            // �������� ������ ���� �ʾ�����
        {
            InfoDisappear();            // �׼�����â ��Ȱ��ȭ
        }
    }

    private void Reset()            // ���º��� false�� ����
    {
        pickupActivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
        lookComputer = false;
    }

    private void ItemInfoAppear()           // ������ �׼�����â Ȱ��ȭ
    {
        Reset();                            // ���º��� false�� ����
        pickupActivated = true;                      // ������ �ݱ� ����
        actionText.gameObject.SetActive(true);       // ������ ���� Ȱ��ȭ
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ��" + "<color=yellow>" + "(E)" + "</color>";        // �ش� ������ �̸� ȹ��ǥ��, �ؽ�Ʈ�� �� ����
    }
    private void MeatInfoAppear()           // �����ü �׼�����â Ȱ��ȭ
    {
        if(hitInfo.transform.GetComponent<Animal>().isDead)     // ������ �׾�������
        {
            Reset();                                // ���º��� false�� ����
            dissolveActivated = true;                      // ��� ��ü ����
            actionText.gameObject.SetActive(true);       // ������ ���� Ȱ��ȭ
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + " ��ü�ϱ� " + "<color=yellow>" + "(E)" + "</color>";        // �ش� ���� �̸� ��üǥ��, �ؽ�Ʈ�� �� ����
        }
    }

    private void FireInfoAppear()       // �� ���� �׼�����â Ȱ��ȭ
    {
        Reset();                            // ���º��� false�� ����
        fireLookActivated = true;           // ���� �ٶ󺸰� ����
        if (hitInfo.transform.GetComponent<Fire>().GetIsFire())      // �� ������Ʈ���� bool���� isFire���� �޾Ƽ� ���� �����ִ��� Ȯ��
        {
            actionText.gameObject.SetActive(true);       // �� ���� ���� Ȱ��ȭ
            actionText.text = "���õ� ������ �ҿ� �ֱ� " + "<color=yellow>" + "(E)" + "</color>";     // �ؽ�Ʈ�� �� ����
        }
    }

    private void ComputerInfoAppear()           // ��ǻ�� �׼�����â Ȱ��ȭ
    {
        // ��ǻ�� ������ ����������
        if(!hitInfo.transform.GetComponent<ComputerKit>().isPowerOn)
        {
            Reset();                            // ���º��� false�� ����
            lookComputer = true;                      //  ��ǻ�͸� �ٶ󺸰� ������
            actionText.gameObject.SetActive(true);       // ��ǻ�� ���� ���� Ȱ��ȭ
            actionText.text = "��ǻ�� ���� " + "<color=yellow>" + "(E)" + "</color>";        // �ش� ������ �̸� ȹ��ǥ��, �ؽ�Ʈ�� �� ����
        }
    }
    private void ArchemyInfoAppear()           // ���� ���̺� �׼�����â Ȱ��ȭ
    {
        // ���� ���̺� �׼�����â�� ��Ȱ��ȭ�϶� 
        if(!hitInfo.transform.GetComponent<ArchemyTable>().GetIsOpen())
        {
            Reset();                                     // ���º��� false�� ����
            lookArchemyTable = true;                     //  ���� ���̺��� �ٶ󺸰� ������
            actionText.gameObject.SetActive(true);       // ���� ���̺� ���� Ȱ��ȭ
            actionText.text = "���� ���̺� ���� " + "<color=yellow>" + "(E)" + "</color>";        // �ش� ������ �̸� ȹ��ǥ��, �ؽ�Ʈ�� �� ����
        }
    }


    private void InfoDisappear()            // (������ , �����ü)�׼�����â ��Ȱ��ȭ
    {
        pickupActivated = false;                    // ������ �ݱ� �Ұ���
        dissolveActivated = false;                  // ��� ��ü �Ұ���
        fireLookActivated = false;                  // ���� �ٶ󺸰� ����
        lookComputer = false;                       //  ��ǻ�͸� �ٶ��� ������
        lookArchemyTable = false;                   //  ���� ���̺��� �ٶ��� ������
        actionText.gameObject.SetActive(false);           // ������ ���� ��Ȱ��ȭ
    }
}
