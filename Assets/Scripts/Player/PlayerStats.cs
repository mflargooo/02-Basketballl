using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Stats/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public int placement = 4;
    public int score = 0;
    public int eggCt = 0;
    public int powerupID = -1;
    public int id = -1;
}
