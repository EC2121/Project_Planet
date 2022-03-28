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
        GameObject ChomperAlpha = Resources.Load<GameObject>("Enemies/ChomperAlpha/AlphaChomper");
        EnemyData ChomperBaseData = Resources.Load<EnemyData>("Enemies/ChomperBase/BaseChomperData");
        EnemyData ChomperAlphaData = Resources.Load<EnemyData>("Enemies/ChomperAlpha/AlphaChomperData");
        for (int i = 0; i < SpawnPoint.Count; i++)
        {
            //Chompy Alpha
            GameObject go = Instantiate(ChomperAlpha, null);
            PrepareEnemy(go, SpawnPoint[i].position, ChomperAlphaData, Player, Roby);
            enemies.Add(go);

            //Chompys Beta
            for (int j = 0; j < NumberOfBaseChompy; j++)
            {
                GameObject Go = Instantiate(ChomperPrefab, null);
                PrepareEnemy(Go,SpawnPoint[i].position,ChomperBaseData,Player,Roby);
                print(SpawnPoint[i].position);
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
