using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityTemplateProjects.Saves_Scripts;
using Random = UnityEngine.Random;

[System.Serializable]
public class SpawnPoint
{
    public Transform SpawnPointLocation;
    public int NumberOfChompies;
}
public class EnemyMGR : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Hologram;
    private List<GameObject> enemies;
    public List<SpawnPoint> SpawnPoint;

    [HideInInspector] private Transform Player;
    [HideInInspector] private Transform Roby;
    [HideInInspector] private GameObject ChomperPrefab;
    [HideInInspector] private GameObject ChomperAlpha;
    [HideInInspector] private EnemyData ChomperBaseData;
    [HideInInspector] private EnemyData ChomperAlphaData;

    private void Awake()
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
            if (SpawnPoint[i].SpawnPointLocation == null)
                continue;
            
            //Chompy Alpha
            SpawnAlphaChomper(SpawnPoint[i].SpawnPointLocation, SpawnPoint[i].SpawnPointLocation.position);

            //Chompys Beta
            for (int j = 0; j < SpawnPoint[i].NumberOfChompies; j++)
            {
                SpawnBetaChomper(SpawnPoint[i].SpawnPointLocation, SpawnPoint[i].SpawnPointLocation.position);
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
        PrepareEnemy(Go, SpawnPosition, ChomperBaseData, Player, Roby, Hologram, PerfectPosition);
        if (Rotation != default(Quaternion))
        {
            Go.transform.rotation = Rotation;
        }
        enemies.Add(Go);
    }

    public void PrepareEnemy(GameObject Go, Vector3 position, EnemyData Data, Transform Player, Transform Roby, Transform Hologram, bool PerfectPosition = false)
    {
        if (!PerfectPosition)
        {
            Vector3 randomPosition = Random.insideUnitSphere * Data.PatrolMaxDistance + position;
            NavMeshHit navMeshHit;
            int patrolableAreaMask = 1 << 3;
            if (NavMesh.SamplePosition(randomPosition, out navMeshHit, Data.PatrolMaxDistance, patrolableAreaMask))
            {
                Go.transform.position = navMeshHit.position;
            }

        }
        else
        {
            Go.transform.position = position;
        }

        Enemy enemy = Go.GetComponent<Enemy>();
        enemy.LoadData(Data, Player, Roby, Hologram);
    }

    private void OnEnable()
    {
        SaveSystem.OnLoad += SaveSystemOnOnLoad;
    }

    private void SaveSystemOnOnLoad(object sender, EventArgs e)
    {
        GameData data = SaveSystem.LoadPlayer(true);
        //Se nel salvataggio non c'era nessun nemico
        if (data.CustomDictionaries.Count <= 0)
        {
            foreach (Transform child in transform)
            {
                foreach (Transform child1 in child)
                {
                    Destroy(child1.gameObject);
                }
            }
            return;
        }
        
        //Presuppongo che l'enemyMGR venga sempre messo sotto un GO che funge da SpawnMGR (quindi nella gerarchia contiene spawnPoint)
        foreach (CustomDictionary CD in data.CustomDictionaries)
        {
            //o fa un wipe di tutti i figli e li ricrea da zero o controlla per singolo elemento se esiste già o se è disattivo

            if (transform.Find(CD.SpawnPointName) == null || transform.Find(CD.SpawnPointName).gameObject.activeSelf == false)
            {//se non trovo alcuno spawnPoint o è disabilitato lo creo
                GameObject SP = new GameObject(CD.SpawnPointName);
                SP.transform.parent = transform; //ho creato un nuovo SP e l'ho associato allo spawnMGW

                SpawnEnemiesFromCustomDictionary(CD, true);
            }
            else if (transform.Find(CD.SpawnPointName) != null)  //se ha trovato lo spawn point
            {//se non ha lo stesso numero di figli
                foreach (Transform child in transform.Find(CD.SpawnPointName))
                {
                    Destroy(child.gameObject);
                }
                
                SpawnEnemiesFromCustomDictionary(CD,true);
            }
        }
    }

    /// <summary>
    /// Per ogni EnemyStats presente nella lista dei nemici ne controllo l'EnemyType e lo spawno
    /// </summary>
    /// <param name="CD">Il CustomDictionary preso dal salvataggio (GameData)</param>
    /// <param name="PerfectPosition">Per scegliere se spawnare i nemici nell'esatta posizione in cui si trovavano oppure spawnarli con un random</param>
    private void SpawnEnemiesFromCustomDictionary(CustomDictionary CD, bool PerfectPosition = false)
    {
        foreach (EnemyStats ES in CD.EnemyStatsList) //ripetizione
        {
            if (ES.EnemyType == EnemyType.AlphaChomper)
            {
                SpawnAlphaChomper(transform.Find(CD.SpawnPointName),
                    new Vector3(ES.EnemyPosition[0], ES.EnemyPosition[1], ES.EnemyPosition[2]),
                    new quaternion(ES.EnemyRotation[0], ES.EnemyRotation[1], ES.EnemyRotation[2], ES.EnemyRotation[3]), PerfectPosition);
            }
            else if (ES.EnemyType == EnemyType.Chomper)
            {
                SpawnBetaChomper(transform.Find(CD.SpawnPointName),
                    new Vector3(ES.EnemyPosition[0], ES.EnemyPosition[1], ES.EnemyPosition[2]),
                    new quaternion(ES.EnemyRotation[0], ES.EnemyRotation[1], ES.EnemyRotation[2], ES.EnemyRotation[3]), PerfectPosition);
            }
            //per avere una referenza a se stesso posso prendere l'ultimo nemico aggiunto nella lista "enemies"
            enemies[enemies.Count - 1].GetComponent<Enemy>().Hp = ES.hp;
        }
    }

    private void OnDisable()
    {
        SaveSystem.OnLoad -= SaveSystemOnOnLoad;
    }
}
