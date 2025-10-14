using System;
using System.Collections.Generic;
using UnityEngine;

//
// 각 스테이지에 맞게 소켓을 준비하여 그 각 스테이지 인덱스에 맞게 소품을 설정함

// 특정 소켓에 어떤 프리팹을 둘지 정의하는 구조체
[Serializable]
public class SocketItem
{
    [Tooltip("배치할 소켓 인덱스 (holyway.sockets 배열의 인덱스 기준)")]
    public int socketIndex; // 몇 번째 소켓에 둘지
    [Tooltip("이 소켓에 놓을 프리팹 오브젝트")]
    public GameObject prefab; // 놓을 프리팹(예: 문, 의자, 소품 등)
}

// 하나의 스테이지에 대한 구성 정보(소켓별 아이템, 추가 소품 등)
[Serializable]
public class StagePreset
{
    [Tooltip("스테이지 번호 (1,2,3,4...)")]
    public int stageIndex;

    [Tooltip("이 스테이지에서 '어떤 소켓에 어떤 프리팹'을 놓을지의 목록")]
    public List<SocketItem> items = new List<SocketItem>();

    [Header("선택사항: 남는 소켓에 랜덤으로 추가할 소품들")]
    public GameObject[] extras;        // 남는 소켓에 랜덤 배치할 소품 후보 배열
    [Range(0, 10)] public int extraCount = 0;  // 추가 소품을 몇 개 넣을지
    public Vector3 extraJitter = new Vector3(0.12f, 0f, 0.12f); // 소품 랜덤 위치 오프셋 (겹침 방지)
}

// 실제로 소켓을 관리하고, 프리팹을 배치하는 메인 클래스
public class holyway : MonoBehaviour
{
    [Header("holyway 내부의 하위 방들 (holyway1~4 등)")]
    public GameObject[] roomGroups; // holyway1, holyway2, holyway3, holyway4 연결

    [Header("다음 스테이지로 이동 시 시작 위치(텔레포트 위치 등)")]
    public Transform startPoint;

    [Header("이 방(Stage) 내의 배치용 소켓들")]
    public Transform[] sockets; // 프리팹을 배치할 위치(Transform 배열)

    [Header("스테이지별 프리셋 목록 (1,2,3...)")]
    public List<StagePreset> presets = new List<StagePreset>();

    private int currentIndex = 0;

    // 매니저(상위 로직)가 새 스테이지를 스폰할 때 호출
    public void OnSpawned(int stageIndex)
    {
        // holyway1~4 활성화 관리
        currentIndex = stageIndex - 1;
        ActivateRoom(currentIndex);

        // 기존 PlaceFor 로직 유지
        PlaceFor(stageIndex);
    }

    // holyway 내부의 방 활성화 관리
    private void ActivateRoom(int index)
    {
        if (roomGroups == null || roomGroups.Length == 0) return;

        for (int i = 0; i < roomGroups.Length; i++)
        {
            if (roomGroups[i] != null)
                roomGroups[i].SetActive(i == index);
        }

        Debug.Log($"[holyway] holyway{index + 1} 활성화됨");
    }

    // 다음 방 활성화
    public void ActivateNextRoom()
    {
        currentIndex++;
        if (currentIndex >= roomGroups.Length)
        {
            Debug.Log("[holyway] 더 이상 다음 방이 없습니다.");
            return;
        }
        ActivateRoom(currentIndex);
    }

    // 스테이지가 파괴되기 전 호출 정리 싺
    public void OnBeforeDestroyed()
    {
        // 필요 시, 이 방(Stage)에서만 유지하던 상태를 정리
    }

    // 지정된 스테이지 번호(stageIndex)에 맞게 프리팹 배치
    void PlaceFor(int stageIndex)
    {
        // 소켓이 없으면 바로 종료
        if (sockets == null || sockets.Length == 0) return;

        // 기존 소켓 안의 모든 자식(이전 배치물) 제거
        ClearAllSockets();

        // stageIndex에 해당하는 프리셋(StagePreset) 검색
        var preset = presets.Find(p => p.stageIndex == stageIndex);
        if (preset == null)
        {
            Debug.LogWarning($"[Holyway] StagePreset {stageIndex} 를 찾지 못했어요.");
            return;
        }

        // 기본적으로 설정한 소켓 - 물품 1:1 소환
        foreach (var it in preset.items)
        {
            if (it == null || it.prefab == null) continue; // 비어있으면 스킵
            if (it.socketIndex < 0 || it.socketIndex >= sockets.Length) continue; // 인덱스 범위 체크

            var sock = sockets[it.socketIndex]; // 해당 인덱스의 소켓 가져오기
            if (!sock) continue;

            var go = Instantiate(it.prefab, sock.position, sock.rotation, sock);

            // Convex를 함으로써 물리 충돌이 안나게 하고 raycast가 인식 못하는걸 방지함
            foreach (var col in go.GetComponentsInChildren<MeshCollider>())
            {
                col.convex = true;
            }
        }

        // 남는 소켓이 있을 때 이것으로 소품을 추가하여 랜덤 배치함
        if (preset.extraCount > 0 && preset.extras != null && preset.extras.Length > 0)
        {
            // 자식(물품)이 없는 빈 소켓을 수집
            var freeSockets = new List<Transform>();
            for (int i = 0; i < sockets.Length; i++)
            {
                if (!sockets[i]) continue;
                if (sockets[i].childCount == 0) freeSockets.Add(sockets[i]);
            }

            // 배치 개수는 남은 소켓 수와 extraCount 중 작은 값으로 제한
            int add = Mathf.Min(preset.extraCount, freeSockets.Count);
            for (int n = 0; n < add; n++)
            {
                var sock = freeSockets[n];
                var prefab = preset.extras[UnityEngine.Random.Range(0, preset.extras.Length)];
                if (!prefab) continue;

                // 소품 프리팹 인스턴스화
                var go = Instantiate(prefab, sock.position, sock.rotation, sock);

                // MeshCollider Convex 자동 설정
                foreach (var col in go.GetComponentsInChildren<MeshCollider>())
                {
                    col.convex = true;
                }

                // 무작위 배치 겹치지 않도록 배정
                go.transform.localPosition += new Vector3(
                    UnityEngine.Random.Range(-preset.extraJitter.x, preset.extraJitter.x),
                    UnityEngine.Random.Range(-preset.extraJitter.y, preset.extraJitter.y),
                    UnityEngine.Random.Range(-preset.extraJitter.z, preset.extraJitter.z)
                );

                // 회전은 초기화
                go.transform.localRotation = Quaternion.identity;
            }
        }
    }

    // 기존 소켓의 물품들을 리셋시키고 다시 배치
    void ClearAllSockets()
    {
        foreach (var s in sockets)
        {
            if (!s) continue;
            // 자식 오브젝트들을 역순으로 하나씩 삭제
            for (int i = s.childCount - 1; i >= 0; i--)
            {
                var child = s.GetChild(i);
#if UNITY_EDITOR
                if (!Application.isPlaying) DestroyImmediate(child.gameObject);
                else Destroy(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
            }
        }
    }
}

