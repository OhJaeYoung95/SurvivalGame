using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    // ���� ������ Hand�� Ÿ��
    [SerializeField]
    private Hand currentHand;

    // ������
    private bool isAttack = false;  // ���������� üũ�ϴ� ����
    private bool isSwing = false;   // ���� �ֵθ��� ������ üũ�ϴ� ����

    private RaycastHit hitInfo;     // ������ ������� ���� �༮�� ������ ���� �� �ִ� ����

    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    private void TryAttack()    // ���� �õ��ϴ� �Լ�
    {
        if(Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()   // ���� �ڷ�ƾ
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);  // ������ ���Ŀ� ����Ȱ��ȭ ����
        isSwing = true;
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);  // ������ ���Ŀ� ���ݺ�Ȱ��ȭ ����
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB); // ������ �� ������ ����
        isAttack = false;
    }

    IEnumerator HitCoroutine()  // ���� �ڷ�ƾ ���� Ÿ�ݱ��� �ڷ�ƾ �Լ�
    {
        while(isSwing)
        {
            if(CheckObject())   // ���ݹ����� �ִٸ�
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;  // 1������ ���
        }
    }

    private bool CheckObject()  // ������ ��Ҵ��� Ȯ�����ִ� Bool�� �Լ�
    {
        // ĳ���Ϳ��� ������ ��� �����ȿ� ��ü�� �ִٸ� ���� ��ü�� ������ �ҷ����� ���ش�.
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true; // ��ü�� ������ True�� ��ȯ�Ѵ�.
        }
        return false;   // �����Ǵ� ��ü�� ������ False�� ��ȯ�Ѵ�.
    }
}
