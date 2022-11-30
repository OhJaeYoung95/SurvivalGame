using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CloseWeaponController ��ũ��Ʈ�� ��ӹ޴´�
public class HandController : CloseWeaponController
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;
    public static Item currentKit;      // ��ġ�Ϸ��� ŰƮ (���� ���̺�)

    private bool isPreview = false;     // ŰƮ �̸����� Ȱ��ȭ ����

    private GameObject go_Preview;      // ��ġ�� ŰƮ �̸�����
    private Vector3 previewPos;         // ��ġ�� ŰƮ ��ġ
    [SerializeField]
    private float rangeAdd;             // ����� �߰� �����Ÿ�

    [SerializeField]
    private QuickSlotController theQuickSlot;   // ������ ���� ���� �������� ���� ����

    void Update()
    {
        if (isActivate && !Inventory.inventoryActivated) // �� Ȱ��ȭ & �κ��丮 ��Ȱ��ȭ��
        {
            if(currentKit == null)      // ��ġ�Ϸ��� ŰƮ�� ������
            {
                if (QuickSlotController.go_HandItem == null)        // �տ� �� �������� ���ٸ�
                    TryAttack();
                else                                                // �ִٸ�
                    TryEating();
            }
            else
            {
                if(!isPreview)      // �̸����Ⱑ ��Ȱ��ȭ�϶�
                    InstallPreviewKit();        // ŰƮ �̸����� ������ ����
                PreviewPositionUpdate();        // �̸����� ������ ��ġ ������Ʈ
                Build();                // ŰƮ ������ ���� �Լ�
            }
        }
    }

    private void InstallPreviewKit()        // ŰƮ �̸����� ������ ����
    {
        isPreview = true;
        go_Preview = Instantiate(currentKit.kitPreviewPrefab, transform.position, Quaternion.identity);
    }

    private void PreviewPositionUpdate()        // �̸����� ������ ��ġ ������Ʈ
    {
        // �������� �������� ���� layerMask�� �ش��ϴ� ���̾ �浹�ϵ��� ����
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range + rangeAdd, layerMask))
        {
            previewPos = hitInfo.point;     // �������� ���� ��ġ
            go_Preview.transform.position = previewPos;     // ��ġ �ż��� ������Ʈ
        }
    }

    private void Build()        // ŰƮ ������ ���� �Լ�
    {
        if(Input.GetButtonDown("Fire1"))    // ���콺 ��Ŭ����
        {
            // �ǹ� ��ġ ���ɽ�
            if(go_Preview.GetComponent<PreviewObject>().IsBuildable())
            {
                theQuickSlot.DecreaseSelectedItem();        // ���� ������ ���� -1
                GameObject temp = Instantiate(currentKit.kitPrefab, previewPos, Quaternion.identity);   // ŰƮ ������ ����
                temp.name = "Archemy Kit";
                // �ʱ�ȭ ����(�����ۻ���, bool��), ����������
                Destroy(go_Preview);        // �̸����� ������ ����
                currentKit = null;
                isPreview = false;
            }
        }
    }

    public void Cancel()        // �ܺο��� ŰƮ �̸����� �ʱ�ȭ���ִ� �Լ�
    {
        // �ʱ�ȭ ����(�����ۻ���, bool��), ����������
        Destroy(go_Preview);        // �̸����� ������ ����
        currentKit = null;
        isPreview = false;
    }

    private void TryEating()        // �Ҹ�ǰ ���(�Ա�)
    {
        if(Input.GetButtonDown("Fire1") && !theQuickSlot.GetIsCoolTime())        // ���콺 ��Ŭ����
        {
            currentCloseWeapon.anim.SetTrigger("Eat");      // ������ �Դ� �ִϸ��̼� ����
            theQuickSlot.DecreaseSelectedItem();             // ���õ� ������ ������ �Ҹ� �Լ�
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
