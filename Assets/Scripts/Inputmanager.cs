using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputmanager : MonoBehaviour
{
    Dictionary<int,Circle> circleDic;

    private bool isactive;

    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i<transform.childCount; i++){
            var circleT = transform.GetChild(i);
            var circleC = circleT.GetComponent<Circle>();
            circleC.SetID(i);
            circleDic.Add(i,circleC);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPointerDown(Circle c){
        Debug.Log("pdown"+c.getID());
    }

    public void onPointerUp(Circle c){
        Debug.Log("pup"+c.getID());
    }

    public void onPointerEnter(Circle c){
        Debug.Log("penter"+c.getID());
    }

    public void onPointerExit(Circle c){
        Debug.Log("pexit"+c.getID());
    }
}
