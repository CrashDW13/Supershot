using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//Calculate sthe player's rank based on how well they performed. That rank is then printed onto
//the end screen, if and when the player makes it there. 
public class EndScore : MonoBehaviour
{
  
    private TextMeshProUGUI textMesh;
    private string rank;
    private string damageRank;
    private string timeRank; 

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>(); 
        timeRank = CalculateTimeRank();
        damageRank = CalculateDamageRank();
        rank = CalculateFinalRank();

        //Unlock cursor, now that the game's over.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Only print the ranks if you made it to the end.

        if (RankData.status.Equals("WIN"))
        {
            textMesh.text +=
            "\nSPEED RANK: " + timeRank +
            "\nDAMAGE RANK: " + damageRank +
            "\nTOTAL RANK: " + rank;
        }

    }

    //Returns a rank based on how well the player performed in terms of speed.
    //To get S Rank, the player must clear the game in less than 3 minutes. Every minute that passes
    //after that makes the rank go down a tier. 
    private string CalculateTimeRank()
    {
        int rank = 0;
        int time = RankData.time; 


        //Technically speaking, this could return "ERROR" if the player managed to reach the goal before
        //a second had passed. But, I feel like given that this is a borderline impossibliity, this code should be fine. 

        rank += Mathf.CeilToInt(((time - 180)/60)) + 2;

        switch (rank) {
            case 0:
                return "S";
            case 1:
                return "A";
            case 2:
                return "B";
            case 3:
                return "C";
            case 4:
                return "D";
            case int n when n >= 5:
                return "F";
            default:
                return "ERROR"; 
        }
    }

    //Returns a rank based on how well the player performed in terms of damage.
    //To receive an S rank, the player must clear the game without taking damage.
    //Every three hits after that, the rank goes down a tier. 
    private string CalculateDamageRank()
    {
        int rank = 0; 
        int damage = RankData.damage;

        rank += Mathf.CeilToInt(damage / 3);

        switch (rank)
        {
            case 0:
                return "S";
            case 1:
                return "A";
            case 2:
                return "B";
            case 3:
                return "C";
            case 4:
                return "D";
            case int n when n >= 5:
                return "F";
            default:
                return "ERROR";
        }
    }

    //Returns the final rank based on the previous two ranks.
    //The only way to get a perfect S rank is to get an S rank in the other two categories.
    //Otherwise, the final rank is equal to the average of the other two ranks, rounded down (in terms of rank). 
    private string CalculateFinalRank()
    {
        int speedRank = Mathf.CeilToInt(((RankData.time - 180) / 60)) + 2;
        int damageRank = Mathf.CeilToInt(RankData.damage / 3);

        int rank = Mathf.CeilToInt((speedRank + damageRank) / 2);

        switch (rank)
        {
            case 0:
                return "S";
            case 1:
                return "A";
            case 2:
                return "B";
            case 3:
                return "C";
            case 4:
                return "D";
            case int n when n >= 5:
                return "F";
            default:
                return "ERROR";
        }
    }
}
