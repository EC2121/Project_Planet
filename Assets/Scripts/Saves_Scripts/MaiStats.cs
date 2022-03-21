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
        public bool IsEnabled, IsVisible, IsAlive;
        public float MaxHealth = 100f;
        public float CurrentHealth = 100f;
        public int CollectedCoins = 0;
        //Transform
        public Vector3 Position; //public float[] Position = new float[3];
        public Quaternion Rotation;

        private void Start()
        {
            IsEnabled = IsAlive = IsVisible =  true;
        }

        private void Update()
        {
            if (SaveData)
            {
                SavePlayer(); //locale
                SaveData = false;
            }
            if (LoadData)
            {
                LoadPlayer(); //locale
                LoadData = false;
            }
        }

        public void UpdateStats()
        {
            
            Position = transform.position;
            Rotation = transform.rotation;
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
                SaveSystem.SaveData(this.gameObject, true);
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
            GameData data = SaveSystem.LoadPlayer(true);

            #region Apply Loaded Data to Transform

            //gameObject.SetActive(data.IsEnabled); //NON STA FUNZIONANDO
            //IsAlive =
            //IsVisible = 
                
            transform.position = new Vector3(Position.x, Position.y, Position.z);
            transform.rotation = new Quaternion(Rotation.x, Rotation.y, Rotation.z, Rotation.w);
            #endregion
            
            #region Apply loaded data to MonoBehaviour
                CurrentAbilities = data.CurrentAbilities;
                MaxHealth = data.MaiMaxHealth;
                CurrentHealth = data.MaiCurrentHealth;
                CollectedCoins = data.CollectedCoins;
                
                UpdateStats(); //Aggiorna la struttura
            #endregion
            
            //ABILITARE IL CHARACTER CONTROLLER DURANTE IL RIPOSIZIONAMENTO
            transform.GetComponent<CharacterController>().enabled = true;

            Debug.Log("Game Loaded");
        }
        //Fine  SaveMgr
    }
}