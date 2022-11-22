using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    public void Chase(Vector3 _targetPos)      // �÷��̾�� ������ �߰��ϴ� �Լ�
    {
        isChasing = true;           // �߰���
        destination = _targetPos;   // �÷��̾� ��ġ ����
        nav.speed = runSpeed;       // �ٱ� �ӵ� ����
        isRunning = true;           // �ٴ���
        anim.SetBool("Running", isRunning);     // �ٱ� �ִϸ��̼� ����
        nav.SetDestination(destination);        // �׺�Ž� ������ ����
    }

    // �θ�Ŭ���� �Լ��� �����ؼ� ����Ҷ� override �ؼ� ���
    public override void Damage(int _dmg, Vector3 _targetPos)       // ���� ������ �������� ������ ����
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)                          // ������ ���� �ʾ�����
            Chase(_targetPos);                // ������ �÷��̾ �߰��ϴ� �Լ�

    }
}
