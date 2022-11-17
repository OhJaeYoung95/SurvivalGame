using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activated = false;

    [SerializeField]
    private Text text_Preview;      // 아이템 최대 버릴수 있는 개수
    [SerializeField]
    private Text text_Input;        // 아이템 버릴 개수
    [SerializeField]
    private InputField if_text;     // Input하는 Text를 초기화 하기 위한 변수

    [SerializeField]
    private GameObject go_Base;     // 아이템 버리는 갯수 창(인풋필드)

    [SerializeField]
    private ActionController thePlayer;       // 아이템 버릴 좌표를 얻기 위한 플레이어 카메라 위치

    void Update()
    {
        if(activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))        // 엔터 누르면 OK함수 실행
                OK();
            else if (Input.GetKeyDown(KeyCode.Escape))     // ESC키를 누르면 Cancel함수 실행  
                Cancel();
        }
    }

    public void Call()              // 아이템 버리는 갯수 창(인풋필드) 활성화
    {
        go_Base.SetActive(true);
        activated = true;            // 인풋창이 활성화 되었다는 Bool 값
        if_text.text = "";           // 아이템 버릴 개수 텍스트 내용 초기화
        text_Preview.text = DragSlot.instance.dragSlot.itemCount.ToString();        // 아이템 최대개수 값 설정(문자열로 변환),
    }

    public void Cancel()            // 아이템 버리는 갯수 창(인풋필드) 활성화
    {
        activated = false;            // 인풋창이 비활성화 되었다는 Bool 값
        go_Base.SetActive(false);
        DragSlot.instance.SetColor(0);          // 슬롯 비활성화
        DragSlot.instance.dragSlot = null;        // 슬롯 초기화
    }

    public void OK()                // OK버튼 누를때 나타나는 함수(아이템 지정 갯수 버리기)
    {
        DragSlot.instance.SetColor(0);          // 슬롯 비활성화

        int num;
        if (text_Input.text != "")               // 텍스트가 빈칸이 아니라면
        {
            if (CheckNumber(text_Input.text))        // 텍스트가 숫자라면
            {
                num = int.Parse(text_Input.text);    // text 문자형 -> int 형 변환
                if (num > DragSlot.instance.dragSlot.itemCount)     // 입력한 값의 숫자가 드래그한 아이템의 갯수보다 클 경우
                    num = DragSlot.instance.dragSlot.itemCount;     // 드래그한 아이템의 최대 갯수로 설정
            }
            else                        // 텍스트가 숫자가 아니라면
            {
                num = 1;            // 1개만 버리도록 설정
            }
        }
        else                    // 텍스트가 빈칸이라면
            num = int.Parse(text_Preview.text);     // 아이템 최대갯수 설정

        StartCoroutine(DropItemCoroutine(num));
    }

    IEnumerator DropItemCoroutine(int _num)
    {
        for (int i = 0; i < _num; i++)      // 아이템 개수 만큼 아이템 드롭
        {
            if(DragSlot.instance.dragSlot.item.itemPrefab != null)      // 아이템 프리팹이 존재하면
                Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, thePlayer.transform.position + thePlayer.transform.forward, Quaternion.identity);
            DragSlot.instance.dragSlot.SetSlotCount(-1);            // 슬롯아이템 개수 차감
            yield return new WaitForSeconds(0.05f);
        }

        DragSlot.instance.dragSlot = null;      // 슬롯 초기화
        go_Base.SetActive(false);               // 아이템 버리는 갯수 창(인풋필드)비활성화
        activated = false;            // 인풋창이 비활성화 되었다는 Bool 값

    }

    private bool CheckNumber(string _argString)         // 문자열이 숫자인지 확인해주는 함수
    {
        char[] _tempCharArray = _argString.ToCharArray();       // 문자열을 문자배열에 집어넣는다
        bool isNumber = true;
        for (int i = 0; i < _tempCharArray.Length; i++)
        {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57)     // 아스키코드 0에서9까지의 숫자에 든다면
                continue;   // 다음 for문을 돌아라
            isNumber = false;       // 문자가 모두 숫자라면 여기에 도달하지 않음
        }

        return isNumber;
    }
}
