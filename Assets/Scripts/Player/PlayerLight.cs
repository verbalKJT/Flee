using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public Light lanternLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (lanternLight != null)
        {
            lanternLight.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(lanternLight != null)
            {
                lanternLight.enabled = !lanternLight.enabled;
            }
        }
    }
}
