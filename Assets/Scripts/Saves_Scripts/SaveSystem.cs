using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using EventHandler = System.EventHandler;

namespace UnityTemplateProjects.Saves_Scripts
{
    
    public static class SaveSystem
    {
        public static Scene CurrentScene;
        public enum SaveType {None, Save, SaveOnNewGame, Load}

        public static SaveType saveType = SaveType.None;
        public static event EventHandler OnSave, OnLoad;
        public static List<GameData> Saves = new List<GameData>();
        public static bool IsFirstInvoke
        {
            get { return isFirstInvoke;}
            set { isFirstInvoke = value; }
        }
        
        public static int CurrentSave = 0;

        private static string path = Application.dataPath+"/Saves"; 
        private static bool isFirstInvoke; //Mi serve per sapere se resettare le strutture dati della GameData

        public static List<bool> PassedTrials = new List<bool>(3){false,false,false};

        public static void InvokeOnSave()
        {
            SaveSystem.saveType = SaveSystem.SaveType.Load;
            isFirstInvoke = true;
            OnSave?.Invoke(null, EventArgs.Empty); //Invoca l'evento se non è null (nessun subscriber)
            WriteOnFile();
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
                //Aggiungere gamedata invece di sovrascrivere
            }
            else
            {
                Saves.Add(new GameData(sender));
            }
        }

        public static void DirectoryCheck()
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
        }    
        
        public static void SaveData(GameObject self, bool JSON)
        {
            //Sceglie in quale slot andare a salvare
            OnSave_ListOfSaves(self);
        }

        public static void WriteOnFile(bool JSON = true)
        {
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