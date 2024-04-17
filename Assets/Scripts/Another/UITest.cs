using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public TextMeshProUGUI text;
    Sheet _sheet;

    // Start is called before the first frame update
    void Start()
    {
        _sheet = GameObject.Find("Sheet").GetComponent<Sheet>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_sheet == null){
            text.text = "no Data found";
        }
        else{
            text.text = _sheet.Title + _sheet.Artist + _sheet.Genre +  _sheet.Difficulty.ToString();           
        }

    }
}
