using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private int hp;             // Ǯ ü��

    [SerializeField]
    private float destroyTime;      // ����Ʈ ���� ���ð�

    [SerializeField]
    private float force;            // �������� ������ ����(���� ����)

    [SerializeField]
    private GameObject go_hit_effect_prefab;        // Ÿ�� ȿ��

    // Ǯ �ȿ� �ִ� �������� �ִ� ���� Ǯ�ٵ��� Rigidbody�� BoxCollider�� Ȱ��ȭ ��Ű�� ���� ������
    private Rigidbody[] rigidbodys;                 // Rigidbody �迭
    private BoxCollider[] boxColliders;             // BoxCollider �迭

    [SerializeField]
    private string hit_sound;           // Ÿ�� ȿ����

    // Start is called before the first frame update
    void Start()
    {
        rigidbodys = this.transform.GetComponentsInChildren<Rigidbody>();       // �� ��ü�� �ڽĵ鿡 �ִ� Rigidbody������Ʈ ������ �����´�.
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();   // �� ��ü�� �ڽĵ鿡 �ִ� BoxCollider������Ʈ ������ �����´�. 
    }

    public void Damage()    // Ǯ�� �������� �ָ� �߻��ϴ� �Լ�
    {
        hp--;

        Hit();

        if(hp<=0)
        {
            Destruction();
        }
    }

    private void Hit()      // Ÿ�ݽ� �߻��ϴ� �Լ�
    {
        SoundManager.instance.PlaySE(hit_sound);        // Ÿ�� ȿ���� ���

        var clone = Instantiate(go_hit_effect_prefab, transform.position + Vector3.up, Quaternion.identity);    // Ÿ�� ����Ʈ ������ destroyTime����� ����
        Destroy(clone, destroyTime);
    }

    private void Destruction()      // �ļս� �߻��ϴ� �Լ�
    {
        for (int i = 0; i < rigidbodys.Length; i++)
        {
            rigidbodys[i].useGravity = true;                                       // �߷� ����
            rigidbodys[i].AddExplosionForce(force, transform.position, 1f);        // ������ �������� ������ ȿ�� ����(���� ����, ���� ��ġ, ���� �ݰ�)
            boxColliders[i].enabled = true;                                        // BoxCollider Ȱ��ȭ
        }

        Destroy(this.gameObject, destroyTime);
    }
}
