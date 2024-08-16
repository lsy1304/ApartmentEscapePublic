using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlinkLight : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private int blinkCount;

    private Light2D _light;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    void Start()
    {
        StartCoroutine(FlashBlink(waitTime, blinkCount));
    }

    IEnumerator FlashBlink(float waitTime, int blinkCount)
    {
        yield return new WaitForSeconds(waitTime);
        int count = 1;

        while (count <= blinkCount)
        {
            if (_light.enabled)
            {
                yield return new WaitForSeconds(0.15f);
                _light.enabled = false;
            }
            else if (!_light.enabled)
            {
                yield return new WaitForSeconds(0.15f);
                _light.enabled = true;
                count++;
            }
        }
        yield return new WaitForSeconds(0.5f);
        _light.gameObject.SetActive(false);
    }
}
