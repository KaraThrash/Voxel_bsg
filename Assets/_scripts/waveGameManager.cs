using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class waveGameManager : MonoBehaviour {

    public int killstoadvance;
    public int raidersdestroyed;
    public int currentwave;
    public GameObject spwaner;
    public GameObject basestarSpawner;
    public GameObject enemyparent;
    public GameObject bulletparent;
    public List<GameObject> basestarSpawners;
    public Text raidersdestroyedtext;
    public Text jumptimertext;
    public Text wavenumber;
    public float nextwaveclock; //after jump until the wave starts
    public float timetojumpclock; //after goal for the wave is achieved and the wave is ready to end
    public GameObject ftljumpimage;
    public GameObject npcManager;



    // Use this for initialization
    void Start()
    {
        wavenumber.text = "1";
        raidersdestroyedtext.text = "1";
        startwave();
    }

    // Update is called once per frame
    void Update()
    {

        if (nextwaveclock != -1)
        {
            nextwaveclock -= Time.deltaTime;
            if (nextwaveclock <= 0)
            {
                nextwaveclock = -1;
                startwave();
            }
        }

        if (timetojumpclock != -1)
        {
            if (timetojumpclock > 2)
            { jumptimertext.text = "Prepare to Jump"; }
            else { jumptimertext.text = "Jump imminent"; }

            timetojumpclock -= Time.deltaTime;

            if (timetojumpclock <= 0)
            {
                jumptimertext.text = "";
                timetojumpclock = -1;
                endwave();
            }
        }


    }
    public void startwave()
    {
        jumptimertext.text = "";
        raidersdestroyed = 0;
        killstoadvance += 5;
        currentwave++;
        wavenumber.text = currentwave.ToString();
        raidersdestroyedtext.text = killstoadvance.ToString();

        basestarSpawner.GetComponent<Spawn>().spawnspeed = 25;
        basestarSpawner.GetComponent<Spawn>().count = 5;


        //(killstoadvance % 5);
    }
    public void endwave()
    {
        nextwaveclock = 5.0f;
        basestarSpawner.GetComponent<Spawn>().spawnspeed = -1;
        basestarSpawner.GetComponent<Spawn>().count = 0;
        ftljumpimage.GetComponent<FtlImageFade>().StartFade();
        foreach (Transform el in enemyparent.transform)
        {
            Destroy(el.gameObject);
        }
        foreach (Transform el2 in bulletparent.transform)
        {
            Destroy(el2.gameObject);
        }

    }
    public void RaiderDestroyed(int enemyvalue)
    {
        raidersdestroyed += enemyvalue;
        raidersdestroyedtext.text = (killstoadvance - raidersdestroyed).ToString();
        if (killstoadvance <= raidersdestroyed)
        {
            raidersdestroyedtext.text = "Spooling FTL";
            timetojumpclock = 5.0f;
            //endwave();

            // spwaner.GetComponent<Spawn>().SpawnStuff(killstoadvance);
            // basestarSpawners[basestarSpawners.Count % 5].GetComponent<Spawn>().spawnspeed = 15;



        }
    }

}