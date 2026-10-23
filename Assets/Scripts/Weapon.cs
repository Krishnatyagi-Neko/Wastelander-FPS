
using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject hitVFXPrefab;
    [SerializeField] private float damageAmount  = 40f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] Animator animator;

    const string SHOOT_STRING= "Shoot";

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

        muzzleFlash.Play();
        animator.Play(SHOOT_STRING, 0, 0f);
        starterAssetsInputs.shoot = false;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Instantiate(hitVFXPrefab, hit.point, Quaternion.LookRotation(hit.normal));

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
