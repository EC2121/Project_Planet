using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMGR : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> enemies;
    public List<Transform> SpawnPoint;
    public int NumberOfBaseChompy;
    
    void Start()
    {
        enemies = new List<GameObject>();
        Transform Player = GameObject.FindGameObjectWithTag("Player").transform;
        Transform Roby = GameObject.FindGameObjectWithTag("Roby").transform;
        GameObject ChomperPrefab = Resources.Load<GameObject>("Enemies/ChomperBase/Chomper");
        EnemyData ChomperBaseData = Resources.Load<EnemyData>("Enemies/ChomperBase/BaseChomperData");
        for (int i = 0; i < SpawnPoint.Count; i++)
        {
            for (int j = 0; j < NumberOfBaseChompy; j++)
            {
                GameObject Go = Instantiate(ChomperPrefab, null);
                PrepareEnemy(Go,SpawnPoint[i].position,ChomperBaseData,Player,Roby);
                enemies.Add(Go);
            }
            
        }
    }

    public void PrepareEnemy(GameObject Go,Vector3 position,EnemyData Data,Transform Player,Transform Roby)
    {
        Go.transform.position = new Vector3(position.x + Random.insideUnitCircle.x * 10
         , 0.5f, position.z + Random.insideUnitCircle.y * 10);
        Enemy enemy = Go.GetComponent<Enemy>();
        enemy.LoadData(Data,Player,Roby);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
