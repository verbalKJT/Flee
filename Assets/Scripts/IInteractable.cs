using UnityEngine;

public interface IInteractable//크로스헤어 상호작용을 위한 인터페이스
{
    void Interact();
    string GetPromptText(); // 상호작용 문구 반환용

}
