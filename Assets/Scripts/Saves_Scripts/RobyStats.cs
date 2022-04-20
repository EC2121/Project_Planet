using System;
using UnityEngine;

namespace UnityTemplateProjects.Saves_Scripts
{
    public class RobyStats : MonoBehaviour
    {
        public float MaxHealth = 100f;
        public float CurrentHealth = 100f;
        public Vector3 Position;
        public Quaternion Rotation;
        //Forma attuale?

        private void OnEnable()
        {
            SaveSystem.OnSave += SaveSystemOnOnSave;
            SaveSystem.OnLoad += SaveSystemOnOnLoad;
        }
        private void OnDisable()
        {
            SaveSystem.OnSave -= SaveSystemOnOnSave;
            SaveSystem.OnLoad -= SaveSystemOnOnLoad;
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
                return;
            }
        }
        private void SaveSystemOnOnLoad(object sender, EventArgs e)
        {
            GameData data = SaveSystem.LoadPlayer(true);
            
            #region Apply Loaded Data to Transform
            transform.gameObject.SetActive(false);

            transform.SetPositionAndRotation(new Vector3(data.RobyPosition[0], data.RobyPosition[1], data.RobyPosition[2]),
                                          new Quaternion(data.RobyRotation[0], data.RobyRotation[1],
                                                                data.RobyRotation[2], data.RobyRotation[3]));
            transform.gameObject.SetActive(true);
            transform.GetComponent<Script_Roby>().SwitchState(RobyStates.Idle);
            
            #endregion
            
            #region Apply loaded data to MonoBehaviour
            MaxHealth = data.RobyMaxHealth;
            CurrentHealth = transform.GetComponent<Script_Roby>().roby_Life = data.RobyCurrentHealth;
            
            
            UpdateStats();
            #endregion
            
            //TODO Set State to Idle
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