using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOnFire : MonoBehaviour
{
    [SerializeField]
    private float time;                 // 익히거나 타는데 걸리는 시간
    private float currentTime;          // 실제 걸리는 시간

    private bool done;                  // 조리가 끝났으면(탔거나 익었거나),
                                        // 더이상 불에 있어도 계산 무시할 수 있게 해주는 bool형 변수

    [SerializeField]
    private GameObject go_CookedItemPrefab;   // 익혀진 혹은 탄 아이템 교체

    // 콜라이더 안에 위치해있으면 발동하는 함수
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Fire" && !done)  // 불에 있을때 && 조리가 끝나지 않았을때(탔거나 익었거나),
        {
            currentTime += Time.deltaTime;      // 실제 걸리는 시간에 1초에 1씩 증가

            if(currentTime >= time)     // 해당 시간(익히거나 타는데 걸리는 시간)에 도달한 경우
            {
                done = true;            // 조리 끝
                // 조리한 아이템 생성,  자기자신의 회전값을 가진 프리팹생성
                Instantiate(go_CookedItemPrefab, transform.position, Quaternion.Euler(transform.eulerAngles));
                Destroy(gameObject);        // 조리할때 넣었던 오브젝트 파괴
            }

        }
    }
}
