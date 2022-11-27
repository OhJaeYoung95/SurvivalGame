using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOnFire : MonoBehaviour
{
    [SerializeField]
    private float time;                 // �����ų� Ÿ�µ� �ɸ��� �ð�
    private float currentTime;          // ���� �ɸ��� �ð�

    private bool done;                  // ������ ��������(���ų� �;��ų�),
                                        // ���̻� �ҿ� �־ ��� ������ �� �ְ� ���ִ� bool�� ����

    [SerializeField]
    private GameObject go_CookedItemPrefab;   // ������ Ȥ�� ź ������ ��ü

    // �ݶ��̴� �ȿ� ��ġ�������� �ߵ��ϴ� �Լ�
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Fire" && !done)  // �ҿ� ������ && ������ ������ �ʾ�����(���ų� �;��ų�),
        {
            currentTime += Time.deltaTime;      // ���� �ɸ��� �ð��� 1�ʿ� 1�� ����

            if(currentTime >= time)     // �ش� �ð�(�����ų� Ÿ�µ� �ɸ��� �ð�)�� ������ ���
            {
                done = true;            // ���� ��
                // ������ ������ ����,  �ڱ��ڽ��� ȸ������ ���� �����ջ���
                Instantiate(go_CookedItemPrefab, transform.position, Quaternion.Euler(transform.eulerAngles));
                Destroy(gameObject);        // �����Ҷ� �־��� ������Ʈ �ı�
            }

        }
    }
}
