using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerVision : MonoBehaviour
{
    public Image blackScreen;

    public IEnumerator BlindForSeconds(float seconds) // 붙집히고 난뒤 암전 설정
    {
        blackScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        blackScreen.gameObject.SetActive(false);
    }
}
