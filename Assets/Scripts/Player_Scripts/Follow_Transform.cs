using UnityEngine;

public class Follow_Transform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform target;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + new Vector3(0,0,-0.5f);
    }
}
