
using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damageAmount  = 40f;
    private StarterAssetsInputs starterAssetsInputs;

    void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (starterAssetsInputs == null)
            return;

        if (starterAssetsInputs.shoot)
        {
            HandleShoot();

            starterAssetsInputs.shoot = false;
        }
    }

    void HandleShoot()
    {
        Ray ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            enemyHealth?.TakeDamage(damageAmount);
            // if (enemyHealth != null)
            // {
            //     enemyHealth.TakeDamage(damageAmount);
            // }
        }
        else
        {
            Debug.Log("Shot fired, but nothing was hit.");
        }
    }
}
