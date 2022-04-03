using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using UnityEditor;

//JSON?

namespace UnityTemplateProjects.Saves_Scripts
{
   
    
    public static class SaveSystem
    {
        public static event EventHandler OnSave, OnLoad;
        public static List<GameData> Saves = new List<GameData>();
        // per specificare ulteriori parametri da passare all'evento (usare il generic <> sull'evento)
        // public class OnSaveEventArgs : EventArgs
        // {
        //     public int CurrentSave;
        // }
        
        
        public static int CurrentSave = 0;

        private static string path = Application.dataPath+"/Saves"; //DateTime.Now.ToString();

        public static void InvokeOnSave()
        {
            OnSave?.Invoke(null, EventArgs.Empty); //Invoca l'evento se non è null (nessun subscriber)
        }

        public static void InvokeOnLoad()
        {
            OnLoad?.Invoke(null, EventArgs.Empty); //Invoca l'evento se non è null (nessun subscriber)    
        }
        
        private static void OnSave_ListOfSaves(GameObject sender)
        {
            //se esiste un elemento in quella posizione
            if (Saves.ElementAtOrDefault(CurrentSave) != null)
            {
                Saves[CurrentSave].Append_GameData(sender);
                
                //Saves[CurrentSave] = new GameData((GameObject)sender);  
                //Aggiungere gamedata invece di sovrascrivere?
            }
            else
            {
                Saves.Add(new GameData(sender));
            }
        }
        
        public static void SaveData(GameObject self, bool JSON)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
           
            //Deve far fare il savedata a tutte le classi che implementano il metodo (MAI, ROBY, ENEMIES)

            OnSave_ListOfSaves(self);
            
            //GameData data = new GameData(self);
            
            if (JSON)
            {
                string json = JsonUtility.ToJson(Saves[CurrentSave]);
                File.WriteAllText(path+"/NEWSaveTest.json", json);        
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(path+"/NEWSaveTest1.txt", FileMode.Create))
                {
                    //PlayerData data = new PlayerData(mai, roby);
                    formatter.Serialize(stream, Saves[CurrentSave]);
                }   
            }
        }
        
        public static GameData LoadPlayer(bool JSON)
        {
            if (JSON)
            {
                if (File.Exists(path + "/NEWSaveTest.json"))
                {
                    string jsonSave = File.ReadAllText(path + "/NEWSaveTest.json");
                    return JsonUtility.FromJson<GameData>(jsonSave);
                }
                else
                {
                    Debug.Log("JSONSaveFile not found!");
                    return null;    
                }
            }
            else
            {
                if (File.Exists(path+"/SaveTest1.txt"))
                {
                    Debug.Log(path+"/SaveTest1.txt");
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream stream = new FileStream(path+"/SaveTest1.txt", FileMode.Open))
                    {
                        GameData data = formatter.Deserialize(stream) as GameData;
                        return data;
                    }
                }
                else
                {
                    Debug.Log("SaveFile not found!");
                    return null;
                }    
            }
        }
        
    }
}