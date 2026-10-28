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
        starterAssetsInputs =
            GetComponentInParent<StarterAssetsInputs>();

        animator = GetComponentInChildren<Animator>();
        currentWeapon = GetComponentInChildren<Weapon>();
    }

    void Update()
    {

        if (starterAssetsInputs == null)
            return;

        if (starterAssetsInputs.shoot)
        {
            HandleShoot();
            HandleZoom();
        }
    }

    public void SwitchWeapon(WeaponSO weaponSO)
    {
        if (currentWeapon)
        {
            Destroy(currentWeapon.gameObject);
        }

        GameObject newWeapon = Instantiate(weaponSO.weaponPrefab, transform);
        currentWeapon = newWeapon.GetComponent<Weapon>();
        this.weaponSO = weaponSO;
    }

    void HandleShoot()
    {
        timeSinceLastShot += Time.deltaTime;

        if (weaponSO == null || currentWeapon == null)
        {
            starterAssetsInputs.ShootInput(false);
            return;
        }

        if (timeSinceLastShot >= weaponSO.firerate)
        {
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

    void HandleZoom()
    {
        if(!weaponSO.CanZoom) return;

        if(starterAssetsInputs.zoom)
        {
            Debug.Log("Zooming in");
        }
        else
        {
            Debug.Log("Not Zooming in");
        }


    }

}