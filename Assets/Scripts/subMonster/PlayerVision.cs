using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerVision : MonoBehaviour
{
    public Image blackScreen;

    public IEnumerator BlindForSeconds(float seconds) // �������� ���� ���� ����
    {
        blackScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        blackScreen.gameObject.SetActive(false);
    }
}
