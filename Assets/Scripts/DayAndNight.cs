using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float secondPerRealTimeSecond;      // 게임 세계의 100초 = 현실 세계의 1초

    [SerializeField]
    private float fogDensityCalc;           // Fog 증감량 비율

    [SerializeField]
    private float nightFogDensity;          // 밤 상태의 Fog 밀도
    private float dayFogDensity;            // 낮 상태의 Fog 밀도
    private float currentFogDensity;        // 현재 Fog 밀도 계산

    // Start is called before the first frame update
    void Start()
    {   // 낮상태의 Fog 밀도값 설정
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {   // 해당 컴포넌트를 지닌 태양이 회전하게 해준다
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)    // 밤일때
            GameManager.isNight = true;
        else if (transform.eulerAngles.x >= 340) // 낮일때
            GameManager.isNight = false;

        if(GameManager.isNight)     // 밤일경우
        {   // 현재 Fog밀도가 밤 상태의 Fog 밀도보다 작을경우
            if(currentFogDensity <= nightFogDensity)
            {   // 현재 Fog밀도를 1초에 Fog증감량 만큼 증가, 증가한 값 세팅
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else            // 낮일 경우
        {
            // 현재 Fog밀도가 낮 상태의 Fog 밀도보다 클경우
            if(currentFogDensity >= dayFogDensity)
            {   // 현재 Fog밀도를 1초에 Fog증감량 만큼 감소, 감소한 값 세팅
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
