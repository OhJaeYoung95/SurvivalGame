using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �߻� �ڷ�ƾ �Լ��� ���Ե� Ŭ�������� abstract�� �ٿ���� �Ѵ�
// �̿ϼ� Ŭ���� = �߻� Ŭ����
// �߻� Ŭ������ �ٸ� ��ü�� �߰� ��ų�� ����, ������Ʈ�� ���x
public abstract class CloseWeaponController : MonoBehaviour
{


    // ���� ������ �������� Ÿ��
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // ������
    protected bool isAttack = false;  // ���������� üũ�ϴ� ����
    protected bool isSwing = false;   // ���� �ֵθ��� ������ üũ�ϴ� ����

    protected RaycastHit hitInfo;     // ������ ������� ���� �༮�� ������ ���� �� �ִ� ����
    [SerializeField]
    protected LayerMask layerMask;      // ���� ���� �ʴ� ���̾ �����ϱ� ���� ����

    // �ʿ��� ������Ʈ
    private PlayerController thePlayerController;       // ī�޶� �����̼� �� ������ ���� ������Ʈ

    void Start()
    {
        thePlayerController = FindObjectOfType<PlayerController>();
    }
    protected void TryAttack()    // ���� �õ��ϴ� �Լ�
    {
        if (!Inventory.inventoryActivated)       // �κ��丮 ��Ȱ��ȭ�ÿ���
        {
            if (Input.GetButton("Fire1"))       // ���콺 ��Ŭ��
            {
                if (!isAttack)
                {
                    if (CheckObject())           // ������ ��Ҵ��� Ȯ��,
                    {                                                                           //(�۾����� Ȯ��)
                        if (currentCloseWeapon.isAxe && hitInfo.transform.tag == "Tree")        // �ֵθ��� ���Ⱑ �����̰�, ������ ���� ��ü�� �������� Ȯ��
                        {
                            //StartCoroutine(thePlayerController.TreeLookCoroutine(hitInfo.transform.position));      // ������ ĥ�� ������ �ٶ󺸰� ����
                            StartCoroutine(thePlayerController.TreeLookCoroutine(hitInfo.transform.GetComponent<TreeComponent>().GetTreeCenterPosition()));      // ������ ĥ�� ������ �ٶ󺸰� ����
                            StartCoroutine(AttackCoroutine("Chop", currentCloseWeapon.workDelayA,         // �´ٸ� Chop �ִϸ��̼� ����
                            currentCloseWeapon.workDelayB,
                            currentCloseWeapon.workDelay - currentCloseWeapon.workDelayA - currentCloseWeapon.workDelayB));
                            return;         // �ؿ� �ڷ�ƾ�� �ߺ������ �� �ֱ⿡ return���� �Լ� ����
                        }
                    }
                    StartCoroutine(AttackCoroutine("Attack", currentCloseWeapon.attackDelayA,   // ����
                        currentCloseWeapon.attackDelayB,
                        currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB));
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine(string swingType, float _delayA, float _delayB, float _delayC)   // ����(swing)�ڷ�ƾ
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger(swingType);

        yield return new WaitForSeconds(_delayA);  // ������ ���Ŀ� ����Ȱ��ȭ ����
        isSwing = true;
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(_delayB);  // ������ ���Ŀ� ���ݺ�Ȱ��ȭ ����
        isSwing = false;

        yield return new WaitForSeconds(_delayC - _delayA - _delayB); // ������ �� ������ ����
        isAttack = false;
    }

    // abstract = �߻� �ڷ�ƾ = �̿ϼ�(�ڽ� Ŭ�������� �ϼ����Ѿ���)
    protected abstract IEnumerator HitCoroutine();  // ���� �ڷ�ƾ ���� Ÿ�ݱ��� �ڷ�ƾ �Լ�


    protected bool CheckObject()  // ������ ��Ҵ��� Ȯ�����ִ� Bool�� �Լ�
    {
        // ĳ���Ϳ��� ������ ��� �����ȿ� ��ü�� �ִٸ� ���� ��ü�� ������ �ҷ����� ���ش�.
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true; // ��ü�� ������ True�� ��ȯ�Ѵ�.
        }
        return false;   // �����Ǵ� ��ü�� ������ False�� ��ȯ�Ѵ�.
    }

    // ���� �Լ� : �ϼ��Լ�������, �߰� ���� ������ �Լ�
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)     // �������� ��ȭ �Լ�
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
