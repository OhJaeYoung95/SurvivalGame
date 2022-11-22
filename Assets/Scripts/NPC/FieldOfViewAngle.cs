using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;         // 시야각 (120도)
    [SerializeField]
    private float viewDistance;      // 시야거리 (10미터)
    [SerializeField]
    private LayerMask targetMask;    // 타겟 마스크 (플레이어)

    private PlayerController thePlayer;
    private NavMeshAgent nav;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        nav = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetTargetPos()       // 플레이어 Vector값 반환
    {
        return thePlayer.transform.position;
    }



    public bool View()         // 시야(사거리)안에 플레이어가 들어왔는지 체크하는 Bool형 함수
    {
        // Physics.OverlapSphere() 일정 반경안에 있는 콜라이더들을 전부 뽑아내서 저장시키는 명령어
        // Physics.OverlapSphere(반경이 생성될 위치, 반경의 지름(반경이 퍼질 거리), 해당되는 콜라이더 조건(레이어마스크))
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for(int i =0; i<_target.Length; i++)
        {
            // 타겟의 정보를 가져온다
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")          // 반경안에 플레이어가 있을때
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
                            return true;        // 감지
                        }
                    }
                }
            }
            if(thePlayer.GetRun())      // 플레이어가 뛸 경우
            {
                if(CalcPathLength(thePlayer.transform.position) <= viewDistance)        // 플레이어와의 경로(거리)가 동물의 시야사거리보다 작을경우
                {
                    Debug.Log("돼지가 주변에서 뛰고 있는 플레이어의 움직임을 감지");
                    return true;        // 감지
                }
            }
        }
        return false;               // 감지 못함
    }

    private float CalcPathLength(Vector3 _targetPos)        // 장애물포함 경로 계산 함수
    {
        NavMeshPath _path = new NavMeshPath();      // 네비게이션 시스템에 의해서 계산된 경로
        nav.CalculatePath(_targetPos, _path);       // 타겟까지 가는 길을 계산해서 대입해준다

        Vector3[] _wayPoint = new Vector3[_path.corners.Length + 2];    // 코너값2개와 자기자신과 타겟의 위치를 포함

        _wayPoint[0] = transform.position;                      // 자기자신의 위치
        _wayPoint[_path.corners.Length + 1] = _targetPos;       // 타겟의 위치(배열의 마지막위치)

        float _pathLength = 0;
        for (int i = 0; i < _path.corners.Length; i++)
        {
            _wayPoint[i+1] =_path.corners[i];       // 웨이포인트에 경로를 넣음
            _pathLength += Vector3.Distance(_wayPoint[i], _wayPoint[i + 1]);    // 경로 계산
        }

        return _pathLength;
    }
}