using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float finishTime;

    private bool isHurt = false;            // 피해를 주었는지에 대한 여부(데미지 중복 방지)
    private bool isActivated = false;       // 트랩 작동 여부 

    public IEnumerator ActivatedTrapCoroutine()
    {   // 트랩 작동
        isActivated = true;

        yield return new WaitForSeconds(finishTime);
        isActivated = false;
        isHurt = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isActivated)         // 트랩이 작동중일때
        {
            if(!isHurt)         // 피해를 안주었을때
            {
                isHurt = true;  // 피해를 입음

                if(other.transform.name == "Player")        // 플레이어가 부딪힌다면
                {       // damage 만큼 HP 차감
                    other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                }
            }
        }
    }
}
