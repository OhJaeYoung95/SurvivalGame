using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public static bool isActivated = true;      // 무기 흔들림 활성화시

    // 기존 위치
    private Vector3 originPos;

    // 현재 위치
    private Vector3 currentPos;

    // sway(무기 흔들림) 한계
    [SerializeField]
    private Vector3 LimitPos;

    // 정조준 sway(무기 흔들림) 한계
    [SerializeField]
    private Vector3 fineSightLimitPos;

    // 부드러운 움직임 정도
    [SerializeField]
    private Vector3 smoothSway;


    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;     // 정조준 상태의 값을 받아오기 위한 GunController
    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Inventory.inventoryActivated && isActivated)       // 인벤토리 비활성화시에만 && 무기 흔들림 활성화시
            TrySway();
    }

    private void TrySway()
    {
        if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if(!theGunController.isFineSightMode)       // 정조준 상태 아닐때의 총기 흔들림
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -LimitPos.x, LimitPos.x),
               Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -LimitPos.y, LimitPos.y),
               originPos.z);
        }
        else                                // 정조준 상태일떄의 총기 흔들림                            
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                           originPos.z);

        }
        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.y);
        transform.localPosition = currentPos;
    }
}
