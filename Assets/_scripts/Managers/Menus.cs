
using UnityEngine;

public class Menus : MonoBehaviour {

    private GameManager gameManager;

    public Transform menuTransform;
    public Transform menuDeathChoice;
    public Transform menuInGameUI;


    public void StartInGame( )
    {
        DisableMenus();
        if (MenuInGameUI())
        { MenuInGameUI().gameObject.SetActive(true); }
    }

    public void PlayerDeath()
    {
        DisableMenus();
        if (MenuDeathChoice())
        { MenuDeathChoice().gameObject.SetActive(true); }

    }

    public void DisableMenus()
    {
        if (MenuInGameUI())
        { MenuInGameUI().gameObject.SetActive(false); }
        if (MenuBase())
        { MenuBase().gameObject.SetActive(false); }
        if (MenuDeathChoice())
        { MenuDeathChoice().gameObject.SetActive(false); }

    }




    public Transform MenuBase()
    { return menuTransform; }
    public Transform MenuDeathChoice()
    { return menuDeathChoice; }

    public Transform MenuInGameUI()
    { return menuInGameUI; }


    public GameManager GameManager()
    {
        if (gameManager == null)
        { gameManager = FindObjectOfType<GameManager>(); }

        return gameManager;
    }

}
