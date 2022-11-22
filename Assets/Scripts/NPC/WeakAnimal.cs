using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{
    public void Run(Vector3 _targetPos)      // �÷��̾�� �°� �پ �������� �Լ�
    {
        // ���� ��ġ���� Ÿ����ġ(�÷��̾�)�� ���� �ݴ�������� ����ġ�� ��ǥ�� ���´�
        destination = new Vector3(transform.position.x - _targetPos.x, 0, transform.position.z - _targetPos.z).normalized;
        currentTime = runTime;      // ���� �ൿ �ð� ����
        isWalking = false;
        isRunning = true;
        nav.speed = runSpeed;     // �ٱ� �ӵ� ����
        anim.SetBool("Running", isRunning);     // �ٱ� �ִϸ��̼� ����
    }

    // �θ�Ŭ���� �Լ��� �����ؼ� ����Ҷ� override �ؼ� ���
    public override void Damage(int _dmg, Vector3 _targetPos)       // ���� ������ �������� ������ ����
    {
        base.Damage(_dmg, _targetPos);
        if(!isDead)                         // ������ ���� �ʾ�����
            Run(_targetPos);                // �°� �پ �÷��̾� �ݴ�������� ���������� ����

    }
}
