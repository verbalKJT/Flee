using Meta.WitAi.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class RightHandGrabber : MonoBehaviour
{
    bool isGrabbing = false; // 물체를 잡고 있는지 여부
    GameObject grabbedObject; // 잡고 있는 물체
    public LayerMask grabbedLayer; // 잡은 물체의 종류
    public float grabRange = 0.2f; // 잡을 수 있는 거리
    
    // 손 프리팹을 넣을 변수 추가
    public Transform handMeshTransform;
    // 손 애니메이터를 넣을 변수
    public Animator handAnimator;

    Vector3 prevPos; // 이전 위치
    Quaternion prevRot; // 이전 회전
    public float throwPower = 10; // 던질 힘

    public bool isRemoteGrab = true; // 원거리에서 물체를 잡는 기능 활성화 여부
    public float remoteGrabDistance = 20; // 원거리에서 물체를 잡을 수 있는 거리

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing == false) // 물체를 잡지 않고 있는 경우
        {
            TryGrab();
        }
        else
        {
            TryUnGrab(); // 물체 놓기
        }
    }
    void TryUnGrab()
    {
        // 던질 방향
        Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos) / Time.deltaTime;
        // 위치 기억
        prevPos = ARAVRInput.RHandPosition;
        
        // 쿼터니온 공식
        // angle1 = Q1, angle2 = Q2
        // angle1 + angle2 = Q1 * Q2
        // -angle2 = Quaternion.Inverse(Q2)
        // angle2 - angle1 = Quaternion.FromToRotation(Q1, Q2) = Q2 * Quaternion.Inverse(Q1)
        
        // 회전 방향 = current - previous의 차로 구함. -previous 는 Inverse 로 구함.
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        // 이전 회전 저장
        prevRot = ARAVRInput.RHand.rotation;
        
        // Grab 버튼을 놓았다면
        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger,
           ARAVRInput.Controller.RTouch))
        {
            isGrabbing = false; // 잡지 않은 상태로 전환
            // 물리 기능 활성화
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            // 손에서 폭탄 떼어내기
            grabbedObject.transform.parent = null;

            // 던지기
            //grabbedObject.GetComponent<Rigidbody>().linearVelocity = ARAVRInput.RHandDirection * throwPower; // 그냥 던지기
            grabbedObject.GetComponent<Rigidbody>().linearVelocity = throwDirection * throwPower; // 내가 직접 던지기
            
            // 각 속도 = (1/dt) * d0 (특정 축 기준 변위 각도)
            float angle; // 회전 각도
            Vector3 axis; // 회전 축
            deltaRotation.ToAngleAxis(out angle, out axis); // 몇 도를 어떤 축을 기준으로 돌렸나 ? 
            // 초당 회전 속도로 물체가 회전하면서 날아가도록
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis; 
            grabbedObject.GetComponent<Rigidbody>().angularVelocity  = angularVelocity;
            
            // 잡은 물체가 없도록 설정
            grabbedObject = null;
            
            // [추가 3] 놓았을 때 애니메이션 해제
            if (handAnimator != null)
            {
                handAnimator.SetBool("IsGrab", false);
            }
        }
    }
    void TryGrab()
    {
        // 1. Grab 버튼을 눌렀다면
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger,
           ARAVRInput.Controller.RTouch))
        {
            Debug.Log("RightHand Trigger Pressed"); // 이게 안찍히면 입력 문제
            // 원거리 물체 잡기를 사용한다면
            if (isRemoteGrab) 
            {
                // 손 방향으로 Ray 발사
                Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
                RaycastHit hitInfo;
                // SphereCast 를 이용해 물체 충돌 체크
                if (Physics.SphereCast(ray, 0.5f, out hitInfo, remoteGrabDistance,
                    grabbedLayer))
                {
                    isGrabbing = true; // 잡은 상태로 전환
                    grabbedObject = hitInfo.transform.gameObject; // 잡은 물체에 대한 기억
                    StartCoroutine(GrabbingAnimator()); // 물체가 끌려오는 기능 실행
                    
                    // [추가 2] 잡았을 때 애니메이션 실행 (파라미터 이름이 "IsGrab"이라고 가정)
                    if (handAnimator != null)
                    {
                        handAnimator.SetBool("IsGrab", true);
                    }
                }
                return;
            }


            // 2. 영역 안에 있는 모든 폭탄 검출
            Collider[] hitObjects = Physics.OverlapSphere
                (ARAVRInput.RHandPosition, grabRange, grabbedLayer);
            // 가장 가까운 폭탄 인덱스
            int closest = 0; 
            // 손과 가장 가까운 물체 선택
            for (int i = 1; i < hitObjects.Length; i++)
            {
                // 손과 가장 가까운 물체와의 거리
                Vector3 closestPos = hitObjects[closest].transform.position;
                float closestDistance = Vector3.Distance(closestPos,
                    ARAVRInput.RHandPosition);
                // 다음 물체와 손의 거리
                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos,
                    ARAVRInput.RHandPosition);
                // 다음 물체와의 거리가 더 가깝다면 
                if (nextDistance < closestDistance)
                {
                    closest = i; // 가장 가까운 물체 인덱스 교체
                }
            }
            // 3. 폭탄을 잡는다 -> 검출된 물체가 있을 경우
            if (hitObjects.Length > 0) 
            {
                isGrabbing = true; // 잡은 상태로 전환
                // 잡은 물체에 대한 기억
                grabbedObject = hitObjects[closest].gameObject;
                // 잡은 물체를 손의 자식으로 등록
                grabbedObject.transform.parent = handMeshTransform;
                // 물리 ㄱ ㅣ능 해제
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                // 초기 위치 값 지정
                prevPos = ARAVRInput.RHandPosition; 
                // 초기 회전 값 지정
                prevRot = ARAVRInput.RHand.rotation;
                
                // 근거리에서도 애니메이션 실행
                if (handAnimator != null)
                {
                    handAnimator.SetBool("IsGrab", true);
                }
            }
        }
        IEnumerator GrabbingAnimator()
        {
            // 물리 기능 정지
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            prevPos = ARAVRInput.RHandPosition ; // 초기 위치 값 지정
            prevRot = ARAVRInput.RHand.rotation; // 초기 회전 값 지정
            Vector3 startLocation = grabbedObject.transform.position;
            Vector3 targetLocation = ARAVRInput.RHandPosition +
                ARAVRInput.RHandDirection * 0.1f;

            float currentTime = 0; //�ð��� ��� ���� ����
            float finishTime = 0.2f; //0.2 �ʵ��� ��ü�� ����´�
            // 경과율
            float elapsedRate = currentTime / finishTime; 
            while (elapsedRate < 1)
            {
                currentTime += Time.deltaTime; //�ð��� ���
                elapsedRate = currentTime / finishTime; //����ð�/0.2�� = �����
                grabbedObject.transform.position = Vector3.Lerp(startLocation,
                    targetLocation, elapsedRate);
                yield return null;
            }
            // 잡은 물체를 손의 자식으로 등록
            // grabbedObject.transform.position = targetLocation;
            grabbedObject.transform.parent = handMeshTransform;
            grabbedObject.transform.localPosition = Vector3.zero;        // GrabHold의 정중앙 위치로 이동
            grabbedObject.transform.localRotation = Quaternion.identity; // GrabHold의 회전값과 일치시킴
        }
    }
}
