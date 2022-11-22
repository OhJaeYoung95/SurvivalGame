using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;         // 시야각 (120도)
    [SerializeField]
    private float viewDistance;      // 시야거리 (10미터)
    [SerializeField]
    private LayerMask targetMask;    // 타겟 마스크 (플레이어)

    // 필요한 컴포넌트
    private Pig thePig;         // 돼지 행동 제어를 위한 변수

    void Start()
    {
        thePig = GetComponent<Pig>();
    }

    // Update is called once per frame
    void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)     // 시야 구하는 공식
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()         // 시야
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);           // 왼쪽시야
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);           // 오른쪽시야

        // 디버깅 용으로 레이저를 쏘는 기능, 게임상x 씬상 o
        // DrawRay(레이저가 나갈 위치, 레이저가 나아갈 방향, 레이저색상)
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        // Physics.OverlapSphere() 일정 반경안에 있는 콜라이더들을 전부 뽑아내서 저장시키는 명령어
        // Physics.OverlapSphere(반경이 생성될 위치, 반경의 지름(반경이 퍼질 거리), 해당되는 콜라이더 조건(레이어마스크))
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for(int i =0; i<_target.Length; i++)
        {
            // 타겟의 정보를 가져온다
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;  // 타겟에서 돼지쪽으로의 방향,방향으로 정규화
                // Vector3.Angle(a,b) a와 b 사이의 각도를 구해준다
                float _angle = Vector3.Angle(_direction, transform.forward);
            
                if(_angle < viewAngle * 0.5f)       // 시야각 안에 있다면
                {
                    RaycastHit _hit;
                    // Physics.Raycast(레이저 쏘는 위치, 쏘는 방향, 부딪힌 물체정보, 레이저 사정거리)
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if(_hit.transform.name == "Player")     // 플레이어에 광선이 부딪혔다면
                        {
                            Debug.Log("플레이어가 돼지 시야 내에 있습니다");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            thePig.Run(_hit.transform.position);
                        }
                    }
                }

            }
        }
    }
}
