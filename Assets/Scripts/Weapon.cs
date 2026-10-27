
using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private GameObject hitVFXPrefab;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Animator animator;

    [Header("Shooting")]
    [SerializeField] private float range = 100f;

    private StarterAssetsInputs starterAssetsInputs;

    private const string SHOOT_STRING = "Shoot";

    private void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (starterAssetsInputs == null)
            return;

        if (starterAssetsInputs.shoot)
        {
            // Reset input so one press produces one shot.
            starterAssetsInputs.shoot = false;

            HandleShoot();
        }
    }

    private void HandleShoot()
    {
        // Check required references.
        if (weaponSO == null)
        {
            Debug.LogError("WeaponSO is not assigned!", this);
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("No camera tagged MainCamera found!");
            return;
        }

        // Play weapon effects.
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (animator != null)
            animator.Play(SHOOT_STRING, 0, 0f);

        // Shoot from the centre of the camera.
        Ray ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Shot hit: " + hit.collider.name);

            // Spawn hit effect at the impact point.
            if (hitVFXPrefab != null)
            {
                Instantiate(
                    hitVFXPrefab,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );
            }

            // Search the hit object AND its parent objects.
            EnemyHealth enemyHealth =
                hit.collider.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                Debug.Log("Enemy hit! Damage: " + weaponSO.damage);

                enemyHealth.TakeDamage(weaponSO.damage);
            }
            else
            {
                Debug.Log("Hit object has no EnemyHealth component: "
                    + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("Shot fired, but nothing was hit.");
        }
    }
}
