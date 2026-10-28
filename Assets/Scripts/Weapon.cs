
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlash;

    public void Shoot(WeaponSO weaponSO)
    {
        Debug.Log("Weapon.Shoot() called!");

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
            Debug.Log("Muzzle flash played!");
        }
        else
        {
            Debug.LogWarning("Muzzle flash is not assigned!");
        }

        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hit,
            Mathf.Infinity))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);

            if (weaponSO.HitVFXPrefab != null)
            {
                Instantiate(
                    weaponSO.HitVFXPrefab,
                    hit.point,
                    Quaternion.identity
                );
            }

            EnemyHealth enemyHealth =
                hit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(weaponSO.damage);
                Debug.Log("Enemy damaged!");
            }
            else
            {
                Debug.Log("Hit object has no EnemyHealth component.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything!");
        }
    }
}
