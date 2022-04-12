using System;
using System.Diagnostics;
using System.Timers;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UnityTemplateProjects.Saves_Scripts
{
    public class CheckPoint : MonoBehaviour
    {
        private static Timer saveTimer = new Timer(1);
        private bool canSave = true;

        private void Awake()
        {
            saveTimer.Elapsed += (sender, args) => canSave = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (canSave)
                {
                    saveTimer.Interval = 30000; // 30 secondi
                    saveTimer.Start();
                    canSave = false;
                    SaveMGR.Save();
                    Debug.Log("Saved from Checkpoint");
                }
                
            }
        }
    }
}