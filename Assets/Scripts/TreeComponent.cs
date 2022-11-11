using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject[] go_treePieces;          // ���� ���� �����

    [SerializeField]
    private GameObject go_treeCenter;            // ���� �߾� ����

    [SerializeField]
    private GameObject go_Log_Prefab;           // �볪��

    [SerializeField]
    private float force;                        // ������ �������� �������� ������ ���� ����
    [SerializeField]
    private GameObject go_ChildTree;            // �ڽ� Ʈ��


    // ������ ���ܵǾ �������� ���� ���� CapsuleCollider�� ���� ����� ���� CapsuleCollider���� �浹 ������ On.Off ���־�� �Ѵ�.
    [SerializeField]
    private CapsuleCollider parentCol;           // �θ� ���� CapsuleCollider
    [SerializeField]
    private CapsuleCollider childCol;            // �ڽ� ���� CapsuleCollider 
    [SerializeField]
    private Rigidbody childRigid;                // �ڽ� ���� Rigidbody

    [SerializeField]
    private GameObject go_hit_effect_prefab;             // ������ ������ ����� ����Ʈ ȿ��

    [SerializeField]
    private float debrisDestroyTime;             // ���� ���� �ð�

    [SerializeField]
    private float destroyTime;                   // ���� ���� ���� �ð�

    // �ʿ��� ����
    [SerializeField]
    private string chop_sound;                  // ���� �д� ����
    [SerializeField]
    private string falldown_sound;              // ������ �������� ���� ����
    [SerializeField]
    private string logChange_sound;             // ������ �볪���� �ٲ�� ����

    public void Chop(Vector3 _pos, float angleY)      // ������ �Ӷ��� �Լ�
    {
        Hit(_pos);

        AngleCalc(angleY);

        if (CheckTreePieces())
            return;
        FallDownTree();
    }

    private void Hit(Vector3 _pos)          // ���� Ÿ�ݽ� �߻��ϴ� �Լ�
    {
        SoundManager.instance.PlaySE(chop_sound);       // Ÿ�� ȿ���� ���
                                                                                                        // ���� Ÿ�ݽ� �� �������� ����Ʈ ȿ�� �߻�
        GameObject clone = Instantiate(go_hit_effect_prefab, _pos, Quaternion.Euler(Vector3.zero));     // Quaternion.Euler(Vector3.zero) = Quaternion.identity ���� ���� �ǹ�
        Destroy(clone, debrisDestroyTime);
    }

    private void AngleCalc(float _angleY)
    {
        Debug.Log(_angleY);
        if (0 <= _angleY && _angleY <= 70)
            DestroyPiece(2);
        else if (70 <= _angleY && _angleY <= 140)
            DestroyPiece(3);
        else if (140 <= _angleY && _angleY <= 210)
            DestroyPiece(4);
        else if (210 <= _angleY && _angleY <= 280)
            DestroyPiece(0);
        else if (280 <= _angleY && _angleY <= 360)
            DestroyPiece(1);
    }

    private void DestroyPiece(int _num)
    {
        if(go_treePieces[_num].gameObject != null)
        {
            // ���� Ÿ�ݽ� �� �������� ����Ʈ ȿ�� �߻�
            GameObject clone = Instantiate(go_hit_effect_prefab, go_treePieces[_num].transform.position, Quaternion.Euler(Vector3.zero));     // Quaternion.Euler(Vector3.zero) = Quaternion.identity ���� ���� �ǹ�
            Destroy(clone, debrisDestroyTime);

            Destroy(go_treePieces[_num].gameObject);
        }
    }

    private bool CheckTreePieces()
    {
        for (int i = 0; i < go_treePieces.Length; i++)
        {
            if(go_treePieces[i].gameObject != null)
            {
                return true;
            }
        }
        return false;
    }

    private void FallDownTree()     // ������ ����Ʈ���� �Լ�
    {
        SoundManager.instance.PlaySE(falldown_sound);       // ���� �������� ȿ���� ���

        Destroy(go_treeCenter);     // ���� �߾� ���� ����

        parentCol.enabled = false;      // �θ� ���� �ݶ��̴� ��Ȱ��ȭ
        childCol.enabled = true;        // �ڽ� ���� �ݶ��̴� Ȱ��ȭ
        childRigid.useGravity = true;   // �ڽ� ���� Rigidbody �߷� Ȱ��ȭ

        childRigid.AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));      // �ڽ� ������ ������ �������� ���ֱ�


        StartCoroutine(LogCoroutine());
    }

    IEnumerator LogCoroutine()      // �볪�� ���� �ڷ�ƾ
    {

        yield return new WaitForSeconds(destroyTime);

        SoundManager.instance.PlaySE(logChange_sound);      // �볪�� ���� ȿ���� ���

        // �볪�� ����
        Instantiate(go_Log_Prefab, go_ChildTree.transform.position + (go_ChildTree.transform.up * 3f), Quaternion.LookRotation(go_ChildTree.transform.up));
        Instantiate(go_Log_Prefab, go_ChildTree.transform.position + (go_ChildTree.transform.up * 6f), Quaternion.LookRotation(go_ChildTree.transform.up));
        Instantiate(go_Log_Prefab, go_ChildTree.transform.position + (go_ChildTree.transform.up * 9f), Quaternion.LookRotation(go_ChildTree.transform.up));

        Destroy(go_ChildTree.gameObject);          // �ڽ� ���� ����
    }

    public Vector3 GetTreeCenterPosition()
    {
        return go_treeCenter.transform.position;
    }
}
