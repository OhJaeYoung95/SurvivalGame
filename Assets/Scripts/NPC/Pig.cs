using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeakAnimal
{
    // �θ�Ŭ���� �Լ��� �����ؼ� ����Ҷ� override �ؼ� ���
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();             // ���� �����ൿ
    }
    private void RandomAction()             // ���� ���� �ൿ
    {
        RandomSound();      // �ϻ� ���� ȿ���� ��� �Լ�����

        // Random.Range(x,y) int�� ��� x�� ���� y�� �������� �ʴ� ������ ���� �ش�.
        int _random = Random.Range(0, 4);       // ���, Ǯ���, �θ���, �ȱ�

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()         // ��� �Լ�
    {
        currentTime = waitTime;     // ���� �ൿ �ð� ����
        Debug.Log("���");
    }
    private void Eat()          // Ǯ ��� �Լ�
    {
        currentTime = waitTime;     // ���� �ൿ �ð� ����
        anim.SetTrigger("Eat");     // Ǯ��� �ִϸ��̼� ����
        Debug.Log("Ǯ ���");
    }
    private void Peek()         // �θ��� �Ÿ��� �Լ�
    {
        currentTime = waitTime;     // ���� �ൿ �ð� ����
        anim.SetTrigger("Peek");    // �θ��� �ִϸ��̼� ����
        Debug.Log("�θ���");
    }
}
