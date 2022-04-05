using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum Sounds { walk, run, jump, land, attack, rangedattack, idle, hitted,death, LAST }

[System.Serializable]
public class SoundReference
{
    [HideInInspector] public string fontName;
    public Sounds SoundType;
    public FMODUnity.EventReference AudioClipEvent;
}
[ExecuteInEditMode]
public class AudioController : MonoBehaviour
{


    [Header("List of all sounds the object can play")]
    [SerializeField] List<SoundReference> soundReferences;

    Dictionary<int, Action> eventDict;

    Action[] actionsArr;
    private void OnValidate()
    {
        if (soundReferences.Count == 0) return;
        for (int i = 0; i < soundReferences.Count; i++)
        {
            soundReferences[i].fontName = soundReferences[i].SoundType.ToString();
        }

    }
    /// <summary>
    /// in the awake the dictionary is populated
    /// </summary>
    private void Awake()
    {

        eventDict = new Dictionary<int, Action>();


    }
    // Start is called before the first frame update
    void Start()
    {
        if (soundReferences.Count < 1) return;
        for (int i = 0; i < soundReferences.Count; i++)
        {
            FMODUnity.EventReference eventReference = soundReferences[i].AudioClipEvent;

            eventDict[soundReferences[i].SoundType.ToString().GetHashCode()] =
                () =>
                {
                    FMODUnity.RuntimeManager.PlayOneShot(eventReference, transform.position);

                };
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string Sound)
    {

        try
        {
            eventDict[Sound.GetHashCode()]?.Invoke();
        }
        catch
        {
            Debug.Log(Sound);
        }


    }

}
