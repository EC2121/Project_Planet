using System;
using  UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
//JSON?

namespace UnityTemplateProjects.Saves_Scripts
{
   
    
    public static class SaveSystem
    {
        private static string path = Application.dataPath+"/Saves"; //DateTime.Now.ToString();
        
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
           
            
            GameData data = new GameData(self);
            
            if (JSON)
            {
                string json = JsonUtility.ToJson(data);
                File.WriteAllText(path+"/NEWSaveTest.json", json);        
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(path+"/NEWSaveTest1.txt", FileMode.Create))
                {
                    //PlayerData data = new PlayerData(mai, roby);
                    formatter.Serialize(stream, data);
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
                return null;
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