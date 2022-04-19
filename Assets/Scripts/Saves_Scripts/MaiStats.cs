using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects.Saves_Scripts
{

    public class MaiStats : MonoBehaviour
    {
        
        
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
            SaveSystem.OnSave += SaveSystemOnOnSave;
            SaveSystem.OnLoad += SaveSystemOnOnLoad;
        }

        private void SaveSystemOnOnLoad(object sender, EventArgs e)
        {
            //DISABILITARE IL CHARACTER CONTROLLER DURANTE IL RIPOSIZIONAMENTO
            transform.GetComponent<CharacterController>().enabled = false;
            GameData data = SaveSystem.LoadPlayer(true);

            #region Apply Loaded Data to Transform

            //Se nel salvataggio ha la scatola
            if ((data.HasBox && !transform.GetComponent<Player_State_Machine>().HasBox) || 
                (!data.HasBox && transform.GetComponent<Player_State_Machine>().HasBox))
            {
                transform.GetComponent<Player_State_Machine>().CurrentState.Factory.Interactable().EnterState();     
            }

            transform.position = new Vector3(data.MaiPosition[0], data.MaiPosition[1]+5, data.MaiPosition[2]);
            transform.rotation = new Quaternion(data.MaiRotation[0], data.MaiRotation[1],
                data.MaiRotation[2], data.MaiRotation[3]);
            
            transform.GetComponent<Player_State_Machine>().HasKey = data.HasKey; 
           
            #endregion
            
            #region Apply loaded data to MonoBehaviour
            CurrentAbilities = data.CurrentAbilities;
            MaxHealth = data.MaiMaxHealth;
            CurrentHealth = transform.GetComponent<Player_State_Machine>().Hp = data.MaiCurrentHealth;
            CollectedCoins = data.CollectedCoins;

            UpdateStats(); //Aggiorna la struttura
            #endregion
            
            
            
            //ABILITARE IL CHARACTER CONTROLLER DURANTE IL RIPOSIZIONAMENTO
            transform.GetComponent<CharacterController>().enabled = true;
            
            //TODO Set State to Idle after load
            transform.GetComponent<Animator>().Play("Entry",0);
        }

        private void SaveSystemOnOnSave(object sender, EventArgs e)
        {
            try
            {
                //UPDATE PLAYER STATS
                UpdateStats();
                SaveSystem.SaveData(this.gameObject, true);
            }
            catch (Exception ex)
            {
                Debug.Log(e.ToString());
            }
        }
        

        public void UpdateStats()
        {
            
            Position = transform.position;
            Rotation = transform.rotation;
            //??
            //profit
        }

        //Add OnDisable - OnEnable ?
        private void OnDestroy()
        {
            SaveSystem.OnSave -= SaveSystemOnOnSave;
            SaveSystem.OnLoad -= SaveSystemOnOnLoad;
        }

        //Fine  SaveMgr
    }
}