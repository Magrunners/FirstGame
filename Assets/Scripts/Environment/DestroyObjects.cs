using System;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    public EventHandler OnDestroyAtDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Sword>())
        {
            OnDestroyAtDamage?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);

            NavMeshSurfaceManagment.Instance.RebakedNavMesh();
        }
    }
}
