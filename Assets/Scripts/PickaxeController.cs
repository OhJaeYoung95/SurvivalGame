using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    // Ȱ��ȭ ����
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    // ���� �ڷ�ƾ
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.tag == "Rock")                                          // �����Ͻ�
                    hitInfo.transform.GetComponent<Rock>().Mining();                          // ä��
                else if (hitInfo.transform.tag == "Twig")                                     // ���������Ͻ�
                    hitInfo.transform.GetComponent<Twig>().Damage(this.transform);            // �����
                else if (hitInfo.transform.tag == "NPC")                                      // NPC�Ͻ�
                {
                    SoundManager.instance.PlaySE("Animal_Hit");     // ���� Ÿ�� ȿ���� ���
                    hitInfo.transform.GetComponent<Pig>().Damage(1, transform.position);      // �����
                }
                isSwing = false;        // ������ ���� ȣ���ϱ⿡ false ���� �־� �� �ڵ带 �ѹ��� �����ϵ��� ����
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
