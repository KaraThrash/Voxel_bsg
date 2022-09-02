using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Manager {

    //for switching maps and loading/unloading them

    public List<SO_LevelData> levels;

    public Map currentMap;




    public void Produce()
    {
        // Handles the result of levels that are defeated or left on defeated [lose fleet ships, give enemies resources]
        //named produce for uniformity in naming [enemy and fleet's produce resources at turn end]


        foreach (SO_LevelData el in GetLevels())
        {
            if (el.state != Level_State.complete)
            {

                if (el.state == Level_State.weak || el.state == Level_State.available)
                {

                    if (el.primaryObjective == Objectives.protect)
                    {
                        //failed to rescue
                        el.state = Level_State.complete;
                    }
                    else 
                    {

                        if (el.state == Level_State.strong)
                        {
                            //level at max strength, so gain resources
                            foreach (Stats jay in el.availableResources)
                            { 
                                EnemyManager().UpdateStats(jay, 1);

                            }
                        }
                        else
                        {
                            if (EnemyManager().stat_morale > 0)
                            {
                                EnemyManager().UpdateStats(Stats.morale,-1);

                                //because state is an enum adding one will increase the strength
                                el.state++;


                            }
                        }

                            
                    }
                    

                }



            }


        }



    }







    public List<SO_LevelData> GetLevels()
    {
        if (levels == null)
        {
            levels = new List<SO_LevelData>();
        }

        return levels;
    }


    public Map GetMap()
    {
        if (currentMap == null)
        {
            currentMap = FindObjectOfType<Map>();
        }
        return currentMap;
    }

    
}
