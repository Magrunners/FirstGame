using UnityEngine;
public class SelfDestroyVFX : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (ps && !ps.IsAlive())
        {
            SelfDestroy();
        }
    }
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
