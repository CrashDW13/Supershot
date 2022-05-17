using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//All this class does is change the text on the end screen depending on whether or not the player won or lost. 
public class VictoryText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();

        textMesh.text = "YOU " + RankData.status + "!";
    }
}
