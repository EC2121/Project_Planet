using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Saves_Scripts;

[System.Serializable]
public class EnemyStats
{
    //enum enemytype
    public EnemyType EnemyType;
    public float hp;
    public int GUID;
    public float[] EnemyPosition = new float[3]; 
    public float[] EnemyRotation = new float[4];
}

[System.Serializable]
public class CustomDictionary
{
    public string SpawnPointName;
    public List<EnemyStats> EnemyStatsList;
}

[System.Serializable]
public class GameData
{
    //Generic
    public bool IsEnabled, IsVisible, IsAlive;
    
    //MAI
    //Utilizzare una lista di enum per tenere traccia delle abilità sbloccate?
    public List<MaiStats.Abilities> CurrentAbilities;
    public float MaiMaxHealth, MaiCurrentHealth;
    public int CollectedCoins;
    public float[] MaiPosition = new float[3]; 
    public float[] MaiRotation = new float[4];
    public bool HasBox, HasKey;

    //Roby
    public float RobyMaxHealth, RobyCurrentHealth;
    public float[] RobyPosition = new float[3]; 
    public float[] RobyRotation = new float[4];

    //Crate
    public float[] CratePosition = new float[3]; 
    public float[] CrateRotation = new float[4];

    //Enemies
    public List<CustomDictionary> CustomDictionaries = new List<CustomDictionary>();

    private bool firstTime;
    
    public GameData(GameObject self)
    {
        Append_GameData(self);
    }

    //chiamo questo metodo se non devo creare da zero la struttura
    public void Append_GameData(GameObject self) 
    {
        //IsEnabled = self.activeSelf;

        if (self.CompareTag("Player"))
        {
            MaiStats mai = self.GetComponent<MaiStats>();
            HasBox = self.GetComponent<Player_State_Machine>().HasBox;
            HasKey = self.GetComponent<Player_State_Machine>().HasKey;
            //ID?
            CurrentAbilities = mai.CurrentAbilities;
            MaiMaxHealth     = mai.MaxHealth;
            MaiCurrentHealth = self.GetComponent<Player_State_Machine>().Hp;//mai.CurrentHealth;
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
            RobyCurrentHealth = self.GetComponent<Script_Roby>().roby_Life;//roby.CurrentHealth;
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
            stats.EnemyType = selfEnemy.enemyType;
            stats.hp = selfEnemy.Hp;
            //stats.isAlpha = selfEnemy.
            stats.EnemyPosition[0] = self.transform.position.x;
            stats.EnemyPosition[1] = self.transform.position.y;
            stats.EnemyPosition[2] = self.transform.position.z;
            
            stats.EnemyRotation[0] = self.transform.rotation.x;
            stats.EnemyRotation[1] = self.transform.rotation.y;
            stats.EnemyRotation[2] = self.transform.rotation.z;
            stats.EnemyRotation[3] = self.transform.rotation.w;

            if (SaveSystem.IsFirstInvoke)
            {
                CustomDictionaries.Clear(); //Così sono sicuro di non avere duplicati duplicati
                
                SaveSystem.IsFirstInvoke = false;
            }
            
            //Se è la prima volta che aggiungo un nemico creo un nuovo dizionario
            //Se non è la prima volta che aggiungo un nemico ed il nemico che devo aggiungere proviene da un nuovo spawn point
            if (CustomDictionaries.Count == 0 || (CustomDictionaries.Count != 0 && CustomDictionaries[CustomDictionaries.Count-1].SpawnPointName != self.transform.parent.name))
            {
                List<EnemyStats> enemyStatsList = new List<EnemyStats>();
                enemyStatsList.Add(stats);
                
                CustomDictionary customDictionary = new CustomDictionary();
                customDictionary.EnemyStatsList = enemyStatsList;
                customDictionary.SpawnPointName = self.transform.parent.name;//"SpawnPoint (1)"; HASHING?
                
                CustomDictionaries.Add(customDictionary);
            }
            else //aggiungo all'ultimo indice del dizionario un nemico 
            {
                CustomDictionaries[CustomDictionaries.Count-1].EnemyStatsList.Add(stats);
            }
            //TODO Altrimnenti devo andare a prendere l'indice della lista che ha lo stesso SpawnPointName con dell'oggetto con cui sto confrontando
            
            
        }
    }
    //Environment data
}
