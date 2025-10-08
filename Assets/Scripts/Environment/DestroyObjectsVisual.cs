using UnityEngine;

public class DestroyObjectsVisual : MonoBehaviour
{
    [SerializeField] private DestroyObjects destroyObjects;
    [SerializeField] private GameObject destroyVFX;

    private void Start()
    {
        destroyObjects.OnDestroyAtDamage += DestroyObject_OnDestroyAtDamage;
    }
    private void DestroyObject_OnDestroyAtDamage(object sender, System.EventArgs e)
    {
        ShowDestroyVFX();
    }
    private void ShowDestroyVFX()
    {
        Instantiate(destroyVFX, destroyObjects.transform.position, Quaternion.identity);
    }
    private void OnDestroy()
    {
        destroyObjects.OnDestroyAtDamage -= DestroyObject_OnDestroyAtDamage;
    }
}