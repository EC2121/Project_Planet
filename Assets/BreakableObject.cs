using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreakableObject : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> FragmentedObj;
    public Transform ForceOrigin;
    public float ExplosionRadius;
    public float ExplosionForce;
    public float UpwardsModifier;
    private bool CanExplode;
    [SerializeField] public LayerMask layerMask;
    [SerializeField] public ForceMode forceMode;
    public GameObject OcclusionPortal;
    void Start()
    {
        CanExplode = true;
        foreach (var item in FragmentedObj)
        {
            item.SetActive(false);
        }
       
    }


    public void OnFragment()
    {

        CameraShake.OnCameraShake?.Invoke(0.7f, 4f);
        if (OcclusionPortal != null)
        {
            OcclusionPortal.GetComponent<OcclusionPortal>().open = true;
        }


        this.gameObject.SetActive(false);
        foreach (var item in FragmentedObj)
        {
            item.SetActive(true);
        }
        Collider[] colliders = Physics.OverlapSphere(ForceOrigin.position, ExplosionRadius, layerMask);

        foreach (var item in colliders)
        {
            item.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce,ForceOrigin.position,ExplosionRadius,UpwardsModifier,forceMode);
        }

    }
    

    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.yKey.wasPressedThisFrame && CanExplode)
        {
            CanExplode = false;
            OnFragment();
        }
    }
}
