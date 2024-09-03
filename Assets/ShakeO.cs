using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShakeO : MonoBehaviour
{
    public GameObject Canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        t = 0;   
    }
    float t = 0;
    // Update is called once per frame
    void Update()
    {
        if (t >= 1.0f)
        {
            t = 0;
            if (Canvas.activeSelf && Random.value > 0.6f)
            {
                ((RectTransform)this.transform).DOShakePosition(1.0f, 6, 10, 1, true, true);
            }
        }
        t += Time.deltaTime;

    }
}
