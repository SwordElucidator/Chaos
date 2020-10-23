using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CentralController : MonoBehaviour
{
    public Text leftHpText;
    public GameObject gameOver;
    public Player player;
    public Enemy[] enemies;

    public bool Win()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.hp > 0)
            {
                return false;
            }
        }

        return true;
    }
}
