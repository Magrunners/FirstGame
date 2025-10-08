using UnityEngine;
public class SelfDestroyVFX : MonoBehaviour
{
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (_ps && !_ps.IsAlive())
        {
            SelfDestroy();
        }
    }
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
