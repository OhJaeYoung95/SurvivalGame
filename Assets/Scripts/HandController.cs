using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CloseWeaponController ��ũ��Ʈ�� ��ӹ޴´�
public class HandController : CloseWeaponController
{
    // Ȱ��ȭ ����
    public static bool isActivate = false;

    void Update()
    {
        if (isActivate)
            TryAttack();
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
