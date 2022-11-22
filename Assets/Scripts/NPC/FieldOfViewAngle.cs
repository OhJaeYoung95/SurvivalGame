using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;         // �þ߰� (120��)
    [SerializeField]
    private float viewDistance;      // �þ߰Ÿ� (10����)
    [SerializeField]
    private LayerMask targetMask;    // Ÿ�� ����ũ (�÷��̾�)

    private PlayerController thePlayer;
    private NavMeshAgent nav;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        nav = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetTargetPos()       // �÷��̾� Vector�� ��ȯ
    {
        return thePlayer.transform.position;
    }



    public bool View()         // �þ�(��Ÿ�)�ȿ� �÷��̾ ���Դ��� üũ�ϴ� Bool�� �Լ�
    {
        // Physics.OverlapSphere() ���� �ݰ�ȿ� �ִ� �ݶ��̴����� ���� �̾Ƴ��� �����Ű�� ��ɾ�
        // Physics.OverlapSphere(�ݰ��� ������ ��ġ, �ݰ��� ����(�ݰ��� ���� �Ÿ�), �ش�Ǵ� �ݶ��̴� ����(���̾��ũ))
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for(int i =0; i<_target.Length; i++)
        {
            // Ÿ���� ������ �����´�
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")          // �ݰ�ȿ� �÷��̾ ������
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;  // Ÿ�ٿ��� ������������ ����,�������� ����ȭ
                // Vector3.Angle(a,b) a�� b ������ ������ �����ش�
                float _angle = Vector3.Angle(_direction, transform.forward);
            
                if(_angle < viewAngle * 0.5f)       // �þ߰� �ȿ� �ִٸ�
                {
                    RaycastHit _hit;
                    // Physics.Raycast(������ ��� ��ġ, ��� ����, �ε��� ��ü����, ������ �����Ÿ�)
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if(_hit.transform.name == "Player")     // �÷��̾ ������ �ε����ٸ�
                        {
                            Debug.Log("�÷��̾ ���� �þ� ���� �ֽ��ϴ�");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            return true;        // ����
                        }
                    }
                }
            }
            if(thePlayer.GetRun())      // �÷��̾ �� ���
            {
                if(CalcPathLength(thePlayer.transform.position) <= viewDistance)        // �÷��̾���� ���(�Ÿ�)�� ������ �þ߻�Ÿ����� �������
                {
                    Debug.Log("������ �ֺ����� �ٰ� �ִ� �÷��̾��� �������� ����");
                    return true;        // ����
                }
            }
        }
        return false;               // ���� ����
    }

    private float CalcPathLength(Vector3 _targetPos)        // ��ֹ����� ��� ��� �Լ�
    {
        NavMeshPath _path = new NavMeshPath();      // �׺���̼� �ý��ۿ� ���ؼ� ���� ���
        nav.CalculatePath(_targetPos, _path);       // Ÿ�ٱ��� ���� ���� ����ؼ� �������ش�

        Vector3[] _wayPoint = new Vector3[_path.corners.Length + 2];    // �ڳʰ�2���� �ڱ��ڽŰ� Ÿ���� ��ġ�� ����

        _wayPoint[0] = transform.position;                      // �ڱ��ڽ��� ��ġ
        _wayPoint[_path.corners.Length + 1] = _targetPos;       // Ÿ���� ��ġ(�迭�� ��������ġ)

        float _pathLength = 0;
        for (int i = 0; i < _path.corners.Length; i++)
        {
            _wayPoint[i+1] =_path.corners[i];       // ��������Ʈ�� ��θ� ����
            _pathLength += Vector3.Distance(_wayPoint[i], _wayPoint[i + 1]);    // ��� ���
        }

        return _pathLength;
    }
}