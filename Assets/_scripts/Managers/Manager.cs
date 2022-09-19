using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private GameManager gameManager;
    private EnemyManager enemyManager;
    private FleetManager fleetManager;
    // private PlayerManager playerManager;
    private Menus menuManager;
    private MapManager mapManager;
    private NpcManager npcManager;
    private ItemManager itemManager;
    private Player player;
    private ResourceManager resourceManager;
    public ScrollingText scrollingText;

    private WorldTime timeManager;

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

    public FleetManager FleetManager()
    {
        if (fleetManager == null)
        {
            fleetManager = FindObjectOfType<FleetManager>();
        }
        return fleetManager;
    }


    public ItemManager ItemManager()
    {
        if (itemManager == null)
        {
            itemManager = FindObjectOfType<ItemManager>();
        }
        return itemManager;
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
    public Player Player()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        return player;
    }

    public ResourceManager ResourceManager()
    {
        if (resourceManager == null)
        {
            resourceManager = FindObjectOfType<ResourceManager>();
        }
        return resourceManager;
    }


    public WorldTime TimeManager()
    {
        if (timeManager == null)
        {
            timeManager = FindObjectOfType<WorldTime>();
        }
        return timeManager;
    }

    public ScrollingText ScrollingText()
    {
        if (scrollingText == null)
        {
            scrollingText = FindObjectOfType<ScrollingText>();
        }
        return scrollingText;
    }

}
