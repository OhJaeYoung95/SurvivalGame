using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    [SerializeField]
    protected int attackDamage;               // ���� ������
    [SerializeField]
    protected float attackDelay;                // ���� ������
    [SerializeField]
    protected LayerMask targetMask;             // Ÿ�� ����ũ(�÷��̾�)

    [SerializeField]
    protected float ChaseTime;                  // �� �߰� �ð�
    protected float currentChaseTime;           // �߰� ���ð�
    [SerializeField]
    protected float ChaseDelayTime;             // �߰� ������ �ð�

    public void Chase(Vector3 _targetPos)       // �÷��̾�� ������ �߰��ϴ� �Լ�
    {
        isChasing = true;                       // �߰���
        destination = _targetPos;               // �÷��̾� ��ġ ����
        nav.speed = runSpeed;                   // �ٱ� �ӵ� ����
        isRunning = true;                       // �ٴ���
        anim.SetBool("Running", isRunning);     // �ٱ� �ִϸ��̼� ����
        nav.SetDestination(destination);        // �׺�Ž� ������ ����, �̵�
    }

    // �θ�Ŭ���� �Լ��� �����ؼ� ����Ҷ� override �ؼ� ���
    public override void Damage(int _dmg, Vector3 _targetPos)       // ���� ������ �������� ������ ����
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)                          // ������ ���� �ʾ�����
            Chase(_targetPos);                // ������ �÷��̾ �߰��ϴ� �Լ�

    }

    protected IEnumerator ChaseTargetCoroutine()      // �÷��̾� �߰� �ڷ�ƾ
    {
        currentChaseTime = 0;               // �߰� �ð� �ʱ�ȭ

        while (currentChaseTime < ChaseTime)
        {
            Chase(theViewAngle.GetTargetPos());                     // �÷��̾� �߰�
            if (Vector3.Distance(transform.position, theViewAngle.GetTargetPos()) <= 3f)     // ������ ������ Ÿ��(�÷��̾�)���� �Ÿ��� 3���� ������, ������ �ְ�
            {
                if (theViewAngle.View())     // �� �տ� ���� ���
                {
                    Debug.Log("�÷��̾� ���� �õ�");
                    StartCoroutine(AttackCoroutine());      // ������ ���� ���� ����
                }
            }

            yield return new WaitForSeconds(ChaseDelayTime);        // �߰� ������ �ð� ���
            currentChaseTime += ChaseDelayTime;                     // ���� �߰����� �ð��� ���̴ٰ�
                                                                    // �� �߰ݽð��� ����� �ڷ�ƾ���� ���Ƴ�
        }
        isChasing = false;
        isRunning = false;
        anim.SetBool("Running", isRunning);     // �޸��� ���߱�
        nav.ResetPath();        // ������ ��� �ʱ�ȭ, �߰� ����
    }

    protected IEnumerator AttackCoroutine()   // ������ ���� ���� �ڷ�ƾ
    {
        isAttacking = true;     // ������
        nav.ResetPath();        // ����������
        currentChaseTime = ChaseTime;       // �߰� �����ϵ��� ����

        yield return new WaitForSeconds(0.5f);
        transform.LookAt(theViewAngle.GetTargetPos());      // Ÿ��(�÷��̾�)�� �ٶ󺻴�
        anim.SetTrigger("Attack");                          // ���� �ִϸ��̼� ����
        yield return new WaitForSeconds(0.5f);
        RaycastHit _hit;
        // Physics.Raycast(�����߻� ��ġ, �����߻� ����, ������ �ε��� ��ü����, ������Ÿ�, ������ �ε��� ���̾�
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, 3, targetMask))
        {
            Debug.Log("�÷��̾� ����!!");
            thePlayerStatus.DecreaseHP(attackDamage);
        }
        else
        {
            Debug.Log("�÷��̾� ������!!");
        }
        yield return new WaitForSeconds(attackDelay);       // ���� ������ �ð����� ���
        isAttacking = false;                                // ���� ����
        StartCoroutine(ChaseTargetCoroutine());             // �߰� ����
    }

}
