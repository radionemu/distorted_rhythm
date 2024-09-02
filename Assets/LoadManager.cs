using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public GameObject LoadCanvas;
    public Animator LoadingAnimator;
    
    public RectTransform EffectGameObject;

    private static LoadManager instance;
    public static LoadManager GetInstance() => instance;

    float t = 0;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(t >= 1.0f){
            t = 0;
            if (LoadCanvas.activeSelf && Random.value > 0.6f) {
                EffectGameObject.DOShakePosition(1.0f, 6, 10, 1, true, true);
            }
        }
        t += Time.deltaTime;
    }

    public IEnumerator Load(System.Action callback) {
        LoadCanvas.SetActive(true);
        yield return new WaitUntil(() => LoadingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Load"));
        yield return new WaitForSeconds(3f);
        callback?.Invoke();
        LoadingAnimator.SetTrigger("Complete");
        yield return new WaitUntil(() => LoadingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Complete"));
        yield return new WaitWhile(() => LoadingAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        LoadCanvas.SetActive(false);
    }
}
