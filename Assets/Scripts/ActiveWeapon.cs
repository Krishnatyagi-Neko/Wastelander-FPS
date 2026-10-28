
using UnityEngine;
using StarterAssets;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO weaponSO;

    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    Weapon currentWeapon;

    const string SHOOT_STRING = "Shoot";

    float timeSinceLastShot = 0f;

    void Awake()
    {
        Debug.Log("ActiveWeapon Awake called", this);

        starterAssetsInputs =
            GetComponentInParent<StarterAssetsInputs>();

        animator = GetComponentInChildren<Animator>();
        currentWeapon = GetComponentInChildren<Weapon>();

        Debug.Log("StarterAssetsInputs found: " +
            (starterAssetsInputs != null), this);

        Debug.Log("Weapon found: " +
            (currentWeapon != null), this);

        Debug.Log("WeaponSO assigned: " +
            (weaponSO != null), this);
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (starterAssetsInputs == null)
            return;

        if (starterAssetsInputs.shoot)
        {
            Debug.Log("Shoot input detected!", this);
            HandleShoot();
        }
    }

    public void SwitchWeapon(WeaponSO weaponSO)
    {
        Debug.Log("Switching weapon to: " + weaponSO.name, this);
    }

    void HandleShoot()
    {
        if (weaponSO == null || currentWeapon == null)
        {
            Debug.LogError("Weapon or WeaponSO is missing!", this);
            starterAssetsInputs.ShootInput(false);
            return;
        }

        if (timeSinceLastShot >= weaponSO.firerate)
        {
            Debug.Log("Calling Weapon.Shoot()", this);

            currentWeapon.Shoot(weaponSO);

            if (animator != null)
                animator.Play(SHOOT_STRING, 0, 0f);

            timeSinceLastShot = 0f;

            // Only reset input for semi-automatic weapons.
            if (!weaponSO.isAutomatic)
            {
                starterAssetsInputs.ShootInput(false);
            }
        }
    }
}
