using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData")]
public class SO_LevelData : ScriptableObject
{

    public string mapName;
    public Level_State state;
    public EnviromentType enviroment;

    public Objectives primaryObjective; /// Destroy, collect
    public ActorType primaryObjectiveTarget; // Enemies, resources

    public Objectives secondaryOjective; /// Destroy, collect
    public ActorType secondaryObjectiveTarget; // Enemies, resources

    public List<ResourceType> availableResources;


}
