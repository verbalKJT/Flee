#define PC
//#define Oculus
//#define Vive

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if Vive
using Valve.VR;
using UnityEngine.XR;
#endif

public static class ARAVRInput
{
#if PC
    public enum ButtonTarget
    {
        Fire1,
        Fire2,
        Fire3,
        Jump,
    }
#elif Vive
    public enum ButtonTarget
    {
        Teleport,
        InteractUI,
        GrabGrip,
        Jump,
    }
#endif

    public enum Button
    {
#if PC
        One = ButtonTarget.Fire1,
        Two = ButtonTarget.Jump,
        Thumbstick = ButtonTarget.Fire1,
        IndexTrigger = ButtonTarget.Fire3,
        HandTrigger = ButtonTarget.Fire2
#elif Oculus
        One = OVRInput.Button.One,
        Two = OVRInput.Button.Two,
        Thumbstick = OVRInput.Button.PrimaryThumbstick,
        IndexTrigger = OVRInput.Button.PrimaryIndexTrigger,
        HandTrigger = OVRInput.Button.PrimaryHandTrigger
#elif Vive
        One = ButtonTarget.InteractUI,
        Two = ButtonTarget.Jump,
        Thumbstick = ButtonTarget.Teleport,
        IndexTrigger = ButtonTarget.InteractUI,
        HandTrigger = ButtonTarget.GrabGrip,
#endif
    }

    public enum Controller
    {
#if PC
        LTouch,
        RTouch
#elif Oculus
        LTouch = OVRInput.Controller.LTouch,
        RTouch = OVRInput.Controller.RTouch
#elif Vive
        LTouch = SteamVR_Input_Sources.LeftHand,
        RTouch = SteamVR_Input_Sources.RightHand,
#endif
    }

    // 왼손 컨트롤러
    static Transform lHand;
    // 왼손 컨트롤러의 Transform을 찾아서 반환
    public static Transform LHand
    {
        get
        {
            if (lHand == null)
            {
#if PC
                // LHand라는 이름으로 새로운 게임 오브젝트 생성
                GameObject handObj = new GameObject("LHand");
                // 생성한 오브젝트의 Transform을 lHand에 할당
                lHand = handObj.transform;
                // 카메라의 자식으로 설정하여 위치를 따라가게 함
                lHand.parent = Camera.main.transform;
#elif Oculus
                // Oculus에서는 미리 존재하는 LeftControllerAnchor를 찾음
                lHand = GameObject.Find("LeftControllerAnchor").transform;
#elif Vive
                // Vive에서는 Controller(left) 오브젝트를 찾음
                lHand = GameObject.Find("Controller(left)").transform;
#endif
            }
            return lHand;
        }
    }
    // 오른손 컨트롤러
    static Transform rHand;
    // 오른손 컨트롤러의 Transform을 찾아서 반환
    public static Transform RHand
    {
        get
        {
            // rHand가 아직 할당되지 않았다면
            if (rHand == null)
            {
#if PC
                // RHand라는 이름으로 새로운 게임 오브젝트 생성
                GameObject handObj = new GameObject("RHand");
                // 생성한 오브젝트의 Transform을 rHand에 할당
                rHand = handObj.transform;
                // 카메라의 자식으로 설정하여 위치를 따라가게 함
                rHand.parent = Camera.main.transform;
#elif Oculus
                // Oculus에서는 RightControllerAnchor를 찾음
                rHand = GameObject.Find("RightControllerAnchor").transform;
#elif Vive
                // Vive에서는 Controller(right) 오브젝트를 찾음
                rHand = GameObject.Find("Controller(right)").transform;
#endif
            }
            return rHand;
        }
    }

    // 오른손 컨트롤러의 위치를 반환
    public static Vector3 RHandPosition
    {
        get
        {
#if PC
            // 마우스의 화면상 좌표를 가져옴
            Vector3 pos = Input.mousePosition;
            // z 값(깊이)을 0.7m로 설정 (카메라에서 0.7m 앞)
            pos.z = 0.7f;
            // 화면 좌표를 월드 좌표로 변환
            pos = Camera.main.ScreenToWorldPoint(pos);

            // 변환된 위치를 RHand 오브젝트에 적용
            RHand.position = pos;
            return pos;
#elif Oculus
            // Oculus 오른손 컨트롤러의 로컬 위치를 가져옴
            Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            // 로컬 위치를 월드 좌표로 변환
            pos = GetTransform().TransformPoint(pos);
            return pos;
#elif Vive
            // Vive에서는 RHand 오브젝트의 현재 위치를 그대로 반환
            Vector3 pos = RHand.position;
            return pos;
#endif
        }
    }

    // 오른손 컨트롤러가 가리키는 방향을 반환
    public static Vector3 RHandDirection
    {
        get
        {
#if PC
            // 카메라 위치에서 오른손 위치까지의 방향 벡터 계산
            Vector3 direction = RHandPosition - Camera.main.transform.position;

            // RHand의 forward 방향을 이 방향으로 설정
            RHand.forward = direction;
            return direction;
#elif Oculus
            // Oculus 오른손 컨트롤러의 로컬 회전을 가져와 전방 벡터에 곱함
            Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;
            // 로컬 방향을 월드 방향으로 변환
            direction = GetTransform().TransformDirection(direction);

            return direction;
#elif Vive
            // Vive에서는 RHand 오브젝트의 forward 방향을 반환
            Vector3 direction = RHand.forward;
            return direction;
#endif
        }
    }

    // 왼손 컨트롤러의 위치를 반환
    public static Vector3 LHandPosition
    {
        get
        {
#if PC
            // PC에서는 마우스 하나만 사용하므로 오른손 위치를 그대로 반환
            return RHandPosition;
#elif Oculus
        // Oculus 왼손 컨트롤러의 로컬 위치를 가져옴
        Vector3 pos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        // 로컬 위치를 월드 좌표로 변환
        pos = GetTransform().TransformPoint(pos);
        return pos;
#elif Vive
        // Vive에서는 LHand 오브젝트의 현재 위치를 반환
        Vector3 pos = LHand.position;
        return pos;
#endif
    }
}

// 왼손 컨트롤러가 가리키는 방향을 반환
public static Vector3 LHandDirection
{
    get
    {
#if PC
        // PC에서는 마우스 하나만 사용하므로 오른손 방향을 그대로 반환
        return RHandDirection;
#elif Oculus
        // Oculus 왼손 컨트롤러의 로컬 회전을 가져와 전방 벡터에 곱함
        Vector3 direction = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Vector3.forward;
        // 로컬 방향을 월드 방향으로 변환
        direction = GetTransform().TransformDirection(direction);

        return direction;
#elif Vive
        // Vive에서는 LHand 오브젝트의 forward 방향을 반환
        Vector3 direction = LHand.forward;
        return direction;
#endif
    }
}


#if Oculus || Vive
    static Transform rootTransform;
#endif

#if Oculus
    static Transform GetTransform()
    {
        if (rootTransform == null)
        {
            rootTransform = GameObject.Find("TrackingSpace").transform;
        }
        return rootTransform;
    }
#elif Vive
    static Transform GetTransform()
    {
        if (rootTransform == null)
        {

            rootTransform = GameObject.Find("[CameraRig]").transform;
        }
        return rootTransform;
    }
#endif

    // 컨트롤러의 특정 버튼을 누르고 있는 동안 true 반환
    public static bool Get(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        // PC에서는 ButtonTarget으로 변환 후 문자열로 이름을 얻어 Input 처리 (virtualMask)
        return Input.GetButton(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.Get((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#elif Vive
        string button = ((ButtonTarget)virtualMask).ToString();
        return SteamVR_Input.GetState(button, (SteamVR_Input_Sources)(hand));
#endif
    }

    // 컨트롤러의 특정 버튼을 눌렀을 때 한 번 true 반환
    public static bool GetDown(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonDown(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetDown((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#elif Vive
        string button = ((ButtonTarget)virtualMask).ToString();
        return SteamVR_Input.GetStateDown(button, (SteamVR_Input_Sources)(hand));
#endif
    }

    // 컨트롤러의 특정 버튼을 눌렀다 뗐을 때 true 반환
    public static bool GetUp(Button virtualMask, Controller hand = Controller.RTouch)
    {
#if PC
        return Input.GetButtonUp(((ButtonTarget)virtualMask).ToString());
#elif Oculus
        return OVRInput.GetUp((OVRInput.Button)virtualMask, (OVRInput.Controller)hand);
#elif Vive
        string button = ((ButtonTarget)virtualMask).ToString();
        return SteamVR_Input.GetStateUp(button, (SteamVR_Input_Sources)(hand));
#endif
    }

    // 컨트롤러의 Axis 입력값을 반환
    // axis: "Horizontal", "Vertical" 중 하나
    public static float GetAxis(string axis, Controller hand = Controller.LTouch)
    {
#if PC
        return Input.GetAxis(axis);
#elif Oculus
        if (axis == "Horizontal")
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)hand).x;
        }
        else
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)hand).y;
        }
#elif Vive
        if (axis == "Horizontal")
        {
            return SteamVR_Input.GetVector2("TouchPad", (SteamVR_Input_Sources)(hand)).x;
        }
else
        {
            return SteamVR_Input.GetVector2("TouchPad", (SteamVR_Input_Sources)(hand)).y;
        }
#endif
    }


    // 컨트롤러의 진동(햅틱)을 실행
    public static void PlayVibration(Controller hand)
    {
#if Oculus
        PlayVibration(0.06f, 1, 1, hand);
#elif Vive
        PlayVibration(0.06f, 160, 0.5f, hand);
#endif
    }
    // 진동 호출하기
    // duration : 반복 횟수, frequency : 지속 시간, amplify : 진동 크기, hand : 왼쪽 또는 오른쪽 컨트롤러
    public static void PlayVibration(float duration, float frequency, float amplitude, Controller hand)
    {
#if Oculus
        if (CoroutineInstance.coroutineInstance == null)
        {
            GameObject coroutineObj = new GameObject("CoroutineInstance");
            coroutineObj.AddComponent<CoroutineInstance>();
        }

        // 기존 코루틴이 있다면 중지하고, 새로 시작
        // �̹� �÷������� ���� �ڷ�ƾ�� ����
        CoroutineInstance.coroutineInstance.StopAllCoroutines();
        CoroutineInstance.coroutineInstance.StartCoroutine(VibrationCoroutine(duration, frequency, amplitude, hand));
#elif Vive
        SteamVR_Actions._default.Haptic.Execute(0, duration, frequency, amplitude, (SteamVR_Input_Sources)hand);
#endif
    }


    // 카메라 위치를 초기화 (VR 중심을 재설정)
    public static void Recenter()
    {
#if Oculus
        OVRManager.display.RecenterPose();
#elif Vive
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);
        for (int i = 0; i < subsystems.Count; i++)
        {
            subsystems[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.
            TrackingReference);
            subsystems[i].TryRecenter();
        }
#endif
    }

    // 주어진 방향으로 오브젝트의 전방을 맞춤
    public static void Recenter(Transform target, Vector3 direction)
    {
        target.forward = target.rotation * direction;
    }


#if PC
    static Vector3 originScale = Vector3.one * 0.02f;
#else
    static Vector3 originScale = Vector3.one * 0.005f;
#endif

    // 조준점(crosshair)을 화면에 표시하고 위치 조정
    public static void DrawCrosshair(Transform crosshair, bool isHand = true, Controller hand = Controller.RTouch)
    {

        Ray ray;

        // 손 위치 기준으로 조준점 계산
        if (isHand)
        {
#if PC
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
            if (hand == Controller.RTouch)
            {
                ray = new Ray(RHandPosition, RHandDirection);
            }
            else
            {
                ray = new Ray(LHandPosition, LHandDirection);
            }
#endif
        }
        else
        {
            // 카메라 전방 기준으로 광선 생성
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        // 바닥 평면과의 충돌 지점 계산
        // 눈에 안보이는 Plane 을 만듦.
        Plane plane = new Plane(Vector3.up, 0);
        float distance = 0;
        // plane을 이용해 ray를 쏨
        if (plane.Raycast(ray, out distance))
        {
            // 레이의 GetPoint 함수를 이용해 충돌 지점의 위치를 가져옴.
            crosshair.position = ray.GetPoint(distance);
            crosshair.forward = -Camera.main.transform.forward;
            // 거리에 따라 크로스헤어 크기 조절
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
        else
        {
            // 평면과 교차하지 않으면 먼 거리의 위치로 설정
            crosshair.position = ray.origin + ray.direction * 100;
            crosshair.forward = -Camera.main.transform.forward;
            distance = (crosshair.position - ray.origin).magnitude;
            crosshair.localScale = originScale * Mathf.Max(1, distance);
        }
    }


#if Oculus
    // Oculus에서 진동을 위한 코루틴
    static IEnumerator VibrationCoroutine(float duration, float frequency, float amplitude, Controller hand)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {

            currentTime += Time.deltaTime;

            OVRInput.SetControllerVibration(frequency, amplitude, (OVRInput.Controller)
            hand);
            yield return null;
        }
        OVRInput.SetControllerVibration(0, 0, (OVRInput.Controller)hand);
    }
#endif
}

// ARAVRInput 클래스에서 사용하는 코루틴 실행 객체
class CoroutineInstance : MonoBehaviour
{
    public static CoroutineInstance coroutineInstance = null;
    private void Awake()
    {
        if (coroutineInstance == null)
        {
            coroutineInstance = this;
        }
        
        // 씬이 전환되어도 오브젝트가 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }
}

