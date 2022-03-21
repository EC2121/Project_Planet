using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects.Saves_Scripts
{
    public class MaiStats : MonoBehaviour
    {
        //test bools
        public bool SaveData = false, LoadData = false;
        
        public enum Abilities { Hologram, PhotonCannon, ForceField }

        public List<Abilities> CurrentAbilities = new List<Abilities>();
        //CooldownAbilita?
        public float MaxHealth = 100f;
        public float CurrentHealth = 100f;
        public int CollectedCoins = 0;
        //Transform
        public Vector3 Position; //public float[] Position = new float[3];
        public Quaternion Rotation;

        private MaiStats myStats;

        private void Start()
        {
            myStats = GetComponent<MaiStats>();
        }

        private void Update()
        {
            if (SaveData)
            {
                SavePlayer();
                SaveData = false;
            }
            if (LoadData)
            {
                LoadPlayer();
                LoadData = false;
            }
        }

        public void UpdateStats()
        {
            myStats.Position = transform.position;
            myStats.Rotation = transform.rotation;
            //??
        }
        
        //savemgr?
        public void SavePlayer()
        {
            Debug.Log("Entered in Save");
            try
            {
                //UPDATE PLAYER STATS
                UpdateStats();
                
                SaveSystem.SavePlayer(GetComponent<MaiStats>(), GetComponent<RobyStats>());//riferimento a Roby??
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            Debug.Log("Game Saved");
            
        }

        public void LoadPlayer()
        {
            //DISABILITARE IL CHARACTER CONTROLLER DURANTE IL RIPOSIZIONAMENTO
            transform.GetComponent<CharacterController>().enabled = false;
            PlayerData data = SaveSystem.LoadPlayer();

            #region Apply loaded data to MonoBehaviour
                CurrentAbilities = data.CurrentAbilities;
                MaxHealth = data.MaiMaxHealth;
                CurrentHealth = data.MaiCurrentHealth;
                CollectedCoins = data.CollectedCoins;

                Position.x = data.MaiPosition[0];
                Position.y = data.MaiPosition[1];
                Position.z = data.MaiPosition[2];

                Rotation.x = data.MaiRotation[0];
                Rotation.y = data.MaiRotation[1];
                Rotation.z = data.MaiRotation[2];
                Rotation.w = data.MaiRotation[3];
            #endregion

            #region Apply Loaded Data to Transform
                transform.position = new Vector3(Position.x, Position.y, Position.z);
                transform.rotation = new Quaternion(Rotation.x, Rotation.y, Rotation.z, Rotation.w);
            #endregion
            //ABILITARE IL CHARACTER CONTROLLER DURANTE IL RIPOSIZIONAMENTO
            transform.GetComponent<CharacterController>().enabled = true;

            Debug.Log("Game Loaded");
        }
        //caricare dati anche per Roby
        //Fine  SaveMgr
    }
}