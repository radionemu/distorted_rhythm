using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Rank;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(string rank, string name, string score, bool isPlayer =false) {
        Rank.text = rank;
        Name.text = name;
        Score.text = score;
        if (isPlayer) {
            Rank.color = Color.green;
            Name.color = Color.green;
            Score.color = Color.green;
        }
    }
}
