using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.Menu
{
    public class AudioMenu : MonoBehaviour
    {
        public Slider SFX, Music;
    
        private FMOD.Studio.Bus music;
        private FMOD.Studio.Bus sfx;

        private void Awake()
        {
            try
            {
                music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
                sfx = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
            }
            catch (Exception e)
            {
                //Debug.Log(e.ToString());
            }
            
        }

        private void Start()
        {
            float tempAudio;
            try
            {
                sfx.getVolume(out tempAudio);
                SFX.value = (int) tempAudio;
                
                music.getVolume(out tempAudio);
                Music.value = (int) tempAudio;
            }
            catch (Exception e)
            {
            }
        }

        public void OnSFXValueChange(float value)
        {
            sfx.setVolume(value);
        }

        public void OnMusicValueChanged(float value)
        {
            music.setVolume(value);
        }
    }
}