using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigWild : StrongAnimal
{
    // �θ�Ŭ���� �Լ��� �����ؼ� ����Ҷ� override �ؼ� ���
    protected override void Update()
    {
        base.Update();
        if (theViewAngle.View() && !isDead && !isAttacking)      // �þ߾ȿ� ������, ���� �ʾ�����, �������� �ƴϸ�
        {
            StopAllCoroutines();            // �ڷ�ƾ �ߺ�����
            StartCoroutine(ChaseTargetCoroutine());     // �÷��̾� �߰� �ڷ�ƾ ����
        }
    }
}
