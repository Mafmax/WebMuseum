using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHindrance : MonoBehaviour
{
    public AnimationCurve Curve;
    private Light light;
    private float time = 0;
    private bool blinked;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(BlinkCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator BlinkCoroutine()
    {

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            TryRandomBlink();
        }
    }
    private void TryRandomBlink()
    {
        
        if (Random.Range(1, 100) == 1)
        {
            if (!blinked)
            {
                blinked = true;
                time = 0;
                StartCoroutine(Blink());
            }
        }
    }
    private IEnumerator Blink()
    {
        while (true)
        {
            yield return null;
            time += Time.deltaTime;
            light.intensity = Curve.Evaluate(time);
            if (time >= Curve[Curve.length - 1].time)
            {
                light.intensity = 1;
                blinked = false;
                yield break;
            }
        }
    }
}
