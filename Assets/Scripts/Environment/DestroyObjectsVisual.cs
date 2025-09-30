using UnityEngine;

public class DestroyObjectsVisual : MonoBehaviour
{
    [SerializeField] private DestroyObjects _destroyObjects;
    [SerializeField] private GameObject _destroyVFX;

    private void Start()
    {
        _destroyObjects.OnDestroyAtDamage += DestroyObject_OnDestroyAtDamage;


    }
    private void DestroyObject_OnDestroyAtDamage(object sender, System.EventArgs e)
    {
        ShowDestroyVFX();
    }
    private void ShowDestroyVFX()
    {
        Instantiate(_destroyVFX, _destroyObjects.transform.position, Quaternion.identity);

    }
    private void OnDestroy()
    {
        _destroyObjects.OnDestroyAtDamage -= DestroyObject_OnDestroyAtDamage;
    }

}