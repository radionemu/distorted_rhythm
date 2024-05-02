using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    public GameObject CellPrefabs;
    public GameObject CellList;
    List<GameObject> Lists=new();

    // Start is called before the first frame update
    void Start()
    {
        // CallViewRank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CallViewRank(){
        StartCoroutine(ViewRank());
        return true;
    }

    IEnumerator ViewRank(){
        foreach(GameObject obj in Lists){
            Destroy(obj);
        }
        Lists.Clear();
        WWW www = new WWW("http://106.246.242.58:11345/demo/rank");
        yield return www;
        if(www.text[0]=='0'){
            // Debug.Log(www.text);
            string [] str = www.text.Split('\t')[1..];
            foreach (string rankrow in str){
                GameObject tmp = Instantiate(CellPrefabs,CellList.transform);
                TextMeshProUGUI rnknum = tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI rnkname = tmp.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI rnkscore = tmp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                string[] rankcells = rankrow.Split(',');
                rnknum.text = string.Format("{0:D3}",int.Parse(rankcells[0]));
                rnkname.text = rankcells[1];
                rnkscore.text = string.Format("{0:D7}",int.Parse(rankcells[2]));
                Lists.Add(tmp);
            }
        }
    }
}
