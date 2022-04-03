using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityTemplateProjects.Saves_Scripts;

[System.Serializable]
public class EnemyStats
{
    //enum enemytype
    public bool isAlpha;
    public float hp;
    public int GUID;
    public float[] EnemyPosition = new float[3]; 
    public float[] EnemyRotation = new float[4];
}


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
    public bool HasBox;

    //Roby
    public float RobyMaxHealth, RobyCurrentHealth;
    public float[] RobyPosition = new float[3]; 
    public float[] RobyRotation = new float[4];

    //Crate
    public float[] CratePosition = new float[3]; 
    public float[] CrateRotation = new float[4];

    //Enemies
    public List<EnemyStats> Enemies = new List<EnemyStats>();

    public GameData(GameObject self)
    {
        Append_GameData(self);
    }

    //chiamo questo metodo se non devo creare da zero la struttura
    public void Append_GameData(GameObject self) //OVERRIDE PER LE LISTE DI ENEMY?
    {
        //Ora mettere tutto dentro una lista :)
        //IsEnabled = self.activeSelf;

        if (self.CompareTag("Player"))
        {
            MaiStats mai = self.GetComponent<MaiStats>();
            HasBox = self.GetComponent<Player_State_Machine>().HasBox;

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
        else if (self.CompareTag("Box"))
        {
            CratePosition[0] = self.transform.position.x;
            CratePosition[1] = self.transform.position.y;
            CratePosition[2] = self.transform.position.z;
            
            CrateRotation[0] = self.transform.rotation.x;
            CrateRotation[1] = self.transform.rotation.y;
            CrateRotation[2] = self.transform.rotation.z;
            CrateRotation[3] = self.transform.rotation.w;    
        }
        else if (self.CompareTag("Enemy"))
        {
            //Enemies.Clear();
            Enemy selfEnemy = self.GetComponent<Enemy>();
            EnemyStats stats = new EnemyStats();

            stats.GUID = self.transform.GetInstanceID(); //KEY
            stats.hp = selfEnemy.Hp;
            //stats.isAlpha = selfEnemy.
            stats.EnemyPosition[0] = self.transform.position.x;
            stats.EnemyPosition[1] = self.transform.position.y;
            stats.EnemyPosition[2] = self.transform.position.z;
            
            stats.EnemyRotation[0] = self.transform.rotation.x;
            stats.EnemyRotation[1] = self.transform.rotation.y;
            stats.EnemyRotation[2] = self.transform.rotation.z;
            stats.EnemyRotation[3] = self.transform.rotation.w;
            
            Enemies.Add(stats);
            
        }
        //GUID DI ENEMYSTATS?  
    }

    //Environment data
    
    //Enemies data
    
    
}
