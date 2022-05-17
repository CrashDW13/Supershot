using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The RankData class serves as a container for all of the data that needs to be transferred from the main game to the end screen.
//I know coupling code like this is bad, but unfortunately this data needs to be carried between scenes so the end screen can calculate
//the player's rank based on how well they did in-game, and this is the only practical solution I can think of.
public class RankData : MonoBehaviour
{
    //How long it took the player to beat the game, in seconds.
    public static int time;
    //How many hits the player took throughout the game. 
    public static int damage;
    //By "status," it means whether the player ended the game winning or losing. 
    public static string status; 
}
