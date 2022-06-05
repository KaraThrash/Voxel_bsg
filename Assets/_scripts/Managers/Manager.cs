using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private GameManager gameManager;
    public EnemyManager enemyManager;
    // public PlayerManager playerManager;
    public Menus menuManager;
    public MapManager mapManager;
    public NpcManager npcManager;
    public ItemManager itemManager;
    public WorldTime timeManager;

    public GameManager GameManager()
    {
        if (gameManager == null)
        { gameManager = FindObjectOfType<GameManager>(); }

        return gameManager;
    }

    public EnemyManager EnemyManager()
    {
        if (enemyManager == null)
        {
            enemyManager = FindObjectOfType<EnemyManager>();
        }
        return enemyManager;
    }

    public Menus MenuManager()
    {
        if (menuManager == null)
        {
            menuManager = FindObjectOfType<Menus>();
        }
        return menuManager;
    }

    public MapManager MapManager()
    {
        if (mapManager == null)
        {
            mapManager = FindObjectOfType<MapManager>();
        }
        return mapManager;
    }

}
