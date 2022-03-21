using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Saves_Scripts;


[System.Serializable]
public class GameData
{
    //Generic
    public bool IsEnabled, IsVisible, IsAlive;
    
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


    public GameData(GameObject self)
    {
        //Ora mettere tutto dentro una lista :)
        IsEnabled = self.activeSelf;
        //Debug.Log(self.activeSelf);
        
        if (self.CompareTag("Mai"))
        {
            MaiStats mai = self.GetComponent<MaiStats>();
            
            //ID?
            CurrentAbilities = mai.CurrentAbilities;
            MaiMaxHealth     = mai.MaxHealth;
            MaiCurrentHealth = mai.CurrentHealth;
            CollectedCoins   = mai.CollectedCoins;
            //TRANSFORM
            MaiPosition[0] = mai.Position.x;
            MaiPosition[1] = mai.Position.y;
            MaiPosition[2] = mai.Position.z;
            //ROTATION
            MaiRotation[0] = mai.Rotation.x;
            MaiRotation[1] = mai.Rotation.y;
            MaiRotation[2] = mai.Rotation.z;
            MaiRotation[3] = mai.Rotation.w;
        }
        else if (self.CompareTag("Roby"))
        {
            RobyStats roby = self.GetComponent<RobyStats>();
            
            //ID?
            RobyMaxHealth     = roby.MaxHealth;
            RobyCurrentHealth = roby.CurrentHealth;
            //TRANSFORM
            RobyPosition[0] = roby.Position.x;
            RobyPosition[1] = roby.Position.y;
            RobyPosition[2] = roby.Position.z;
            //ROTATION
            RobyRotation[0] = roby.Rotation.x;
            RobyRotation[1] = roby.Rotation.y;
            RobyRotation[2] = roby.Rotation.z;
            RobyRotation[3] = roby.Rotation.w;    
        }
        //Stats per nemici
        //if tag == enemy
        //aggiungi alla lista dei nemici
        
    }

    //Environment data
    
    //Enemies data
    
    
}
