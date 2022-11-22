using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;         // �þ߰� (120��)
    [SerializeField]
    private float viewDistance;      // �þ߰Ÿ� (10����)
    [SerializeField]
    private LayerMask targetMask;    // Ÿ�� ����ũ (�÷��̾�)

    // �ʿ��� ������Ʈ
    private Pig thePig;         // ���� �ൿ ��� ���� ����

    void Start()
    {
        thePig = GetComponent<Pig>();
    }

    // Update is called once per frame
    void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)     // �þ� ���ϴ� ����
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()         // �þ�
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);           // ���ʽþ�
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);           // �����ʽþ�

        // ����� ������ �������� ��� ���, ���ӻ�x ���� o
        // DrawRay(�������� ���� ��ġ, �������� ���ư� ����, ����������)
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        // Physics.OverlapSphere() ���� �ݰ�ȿ� �ִ� �ݶ��̴����� ���� �̾Ƴ��� �����Ű�� ��ɾ�
        // Physics.OverlapSphere(�ݰ��� ������ ��ġ, �ݰ��� ����(�ݰ��� ���� �Ÿ�), �ش�Ǵ� �ݶ��̴� ����(���̾��ũ))
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for(int i =0; i<_target.Length; i++)
        {
            // Ÿ���� ������ �����´�
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
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
                            thePig.Run(_hit.transform.position);
                        }
                    }
                }

            }
        }
    }
}
