using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using EventHandler = System.EventHandler;

namespace UnityTemplateProjects.Saves_Scripts
{
    public static class SaveSystem
    {
        public static bool newGame;
        public static event EventHandler OnSave, OnLoad;
        public static List<GameData> Saves = new List<GameData>();
        // per specificare ulteriori parametri da passare all'evento (usare il generic <> sull'evento)
        // public class OnSaveEventArgs : EventArgs
        // {
        //     public int CurrentSave;
        // }
        public static bool IsFirstInvoke
        {
            get { return isFirstInvoke;}
            set { isFirstInvoke = value; }
        }
        
        public static int CurrentSave = 0;

        private static string path = Application.dataPath+"/Saves"; //DateTime.Now.ToString();
        private static bool isFirstInvoke; //Mi serve per sapere se resettare le strutture dati della GameData
        

        public static void InvokeOnSave()
        {
            isFirstInvoke = true;
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
                //Aggiungere gamedata invece di sovrascrivere
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

            //Sceglie in quale slot andare a salvare
            OnSave_ListOfSaves(self);

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