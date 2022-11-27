using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    public Building.Type needType;      // 설치할 때 필요한 건물 타입
    private bool needTypeFlag;          // 설치 가능 여부

    // 충돌한 오브젝트의 콜라이더를 저장하는 리스트로 활용
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]
    private int layerGround;    // 지상(땅) 레이어
    private const int IGNORE_RAYCAST_LAYER = 2;         // Ignore Raycast가 레이어창에서 2번째에 속한다

    [SerializeField]
    private Material green;     // 미리보기 설치 가능색
    [SerializeField]
    private Material red;       // 미리보기 설치 불가능색

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()          // 미리보기 색상을 바꿔주는 함수
    {
        if(needType == Building.Type.Normal)    // 건물 타입이 Normal인 경우
        {
            if (colliderList.Count > 0)     // 콜라이더 영역안에 들어온 콜라이더가 있을경우
                SetColor(red); // 미리보기 색상을 빨간색으로 입혀준다
            else                            // 콜라디어 영역안에 들어온 콜라이더가 없을경우
                SetColor(green); // 미리보기 색상을 초록색으로 입혀준다
        }
        else                // 건물 타입이 Noraml외의 경우
        {
            // 콜라이더 영역안에 들어온 콜라이더가 있을경우 || 설치 불가능일 경우
            if (colliderList.Count > 0 || !needTypeFlag)     
                SetColor(red); // 미리보기 색상을 빨간색으로 입혀준다
            else                            // 콜라디어 영역안에 들어온 콜라이더가 없을경우
                SetColor(green); // 미리보기 색상을 초록색으로 입혀준다
        }
    }

    private void SetColor(Material mat)     // 미리보기 색상을 정해주는 함수
    {
        // 이 컴포넌트를 가진 객체의 Transform을 전부 돌면서 반복
        foreach (Transform tf_Child in this.transform)
        {
            // 기존의 materials의 정보를 바꾸기 위한 임시 객체에 정보 대입
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

            // materials의 길이만큼 반복
            for (int i = 0; i < newMaterials.Length; i++)
            {
                // 매개변수로 가져온 색상으로 색을 바꾸는 작업
                newMaterials[i] = mat;
            }
            // 기존의 materials의 색상을 실제로 매개변수의 색상으로 대입하는 작업
            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    // 콜라이더 영역안에 들어올 때 발동
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Structure")      // 들어온 오브젝트의 태그가 "Structure"일때
        {
            // 설치할 때 필요한 건물 타입이 아니라면
            if (other.GetComponent<Building>().type != needType)
                colliderList.Add(other);    // 콜라이더 영역안으로 들어오면 리스트에 추가
            else
                needTypeFlag = true;        // 설치 가능
        }
        else                     // 들어온 오브젝트의 태그가 "Structure" 아닐때
        {
            // 충돌한 오브젝트의 레이어가 그라운드(땅)가 아닐때 && Ignore Raycast레이어가 아닐때
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
                colliderList.Add(other);                // 콜라이더 영역안으로 들어오면 리스트에 추가
        }
    }

    // 콜라이더 영역에서 나갈 때 발동
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Structure")      // 나간 오브젝트의 태그가 "Structure"일때
        {
            // 설치할 때 필요한 건물 타입이 아니라면
            if (other.GetComponent<Building>().type != needType)
                colliderList.Remove(other);    // 콜라이더 영역밖으로 나가면 리스트에서 제거
            else
                needTypeFlag = false;        // 설치 불가능
        }
        else                            // 나간 오브젝트의 태그가 "Structure"가 아닐때
        {
            // 충돌한 오브젝트의 레이어가 그라운드(땅)가 아닐때 && Ignore Raycast레이어가 아닐때
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
                colliderList.Remove(other);                // 콜라이더 영역밖으로 나가면 리스트에서 제거
        }
    }

    public bool IsBuildable()       // 건물을 지을수 있는지에 대한 여부를 외부에서 얻기 위한 bool형 함수
    {
        if(needType == Building.Type.Normal)        // 건물 타입이 Normal이라면
            return colliderList.Count == 0;             // 콜라이더 영역에 들어온 콜라이더가 없을때 true 반환
        else                         // 건물 타입이 Normal 아니라면
            return colliderList.Count == 0 && needTypeFlag;     // 콜라이더 영역에 들어온 콜라이더가 없을때 && 설치 가능할 때 true 반환
    }
}