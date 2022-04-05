using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects.Saves_Scripts
{
    public class RobyStats : MonoBehaviour
    {
        public float MaxHealth = 100f;
        public float CurrentHealth = 100f;
        public Vector3 Position; //public float[] Position = new float[3];
        public Quaternion Rotation;
        //Forma attuale?

        private void Start()
        {
            SaveSystem.OnSave += SaveSystemOnOnSave;
            SaveSystem.OnLoad += SaveSystemOnOnLoad;
        }

        
        private void SaveSystemOnOnSave(object sender, EventArgs e)
        {
            //Debug.Log("Entered in Save ROBY FROM EVENT");
            try
            {
                //UPDATE PLAYER STATS
                UpdateStats();
                SaveSystem.SaveData(this.gameObject, true);
            }
            catch (Exception ex)
            {
                Debug.Log(e.ToString());
                return;
            }
            //Debug.Log("Game Saved ROBY FROM EVENT");
        }
        private void SaveSystemOnOnLoad(object sender, EventArgs e)
        {
            GameData data = SaveSystem.LoadPlayer(true);
            
            #region Apply Loaded Data to Transform
            transform.position = new Vector3(data.RobyPosition[0], data.RobyPosition[1], data.RobyPosition[2]);
            transform.rotation = new Quaternion(data.RobyRotation[0], data.RobyRotation[1],
                                                data.RobyRotation[2], data.RobyRotation[3]);
            #endregion
            
            #region Apply loaded data to MonoBehaviour
            MaxHealth = data.MaiMaxHealth;
            CurrentHealth = data.MaiCurrentHealth;
            
            UpdateStats();
            #endregion
            
            //TODO Set State to Idle
            //Debug.Log("Game Loaded ROBY FROM EVENT");
        }
        
        public void UpdateStats()
        {
            
            Position = transform.position;
            Rotation = transform.rotation;
            //??
            //profit
        }
    }
}