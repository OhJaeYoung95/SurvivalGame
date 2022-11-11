using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private int hp;             // 풀 체력

    [SerializeField]
    private float destroyTime;      // 이펙트 제거 대기시간

    [SerializeField]
    private float force;            // 나뭇잎이 퍼지는 세기(폭발 세기)

    [SerializeField]
    private GameObject go_hit_effect_prefab;        // 타격 효과

    // 풀 안에 있는 나뉘어져 있는 작은 풀잎들의 Rigidbody와 BoxCollider를 활성화 시키기 위한 변수들
    private Rigidbody[] rigidbodys;                 // Rigidbody 배열
    private BoxCollider[] boxColliders;             // BoxCollider 배열

    [SerializeField]
    private string hit_sound;           // 타격 효과음

    // Start is called before the first frame update
    void Start()
    {
        rigidbodys = this.transform.GetComponentsInChildren<Rigidbody>();       // 이 객체의 자식들에 있는 Rigidbody컴포넌트 정보를 가져온다.
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();   // 이 객체의 자식들에 있는 BoxCollider컴포넌트 정보를 가져온다. 
    }

    public void Damage()    // 풀에 데미지를 주면 발생하는 함수
    {
        hp--;

        Hit();

        if(hp<=0)
        {
            Destruction();
        }
    }

    private void Hit()      // 타격시 발생하는 함수
    {
        SoundManager.instance.PlaySE(hit_sound);        // 타격 효과음 재생

        var clone = Instantiate(go_hit_effect_prefab, transform.position + Vector3.up, Quaternion.identity);    // 타격 이펙트 생성후 destroyTime대기후 제거
        Destroy(clone, destroyTime);
    }

    private void Destruction()      // 파손시 발생하는 함수
    {
        for (int i = 0; i < rigidbodys.Length; i++)
        {
            rigidbodys[i].useGravity = true;                                       // 중력 생성
            rigidbodys[i].AddExplosionForce(force, transform.position, 1f);        // 나뭇잎 조각들이 퍼지는 효과 구현(폭발 세기, 폭발 위치, 폭발 반경)
            boxColliders[i].enabled = true;                                        // BoxCollider 활성화
        }

        Destroy(this.gameObject, destroyTime);
    }
}
