using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Artist;
    public TextMeshProUGUI Genre;
    Sheet _sheet;

    // Start is called before the first frame update
    void Start()
    {
        _sheet = GameObject.Find("Sheet").GetComponent<Sheet>();

        if(_sheet == null){
            Title.text = "no Data found";
        }
        else{
            Title.text = _sheet.Title;
            Artist.text = _sheet.Artist;
            Genre.text = _sheet.Genre;           
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
