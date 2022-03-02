using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Tower> towersPlaced = new List<Tower>();
    public List<Enemy> enemies = new List<Enemy>();
    
    //Health left for the player
    public int health = 100;
}
