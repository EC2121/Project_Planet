using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Saves_Scripts;

[System.Serializable]
public class PlayerData
{
    //MAI
    //Utilizzare una lista di enum per tenere traccia delle abilità sbloccate?
    public List<MaiStats.Abilities> CurrentAbilities;// = new List<MeiStats.Abilities>();
    public float MaiMaxHealth, MaiCurrentHealth;
    public int CollectedCoins;
    public float[] MaiPosition = new float[3]; 
    public float[] MaiRotation = new float[4];

    //Roby
    public float RobyMaxHealth, RobyCurrentHealth;
    public float[] RobyPosition = new float[3]; 
    public float[] RobyRotation = new float[4];
    
    public PlayerData(MaiStats maiStats, RobyStats robyStats)
    {
        //MAI
        CurrentAbilities = maiStats.CurrentAbilities;
        MaiMaxHealth = maiStats.MaxHealth;
        MaiCurrentHealth = maiStats.CurrentHealth;
        CollectedCoins = maiStats.CollectedCoins;
        //TRANSFORM
        MaiPosition[0] = maiStats.Position.x;
        MaiPosition[1] = maiStats.Position.y;
        MaiPosition[2] = maiStats.Position.z;
        //ROTATION
        MaiRotation[0] = maiStats.Rotation.x;
        MaiRotation[1] = maiStats.Rotation.y;
        MaiRotation[2] = maiStats.Rotation.z;
        MaiRotation[3] = maiStats.Rotation.w;
        
        
        //ROBY
        RobyMaxHealth = robyStats.MaxHealth;
        RobyCurrentHealth = robyStats.CurrentHealth;
        //TRANSFORM
        RobyPosition[0] = robyStats.Position.x;
        RobyPosition[1] = robyStats.Position.y;
        RobyPosition[2] = robyStats.Position.z;
        //ROTATION
        RobyRotation[0] = robyStats.Rotation.x;
        RobyRotation[1] = robyStats.Rotation.y;
        RobyRotation[2] = robyStats.Rotation.z;
        RobyRotation[3] = robyStats.Rotation.w;
    }
    
    //Environment data
    
    //Enemies data
    
    
}
