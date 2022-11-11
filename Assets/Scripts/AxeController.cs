using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CloseWeaponController ��ũ��Ʈ�� ��ӹ޴´�
public class AxeController : CloseWeaponController
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
                if(hitInfo.transform.tag == "Grass")                        // Ÿ�� �Ǵ°��� �±װ� Grass�̸�
                {
                    hitInfo.transform.GetComponent<Grass>().Damage();       // Grass�� ������� ������ �Լ� ����
                }                
                else if(hitInfo.transform.tag == "Tree")                        // Ÿ�� �Ǵ°��� �±װ� Tree�̸�
                {
                    hitInfo.transform.GetComponent<TreeComponent>()
                        .Chop(hitInfo.point, transform.eulerAngles.y);       // Tree�� �Ӷ��� �Լ� ����, hitInfo�� �ε��� ���� eulerAngles.y ���� ���ڷ� ����
                }
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
