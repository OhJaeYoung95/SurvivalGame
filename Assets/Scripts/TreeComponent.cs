using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject[] go_treePieces;          // 깎일 나무 파편들

    [SerializeField]
    private GameObject go_treeCenter;            // 나무 중앙 파편

    [SerializeField]
    private GameObject go_Log_Prefab;           // 통나무

    [SerializeField]
    private float force;                        // 나무가 쓰러질때 랜덤으로 가해질 힘의 세기
    [SerializeField]
    private GameObject go_ChildTree;            // 자식 트리


    // 나무가 절단되어서 쓰러질때 기존 나무 CapsuleCollider와 새로 생기는 나무 CapsuleCollider와의 충돌 때문에 On.Off 해주어야 한다.
    [SerializeField]
    private CapsuleCollider parentCol;           // 부모 나무 CapsuleCollider
    [SerializeField]
    private CapsuleCollider childCol;            // 자식 나무 CapsuleCollider 
    [SerializeField]
    private Rigidbody childRigid;                // 자식 나무 Rigidbody

    [SerializeField]
    private GameObject go_hit_effect_prefab;             // 나무를 찍을때 생기는 이펙트 효과

    [SerializeField]
    private float debrisDestroyTime;             // 파편 제거 시간

    [SerializeField]
    private float destroyTime;                   // 나무 몸통 제거 시간

    // 필요한 사운드
    [SerializeField]
    private string chop_sound;                  // 나무 패는 사운드
    [SerializeField]
    private string falldown_sound;              // 나무가 쓰러질때 나는 사운드
    [SerializeField]
    private string logChange_sound;             // 나무가 통나무로 바뀌는 사운드

    public void Chop(Vector3 _pos, float angleY)      // 나무를 팰때의 함수
    {
        Hit(_pos);

        AngleCalc(angleY);

        if (CheckTreePieces())
            return;
        FallDownTree();
    }

    private void Hit(Vector3 _pos)          // 나무 타격시 발생하는 함수
    {
        SoundManager.instance.PlaySE(chop_sound);       // 타격 효과음 재생
                                                                                                        // 나무 타격시 그 지점에서 이펙트 효과 발생
        GameObject clone = Instantiate(go_hit_effect_prefab, _pos, Quaternion.Euler(Vector3.zero));     // Quaternion.Euler(Vector3.zero) = Quaternion.identity 거의 같은 의미
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
            // 나무 타격시 그 지점에서 이펙트 효과 발생
            GameObject clone = Instantiate(go_hit_effect_prefab, go_treePieces[_num].transform.position, Quaternion.Euler(Vector3.zero));     // Quaternion.Euler(Vector3.zero) = Quaternion.identity 거의 같은 의미
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

    private void FallDownTree()     // 나무를 쓰러트리는 함수
    {
        SoundManager.instance.PlaySE(falldown_sound);       // 나무 쓰러지는 효과음 재생

        Destroy(go_treeCenter);     // 나무 중앙 파편 제거

        parentCol.enabled = false;      // 부모 나무 콜라이더 비활성화
        childCol.enabled = true;        // 자식 나무 콜라이더 활성화
        childRigid.useGravity = true;   // 자식 나무 Rigidbody 중력 활성화

        childRigid.AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));      // 자식 나무에 랜덤한 방향으로 힘주기


        StartCoroutine(LogCoroutine());
    }

    IEnumerator LogCoroutine()      // 통나무 생성 코루틴
    {

        yield return new WaitForSeconds(destroyTime);

        SoundManager.instance.PlaySE(logChange_sound);      // 통나무 생성 효과음 재생

        // 통나무 생성
        Instantiate(go_Log_Prefab, go_ChildTree.transform.position + (go_ChildTree.transform.up * 3f), Quaternion.LookRotation(go_ChildTree.transform.up));
        Instantiate(go_Log_Prefab, go_ChildTree.transform.position + (go_ChildTree.transform.up * 6f), Quaternion.LookRotation(go_ChildTree.transform.up));
        Instantiate(go_Log_Prefab, go_ChildTree.transform.position + (go_ChildTree.transform.up * 9f), Quaternion.LookRotation(go_ChildTree.transform.up));

        Destroy(go_ChildTree.gameObject);          // 자식 나무 제거
    }

    public Vector3 GetTreeCenterPosition()
    {
        return go_treeCenter.transform.position;
    }
}
