using UnityEngine;

public class OnParticleCollisions : MonoBehaviour
{
    public ParticleSystem roby_ParticleSystem;
    private void Awake()
    {
        roby_ParticleSystem = GetComponent<ParticleSystem>();
    }

    public void OnParticleCollision(GameObject other)
    {
        other.gameObject.GetComponentInParent<Enemy>().AddDamage(20, gameObject, false);
    }
}
