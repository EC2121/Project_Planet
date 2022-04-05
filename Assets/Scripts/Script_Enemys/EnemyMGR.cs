using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityTemplateProjects.Saves_Scripts;
using Random = UnityEngine.Random;

public class EnemyMGR : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Hologram;
    List<GameObject> enemies;
    public List<Transform> SpawnPoint;
    public int NumberOfBaseChompy;

    [HideInInspector] private Transform Player;
    [HideInInspector]private Transform Roby;
    [HideInInspector]private GameObject ChomperPrefab;
    [HideInInspector]private GameObject ChomperAlpha;
    [HideInInspector]private EnemyData ChomperBaseData;
    [HideInInspector]private EnemyData ChomperAlphaData;
    
    void Awake()
    {
        Transform Hologram = this.Hologram;
        enemies = new List<GameObject>(); //TODO fare pulizia
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Roby = GameObject.FindGameObjectWithTag("Roby").transform; 
        ChomperPrefab = Resources.Load<GameObject>("Enemies/ChomperBase/Chomper");
        ChomperAlpha = Resources.Load<GameObject>("Enemies/ChomperAlpha/AlphaChomper");
        ChomperBaseData = Resources.Load<EnemyData>("Enemies/ChomperBase/BaseChomperData");
        ChomperAlphaData = Resources.Load<EnemyData>("Enemies/ChomperAlpha/AlphaChomperData");
        
        for (int i = 0; i < SpawnPoint.Count; i++)
        {
            //Chompy Alpha
            SpawnAlphaChomper(SpawnPoint[i], SpawnPoint[i].position);

            //Chompys Beta
            for (int j = 0; j < NumberOfBaseChompy; j++)
            {
                SpawnBetaChomper(SpawnPoint[i], SpawnPoint[i].position);
            }
        }
    }

    public void SpawnAlphaChomper(Transform parent, Vector3 SpawnPosition, Quaternion Rotation = default(Quaternion), bool PerfectPosition = false)
    {
        GameObject go = Instantiate(ChomperAlpha, parent);
        go.tag = "Enemy";
        PrepareEnemy(go, SpawnPosition, ChomperAlphaData, Player, Roby, Hologram, PerfectPosition);
        if (Rotation != default(Quaternion))
        {
            go.transform.rotation = Rotation;
        }
        enemies.Add(go);    
    }
    
    public void SpawnBetaChomper(Transform parent, Vector3 SpawnPosition, Quaternion Rotation = default(Quaternion), bool PerfectPosition = false)
    {
        GameObject Go = Instantiate(ChomperPrefab, parent);
        Go.tag = "Enemy";
        PrepareEnemy(Go,SpawnPosition,ChomperBaseData,Player,Roby, Hologram, PerfectPosition);
        print(SpawnPosition);
        if (Rotation != default(Quaternion))
        {
            Go.transform.rotation = Rotation;
        }
        enemies.Add(Go);       
    }

    public void PrepareEnemy(GameObject Go,Vector3 position,EnemyData Data,Transform Player,Transform Roby, Transform Hologram, bool PerfectPosition = false)
    {
        if (!PerfectPosition)
        {
            Go.transform.position = new Vector3(position.x + Random.insideUnitCircle.x * 10
                , 0.5f, position.z + Random.insideUnitCircle.y * 10);    
        }
        else
        {
            Go.transform.position = position;
        }
        
        Enemy enemy = Go.GetComponent<Enemy>();
        enemy.LoadData(Data,Player,Roby,Hologram);
    }

    private void OnEnable()
    {
        SaveSystem.OnLoad += SaveSystemOnOnLoad;
    }

    private void SaveSystemOnOnLoad(object sender, EventArgs e)
    {
        GameData data = SaveSystem.LoadPlayer(true);
        //Presuppongo che l'enemyMGR venga sempre messo sotto un GO che funge da SpawnMGR (quindi nella gerarchia contiene spawnPoint)
        foreach (CustomDictionary CD in data.CustomDictionaries)
        {
            //o fa un wipe di tutti i figli e li ricrea da zero o controlla per singolo elemento se esiste già o se è disattivo
            
            if (transform.Find(CD.SpawnPointName) == null || transform.Find(CD.SpawnPointName).gameObject.activeSelf == false) 
            {//se non trovo alcuno spawnPoint o è disabilitato lo creo
                GameObject SP = new GameObject(CD.SpawnPointName); 
                SP.transform.parent = this.transform; //ho creato un nuovo SP e l'ho associato allo spawnMGW

                foreach (EnemyStats ES in CD.EnemyStatsList)
                {
                    if (ES.EnemyType == EnemyType.AlphaChomper) 
                    {
                        SpawnAlphaChomper(SP.transform, new Vector3(ES.EnemyPosition[0], ES.EnemyPosition[1], ES.EnemyPosition[2]),
                            new quaternion(ES.EnemyRotation[0], ES.EnemyRotation[1], ES.EnemyRotation[2], ES.EnemyRotation[3] ));
                    } 
                    else if (ES.EnemyType == EnemyType.Chomper)
                    {
                        SpawnBetaChomper(SP.transform, new Vector3(ES.EnemyPosition[0], ES.EnemyPosition[1], ES.EnemyPosition[2]),
                            new quaternion(ES.EnemyRotation[0], ES.EnemyRotation[1], ES.EnemyRotation[2], ES.EnemyRotation[3] ));        
                    }
                    //per avere una referenza a se stesso posso prendere l'ultimo nemico aggiunto nella lista "enemies"
                    enemies[enemies.Count - 1].GetComponent<Enemy>().Hp = ES.hp;
                }
            }
            else if (transform.Find(CD.SpawnPointName) != null)  //se ha trovato lo spawn point
            {//se non ha lo stesso numero di figli
                foreach(Transform child in transform.Find(CD.SpawnPointName))
                {
                    Destroy(child.gameObject);
                }
                
                foreach (EnemyStats ES in CD.EnemyStatsList) //ripetizione
                {
                    if (ES.EnemyType == EnemyType.AlphaChomper) 
                    {
                        SpawnAlphaChomper(transform.Find(CD.SpawnPointName), new Vector3(ES.EnemyPosition[0], ES.EnemyPosition[1], ES.EnemyPosition[2]),
                            new quaternion(ES.EnemyRotation[0], ES.EnemyRotation[1], ES.EnemyRotation[2], ES.EnemyRotation[3] ));
                    } 
                    else if (ES.EnemyType == EnemyType.Chomper)
                    {
                        SpawnBetaChomper(transform.Find(CD.SpawnPointName), new Vector3(ES.EnemyPosition[0], ES.EnemyPosition[1], ES.EnemyPosition[2]),
                            new quaternion(ES.EnemyRotation[0], ES.EnemyRotation[1], ES.EnemyRotation[2], ES.EnemyRotation[3] ));        
                    }
                    //per avere una referenza a se stesso posso prendere l'ultimo nemico aggiunto nella lista "enemies"
                    enemies[enemies.Count - 1].GetComponent<Enemy>().Hp = ES.hp;
                }
            }
        }
        
    }

    private void OnDisable()
    {
        SaveSystem.OnLoad -= SaveSystemOnOnLoad;
    }
}
