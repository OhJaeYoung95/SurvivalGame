using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : MonoBehaviour
{
    [SerializeField]
    private int hp;         // �������� ü��

    [SerializeField]
    private float destroyTime;          // ����Ʈ ���� ���ð�

    [SerializeField]
    private GameObject go_little_Twig;              // �������� �ı��� ������ ���� �������� ����������

    [SerializeField]
    private GameObject go_hit_effect_prefab;        // Ÿ�� ����Ʈ

    // ȸ���� ����
    private Vector3 originRot;                      // ���� ����
    private Vector3 wantedRot;                      // ȸ���Ǳ� ���ϴ� ����
    private Vector3 currentRot;                     // ���� ����


    // �ʿ��� ���� �̸�
    [SerializeField]
    private string hit_Sound;                       // Ÿ�� ȿ����
    [SerializeField]
    private string broken_Sound;                    // �ļ� ȿ����

    // Start is called before the first frame update
    void Start()
    {
        originRot = this.transform.rotation.eulerAngles;        // �Ϲ� transform.rotation�� Quaternion���̰� Vector3�� eulurAngles ���̱⿡ ��ȯ�����ش�
        currentRot = originRot;                                 // ���� ������ ���� ���⿡ �ʱ�ȭ
    }

    // �÷��̾ ���������� �������� �Լ�
    public void Damage(Transform _playerTf)         // �÷��̾ ���� �ݴ� �������� Ƣ�� �ϱ� ���� ��ġ��(�Ķ����)
    {
        hp--;

        Hit();
        StartCoroutine(HitSwayCoroutine(_playerTf));

        if(hp<=0)
        {
            Destruction();                   // �ı��Լ�
        }
    }

    private void Hit()      // �������� Ÿ�ݽ� ȿ�� �Լ�
    {
        SoundManager.instance.PlaySE(hit_Sound);                // Ÿ�� ȿ���� ���

        GameObject clone = Instantiate(go_hit_effect_prefab,                                        // Ÿ�� ����Ʈ ����
                                       gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),        // �������� �ڽ��ݶ��̴��� �߾ӿ��� ��¦ ���ʿ��� ����
                                       Quaternion.identity);

        Destroy(clone, destroyTime);        // destroyTime �ð� �Ŀ� Ÿ�� ����Ʈ ����
    }

    IEnumerator HitSwayCoroutine(Transform _target)
    {
        Vector3 direction = (_target.position - transform.position).normalized;     // ���������� �÷��̾ �ٶ󺸴� ����, normalized = ����ȭ

        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;       // direction ������ �ٶ󺸰� ������ִ� �Լ�

        CheckDirection(rotationDir);

        while(!CheckThreshold())            // ���������� �������� ����
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.04f);            // ���ϴ� �������� �ε巴�� �̵�
            transform.rotation = Quaternion.Euler(currentRot);                  // ���� ���⿡ ����
            yield return null;
        }

        wantedRot = originRot;                  // �ٽ� ���ư����� �� ����

        while (!CheckThreshold())           // // ���������� ���ư��� ����
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.02f);            // ���ϴ� �������� �ε巴�� �̵�
            transform.rotation = Quaternion.Euler(currentRot);                  // ���� ���⿡ ����
            yield return null;
        }
    }

    private bool CheckThreshold()           // �Ӱ��� Ȯ�� �Լ�
    {
        if(Mathf.Abs(wantedRot.x - currentRot.x) <= 0.5f && Mathf.Abs(wantedRot.z - currentRot.z) <= 0.5f)  // ���ϴ� ���� �����ߴ��� Ȯ��
            return true;
        return false;
    }

    private void CheckDirection(Vector3 _rotationDir)           // ���������� �������� ������ �����ִ� �Լ�
    {
        Debug.Log(_rotationDir);

        if(_rotationDir.y > 180)                                // �÷��̾ ġ�� ���⿡ ���� ���������� �������� ���� ����
        {
            if (_rotationDir.y > 300)
                wantedRot = new Vector3(-50f,0,-50f);
            else if (_rotationDir.y > 240)
                wantedRot = new Vector3(0, 0, -50f);
            else
                wantedRot = new Vector3(50f, 0, -50f);
        }
        else if(_rotationDir.y <= 180)
        {
            if (_rotationDir.y < 60)
                wantedRot = new Vector3(-50f,0, 50f);
            else if (_rotationDir.y < 120)
                wantedRot = new Vector3(0, 0, 50f);
            else
                wantedRot = new Vector3(50f, 0, 50f);
        }
    }

    private void Destruction()      // �������� �ļս� ������ �Լ�
    {
        SoundManager.instance.PlaySE(broken_Sound);         // �ļ� ȿ���� ���

        GameObject clone1 = Instantiate(go_little_Twig,                                                     // �ļ� ����Ʈ ����
                               gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),        // �������� �ڽ��ݶ��̴��� �߾ӿ��� ��¦ ���ʿ��� ����
                               Quaternion.identity);
        GameObject clone2 = Instantiate(go_little_Twig,                                                     // �ļ� ����Ʈ ����
                               gameObject.GetComponent<BoxCollider>().bounds.center - (Vector3.up * 0.5f),        // �������� �ڽ��ݶ��̴��� �߾ӿ��� ��¦ ���ʿ��� ����
                               Quaternion.identity);

        Destroy(clone1,destroyTime);            // �������� ���� 2�� ���� �� destriyTime ���� ����
        Destroy(clone2, destroyTime);   
        Destroy(gameObject);                    // ���� �������� ����
    }
}
