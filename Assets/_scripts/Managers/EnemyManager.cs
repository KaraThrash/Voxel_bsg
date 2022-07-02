using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class EnemyEvent : UnityEvent<Enemy>
{

}

public class EnemyManager : Manager
{
    private Transform playerShip;
    private Transform parent_Enemy;

    public List<Enemy> enemies;

    public int maxOnScreen;


    public Transform AttackTarget()
    {
        if (playerShip == null)
        {
            playerShip = GameManager().Player().Ship().transform;
        }

        return playerShip;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager().GetPlayerDeathEvent().AddListener(PlayerDeathEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    public void SpawnEnemy(GameObject _enemy,Transform _spot)
    {
        if (EnemyParent().childCount >= maxOnScreen)
        { return ; }

        Enemy enemy = Instantiate(_enemy, _spot.position, _spot.rotation).GetComponent<Enemy>();
        enemy.transform.parent = EnemyParent();

        enemy.AttackTarget(playerShip);
        enemy.SetStance(Stance.aggressive);
       
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

    private string enemyParentName = "PARENT_Enemy";

    public Transform EnemyParent()
    {
        if (parent_Enemy == null)
        {
            if (enemyParentName.Length < 1)
            {
                enemyParentName = "PARENT_Enemy";// + this.GetType().ToString();
            }

            GameObject findParent = GameObject.Find(enemyParentName);


            if (findParent == null)
            {
                parent_Enemy = new GameObject(enemyParentName).transform;
            }
            else { parent_Enemy = findParent.transform; }
        }

        return parent_Enemy;
    }


    //private GameManager gameManager;


}
