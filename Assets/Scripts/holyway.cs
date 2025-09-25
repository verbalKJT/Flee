using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SocketItem
{
    [Tooltip("배치할 소켓 인덱스 (Holyway.sockets 배열 기준)")]
    public int socketIndex;
    [Tooltip("놓을 가구 프리팹")]
    public GameObject prefab;
}

[Serializable]
public class StagePreset
{
    [Tooltip("스테이지 번호 (1,2,3,4...)")]
    public int stageIndex;
    [Tooltip("이 스테이지에서 '어느 소켓에 어떤 가구'를 놓을지 목록")]
    public List<SocketItem> items = new List<SocketItem>();

    [Header("선택: 남는 소켓에 랜덤 추가할 소품 풀")]
    public GameObject[] extras;        // 선택 기능: 남는 소켓 채우기용
    [Range(0, 10)] public int extraCount = 0;
    public Vector3 extraJitter = new Vector3(0.12f, 0f, 0.12f);
}

public class holyway : MonoBehaviour
{
    [Header("다음 시작 지점(텔포 위치)")]
    public Transform startPoint;

    [Header("배치용 소켓들 (씬/프리팹에서 순서 정렬)")]
    public Transform[] sockets;

    [Header("스테이지별 프리셋 (1~4 등)")]
    public List<StagePreset> presets = new List<StagePreset>();

    // 매니저가 새 방을 생성했을 때 호출
    public void OnSpawned(int stageIndex)
    {
        PlaceFor(stageIndex);
    }

    public void OnBeforeDestroyed()
    {
        // 필요 시 이 방에서만 유지하던 상태 정리
    }

    // --- 내부 로직 ---

    void PlaceFor(int stageIndex)
    {
        if (sockets == null || sockets.Length == 0) return;

        ClearAllSockets();

        // 1) 스테이지 프리셋 찾기
        var preset = presets.Find(p => p.stageIndex == stageIndex);
        if (preset == null)
        {
            Debug.LogWarning($"[Holyway] StagePreset {stageIndex} 를 찾지 못했어요.");
            return;
        }

        // 2) 명시 매핑대로 배치
        foreach (var it in preset.items)
        {
            if (it == null || it.prefab == null) continue;
            if (it.socketIndex < 0 || it.socketIndex >= sockets.Length) continue;

            var sock = sockets[it.socketIndex];
            if (!sock) continue;

            Instantiate(it.prefab, sock.position, sock.rotation, sock);
        }

        // 3) (옵션) 남는 소켓에 랜덤 소품 추가
        if (preset.extraCount > 0 && preset.extras != null && preset.extras.Length > 0)
        {
            // 남은 소켓 목록 수집
            var freeSockets = new List<Transform>();
            for (int i = 0; i < sockets.Length; i++)
            {
                if (!sockets[i]) continue;
                if (sockets[i].childCount == 0) freeSockets.Add(sockets[i]);
            }

            int add = Mathf.Min(preset.extraCount, freeSockets.Count);
            for (int n = 0; n < add; n++)
            {
                var sock = freeSockets[n];
                var prefab = preset.extras[UnityEngine.Random.Range(0, preset.extras.Length)];
                if (!prefab) continue;

                var go = Instantiate(prefab, sock);
                // 살짝 흩뿌리기(겹침 줄이기)
                go.transform.localPosition += new Vector3(
                    UnityEngine.Random.Range(-preset.extraJitter.x, preset.extraJitter.x),
                    UnityEngine.Random.Range(-preset.extraJitter.y, preset.extraJitter.y),
                    UnityEngine.Random.Range(-preset.extraJitter.z, preset.extraJitter.z)
                );
                go.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void ClearAllSockets()
    {
        foreach (var s in sockets)
        {
            if (!s) continue;
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
