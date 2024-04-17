using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Circle : MonoBehaviour
{
    private bool isactive;
    private int id;


    private void Awake() {
        isactive = false;
    }

    public void SetID(int id){
        this.id = id;
    }

    public int getID(){
        return id;
    }
}