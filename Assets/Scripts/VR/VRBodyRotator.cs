using UnityEngine;

public class VRBodyRotator : MonoBehaviour
{
    [Header("연결 대상")]
    public Transform headCamera; // OVRCameraRig의 CenterEyeAnchor (또는 Main Camera)
    public Transform bodyMesh;   // 회전시킬 몸통 (SK_Changeling_Boy)

    [Header("설정")]
    public bool smoothTurn = true; // 부드럽게 따라갈지 여부
    public float turnSpeed = 5f;   // 회전 속도 (높을수록 빠름)
    
    // 몸이 머리와 30도 이상 차이날 때만 돌리고 싶다면 이 값을 올리세요 (0이면 즉시 회전)
    public float thresholdAngle = 0f; 

    void Update()
    {
        if (headCamera == null || bodyMesh == null) return;

        // 1. 카메라의 보는 방향(Forward)을 가져옵니다.
        Vector3 lookDirection = headCamera.forward;

        // 2. 중요: VR에서 고개를 들거나 숙일 때(Y축 변화) 몸이 기울어지면 안 되므로 Y값을 0으로 만듭니다.
        lookDirection.y = 0;
        lookDirection.Normalize(); // 벡터 길이 정규화

        // 3. 회전값 생성 (바라보는 방향으로의 회전)
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        // 4. 각도 차이 계산 (옵션: 일정 각도 이상일 때만 돌리기 위해)
        float angleDifference = Vector3.Angle(bodyMesh.forward, lookDirection);

        if (angleDifference > thresholdAngle)
        {
            if (smoothTurn)
            {
                // 부드럽게 회전 (Slerp)
                bodyMesh.rotation = Quaternion.Slerp(bodyMesh.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }
            else
            {
                // 즉시 회전
                bodyMesh.rotation = targetRotation;
            }
        }
    }
}