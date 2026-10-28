using UnityEngine;
using StarterAssets;
using Unity.Cinemachine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO weaponSO;
    [SerializeField] CinemachineCamera playerFollowCamera;  
    [SerializeField] GameObject zoomVignette;

    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    FirstPersonController firstPersonController;
    Weapon currentWeapon;

    const string SHOOT_STRING = "Shoot";

    float timeSinceLastShot = 0f;
    float defaultFOV;
    float defaultRotationSpeed;

    void Awake()
    {
        starterAssetsInputs =
            GetComponentInParent<StarterAssetsInputs>();

        firstPersonController = GetComponentInParent<FirstPersonController>();
        animator = GetComponentInChildren<Animator>();
        currentWeapon = GetComponentInChildren<Weapon>();
        defaultFOV = playerFollowCamera.Lens.FieldOfView;
        defaultRotationSpeed = firstPersonController.RotationSpeed;
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

            playerFollowCamera.Lens.FieldOfView = weaponSO.ZoomAmount;
            zoomVignette.SetActive(true);
            firstPersonController.ChangeRotationSpeed(weaponSO.ZoomRotationSpeed);
        }
        else
        {
            Debug.Log("Not Zooming in");
            playerFollowCamera.Lens.FieldOfView = defaultFOV;
            firstPersonController.ChangeRotationSpeed(defaultRotationSpeed);
            zoomVignette.SetActive(false);
        }


    }

}