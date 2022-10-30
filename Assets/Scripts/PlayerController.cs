using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조절 변수
    [SerializeField]
    private float walkSpeed;            // 걷는 속도 변수
    [SerializeField]
    private float runSpeed;             // 달리기 속도 변수
    [SerializeField]
    private float crouchSpeed;          // 앉아서 움직이는 속도 변수
    private float applySpeed;           // 실제 속도에 적용하는 변수

    // 점프 변수
    [SerializeField]
    private float jumpForce;            // 점프 높이

    // 상태 변수
    private bool isRun = false;         // 달리기 유무
    private bool isCrouch = false;      // 앉기 유무
    private bool isGround = true;       // 이중점프 방지

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;


    // 카메라 회전 민감도
    [SerializeField]
    private float lookSensitivity;               // 민감도

    // 카메라 설정 (현재, 한계)
    [SerializeField]
    private float cameraRotationLimit;          // 카메라 한계 제어
    private float currentCameraRotationX = 0f;  // 현재 카메라 X축 회전변수

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;                   // 메인카메라

    private Rigidbody myRigid;                  // 플레이어 물리

    private CapsuleCollider capsuleCollider;    // 땅 착지 여부 확인

    //[SerializeField]                              // [SerializField] 대신 Start()에서 FIndObjectOfType으로 찾아주는 방법도 있음
    private GunController theGunController;        // GunController 스크립트에서 정보를 가져오기 위해서 사용


    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        theGunController = FindObjectOfType<GunController>();

        // 초기화
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void TryCrouch() // 앉기 시도
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch() // 앉기 동작
    {
        /* 아래와 같은뜻
         if (isCrouch)
            isCrouch = false;
          else
            isCrouch = true;
         */
        isCrouch = !isCrouch;  // bool 값을 함수가 불릴때마다 스위치 느낌으로 True, False 값을 스위칭하도록 함

        if(isCrouch) // 앉을시
        {
            applySpeed = crouchSpeed;       // 앉아서 움직이는 속도 대입
            applyCrouchPosY = crouchPosY;   // 앉았을 때 시점 대입
        }
        else
        {
            applySpeed = walkSpeed;         // 앉았다가 일어섰을 때 움직이는 속도 대입
            applyCrouchPosY = originPosY;   // 앉았다가 일어섰을 때 시점 대입
        }
        // 캐릭터가 앉았을 때 카메라의 시점 변경
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine() // 캐릭터가 앉을때 부드러운 카메라 시점변경 코루틴 함수
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            // Lerp 는 선형보간을 뜻함, 카메라 시점값의 변경을 부드럽게 바꿔준다.
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        // 보간은 딱 수가 떨어지지 않기에 count로 while문을 빠져나와서 값을 다시 설정해준다.
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }
    private void IsGround() // 바닥에 닿아있는지 확인하는 함수
    {
        // 광선을 바닥으로 쏘아서 캐릭터가 바닥에 닿았는지 확인
        // bounds는 콜라이더 바닥면을 뜻하고 extents는 콜라이더의 절반의 길이를 뜻함
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
    private void TryJump() // 점프 입력 & 점프 조건함수
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void Jump() // 점프
    {
        // 앉은상태에서 점프시 앉은 상태 해제
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun() // 달리기 입력 & 달리기 조건검사 함수
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running() // 달리기 함수
    {
        // 앉은상태에서 달릴시 앉은 상태 해제
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();
        
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel() // 달리기 취소 함수
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()  // 캐릭터 움직임 구현 함수
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // 캐릭터 좌우 이동 키 입력
        float _moveDirZ = Input.GetAxisRaw("Vertical");   // 캐릭터 앞뒤 이동 키 입력

        Vector3 _moveHorizontal = transform.right * _moveDirX; // 키 값에 따른 캐릭터 좌우 이동 구현
        Vector3 _moveVertical = transform.forward * _moveDirZ; // 키 값에 따른 캐릭터 앞뒤 이동 구현

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; // 방향과 속도 구현

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); // 실제 캐릭터가 이동을 하게 해준다.
    }

    private void CameraRotation() // 카메라 상하 회전 함수
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");             // 마우스 상하 움직에 따라서 X축 회전 값 입력
        float _cameraRotationX = _xRotation * lookSensitivity;      // 카메라X축에 회전값 설정

        currentCameraRotationX -= _cameraRotationX;                 // 현재 카메라 X축 회전 값  ps. X축회전은 아래를 향하면 값이 + 위를 향하면 값이 -가 된다
                                                                    // 하지만 마우스의 값은 이것의 반대이므로 빼줘야한다.
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);// 현재 카메라 X축 회전 값에 cameraRortationLimit 값만큼 제한한다.

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); // 메인카메라 회전정보에 X축 회전 값만큼 회전시킨다.
    }

    private void CharacterRotation() // 캐릭터 좌우 회전 함수
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");             // 마우스 좌우 움직에 따라서 Y축 회전 값 입력
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;  // 캐릭처 Y축에 회전값 설정
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));   // 캐릭터의 Y 축 회전값만큼 회전시킨다.
    }
}
