using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CloseWeaponController ��ũ��Ʈ�� ��ӹ޴´�
public class HandController : CloseWeaponController
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;

    [SerializeField]
    private QuickSlotController theQuickSlot;   // ������ ���� ���� �������� ���� ����

    void Update()
    {
            if (isActivate && !Inventory.inventoryActivated) // �� Ȱ��ȭ & �κ��丮 ��Ȱ��ȭ��
            {
                if (QuickSlotController.go_HandItem == null)        // �տ� �� �������� ���ٸ�
                    TryAttack();
                else                                                // �ִٸ�
                    TryEating();
        }
    }

    private void TryEating()        // �Ҹ�ǰ ���(�Ա�)
    {
        if(Input.GetButtonDown("Fire1") && !theQuickSlot.GetIsCoolTime())        // ���콺 ��Ŭ����
        {
            currentCloseWeapon.anim.SetTrigger("Eat");      // ������ �Դ� �ִϸ��̼� ����
            theQuickSlot.EatItem();             // ������ �Ա�
        }
    }

    // ���� �ڷ�ƾ
    protected override IEnumerator HitCoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    // ���� ���� ��ȭ �Լ� �����(�θ� Ŭ������ �ִ� �Լ����� �ڽ� �Լ��� �߰��� �� �ۼ�)
    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }

}
