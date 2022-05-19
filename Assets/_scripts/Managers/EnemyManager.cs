using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameManager gameManager;
    public List<Enemy> enemies;


    // Start is called before the first frame update
    void Start()
    {
        GameManager().GetPlayerDeathEvent().AddListener(PlayerDeathEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        if (Enemies() == null) { return; }
        foreach (Enemy el in Enemies())
        {
            el.canAct = true;
        }
    }

    public void PlayerDeathEvent()
    {
        foreach (Enemy el in Enemies())
        {
            el.canAct = false;        
        }
    }

    public void AddEnemyToList(Enemy _enemy)
    { Enemies().Add(_enemy); }

    public List<Enemy> Enemies()
    {
        if (enemies == null || enemies.Count == 0)
        { 
            enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        }

        return enemies;
    }

    //private GameManager gameManager;
    public GameManager GameManager()
    {
        if (gameManager == null)
        { gameManager = FindObjectOfType<GameManager>(); }

        return gameManager;
    }

}
