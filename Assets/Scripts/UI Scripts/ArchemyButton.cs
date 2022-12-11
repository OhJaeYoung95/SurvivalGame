using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArchemyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private ArchemyTable theArchemy;    // UI 툴팁창 활성화를 위한 컴포넌트
    [SerializeField]
    private int buttonNum;          // 해당 툴팁창을 띄우기 위한 버튼 번호

    // 현재 스크립트를 컴포넌트로 가지고 있는 오브젝트에 마우스 포인터가 들어올때 발생하는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 버튼 번호에 해당하는 UI 툴팁창 활성화
        theArchemy.ShowToolTip(buttonNum);
    }

    // 현재 스크립트를 컴포넌트로 가지고 있는 오브젝트에서 마우스 포인터가 빠져나갈때  발생하는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        // UI 툴팁창 비활성화
        theArchemy.HideToolTip();

    }
}
