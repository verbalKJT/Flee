// IGrabbable.cs
using UnityEngine;

public interface IGrabbable
{
    void Grab(Transform hand);   // 잡았을 때 호출
    void Release();              // 놓았을 때 호출
}