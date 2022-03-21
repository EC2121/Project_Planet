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
        private static string path = Application.dataPath; //DateTime.Now.ToString();
        
        
        public static void SavePlayer(MaiStats mai, RobyStats roby)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            PlayerData data = new PlayerData(mai, roby);
            using (FileStream stream = new FileStream(path+"/SaveTest1.txt", FileMode.Create))
            {
                //PlayerData data = new PlayerData(mai, roby);
                formatter.Serialize(stream, data);
            }

               // PlayerData data = new PlayerData(mai, roby);
                string json = JsonUtility.ToJson(data);
                File.WriteAllText(path+"/SaveTestJ.json", json);
                

        }

        
        public static PlayerData LoadPlayer()
        {
            if (File.Exists(path+"/SaveTest1.txt"))
            {
                Debug.Log(path+"/SaveTest1.txt");
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(path+"/SaveTest1.txt", FileMode.Open))
                {
                    PlayerData data = formatter.Deserialize(stream) as PlayerData;
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