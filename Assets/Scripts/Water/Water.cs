using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    [SerializeField]
    private float waterDrag;        // 물속 중력
    private float originDrag;       // 초기 중력

    [SerializeField] 
    private Color waterColor;          // 물속 색깔
    [SerializeField] 
    private float waterFogDensity;     // 물 탁함 정도

    [SerializeField]
    private Color waterNightColor;          // 밤 상태의 물속 색깔
    [SerializeField]
    private float waterNightFogDensity;     // 밤 상태의 물 탁함 정도


    // 초기값
    private Color originColor;          // 초기 색깔
    private float originFogDensity;     // 초기 탁함정도
    [SerializeField]
    private Color originNightColor;     // 초기 밤상태의 색깔
    [SerializeField]
    private float originNightFogDensity;    // 초기 밤상태의 탁함 정도

    [SerializeField]
    private string sound_WaterOut;      // 물에서 나올때 효과음
    [SerializeField]    
    private string sound_WaterIn;       // 물에 들어갈때 효과음
    [SerializeField]
    private string sound_Breathe;       // 호흡할때 효과음

    [SerializeField]
    private float breatheTIme;          // 호흡 시간
    private float currentBreatheTime;   // 실제 호흡 계산 시간

    [SerializeField]
    private float totalOxygen;      // 산소량
    private float currentOxygen;    // 산소량 계산값
    private float temp;         // 산소가 없을때 HP감소를 위한 시간변수

    [SerializeField]
    private GameObject go_BaseUI;       // 산소UI창
    [SerializeField]
    private Text text_totalOxygen;      // 산소량 텍스트
    [SerializeField]
    private Text text_currentOxygen;    // 현재산소량 텍스트
    [SerializeField]
    private Image image_gauge;          // 산소량 게이지

    private StatusController thePlayerStat;

    // Start is called before the first frame update
    void Start()
    {   // 초기값 설정
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;
        thePlayerStat = FindObjectOfType<StatusController>();
        currentOxygen = totalOxygen;
        text_totalOxygen.text = totalOxygen.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.isWater)     // 물속에 있을때
        {
            currentBreatheTime += Time.deltaTime;       // 실제 호흡 계산 시간에 1초에 1씩 더함
            if(currentBreatheTime >= breatheTIme)       // 실제 호흡 계산 시간이 호흡시간에 도달하면
            {
                SoundManager.instance.PlaySE(sound_Breathe);    // 호흡할때 효과음 재생
                currentBreatheTime = 0;     // 실제 호흡 계산 시간 초기화
            }
        }

        DecreaseOxygen();   // 산소 감수
    }

    private void DecreaseOxygen()   // 산소 감소 함수
    {
        if(GameManager.isWater)     // 물속에 있을때
        {
            if(currentOxygen >= 0)  // 현재 산소량이 0보다 크다면
            {
                currentOxygen -= Time.deltaTime;        // 현재 산소량 1초에 1씩 감소
            }
            text_currentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();     // 현재 산소량 텍스트화, 반올림한후 Int로 형변환후 문자열화
            image_gauge.fillAmount = currentOxygen / totalOxygen;   // 현재 산소량 게이지화

            if(currentOxygen <= 0)      // 현재 산소량이 0보다 작아지면
            {
                temp += Time.deltaTime;     // temp 1초에 1씩 증가
                if(temp >= 1)   // 1초마다 HP가 닳게 하기위한 장치
                {
                    thePlayerStat.DecreaseHP(1);    // HP감소
                    temp = 0;       // temp 초기화
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetWater(Collider _player)     // 물에 들어갈때 발생하는 함수
    {
        SoundManager.instance.PlaySE(sound_WaterIn);    // 물에 들어갈때 효과음 재생

        go_BaseUI.SetActive(true);      // 산소 UI창 활성화
        GameManager.isWater = true;     // 물속에 들어감
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;   // 물속 중력 대입

        if(!GameManager.isNight)        // 낮일 경우
        {
            RenderSettings.fogColor = waterColor;                   // 물속 색 대입
            RenderSettings.fogDensity = waterFogDensity;            // 물 탁함 정도 대입
        }
        else                // 밤일 경우
        {
            RenderSettings.fogColor = waterNightColor;                   // 밤 상태의 물속 색 대입
            RenderSettings.fogDensity = waterNightFogDensity;            // 밤 상태의 물 탁함 정도 대입
        }
    }
    private void GetOutWater(Collider _player)  // 물에서 나올때 발생하는 함수
    {
        if(GameManager.isWater)     // 물속에 있을때
        {
            go_BaseUI.SetActive(false);     // 산소 UI창 비활성화
            currentOxygen = totalOxygen;        // 현재산소량 초기화, 숨 쉼
            SoundManager.instance.PlaySE(sound_WaterOut);   // 물에서 나올때 효과음 재생

            GameManager.isWater = false;        // 물에서 나옴
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;   // 초기 중력 대입

            if(!GameManager.isNight)        // 낮인 경우
            {
                RenderSettings.fogColor = originColor;                   // 초기 색 대입
                RenderSettings.fogDensity = originFogDensity;            // 초기 탁함 정도 대입
            }
            else                            // 밤인 경우
            {
                RenderSettings.fogColor = originNightColor;                   // 초기 밤때의 색 대입
                RenderSettings.fogDensity = originNightFogDensity;            // 초기 밤때의 탁함 정도 대입
            }
        }
    }
}
