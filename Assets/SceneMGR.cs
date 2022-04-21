using UnityEngine;

public class SceneMGR : MonoBehaviour
{
    private static SceneMGR SelfInstance;
    void Awake(){
        DontDestroyOnLoad (this);
         
        if (SelfInstance == null) {
            SelfInstance = this;
        } else {
            DestroyObject(gameObject);
        }
    }
}
