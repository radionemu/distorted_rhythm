using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EndManager : MonoBehaviour
{
    public GameObject EndCanvas;

    private static EndManager instance;
    public static EndManager GetInstance() => instance;

    public GameObject Triangle;
    public Light2D light2D;
    public Color color;

    
    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Load() {
        yield return new WaitForSeconds(5f);
        TitleManager.GetInstance().TitleCanvas.SetActive(true);
        Animator animator = TitleManager.GetInstance().TitleCanvas.GetComponent<Animator>();
        yield return new WaitUntil(()=>animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f);
        EndCanvas.SetActive(false);
        TitleManager.GetInstance().init();
    }
}
