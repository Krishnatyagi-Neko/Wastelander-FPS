using UnityEngine;
using StarterAssets;
using Unity.Cinemachine;
using TMPro;

public class ActiveWeapon : MonoBehaviour
{

    [SerializeField] WeaponSO startingWeapon;
    [SerializeField] CinemachineCamera playerFollowCamera;  
    [SerializeField] GameObject zoomVignette;
    [SerializeField] TMP_Text ammoText;

    WeaponSO currentWeaponSO;
    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    FirstPersonController firstPersonController;
    Weapon currentWeapon;

    const string SHOOT_STRING = "Shoot";

    float timeSinceLastShot = 0f;
    float defaultFOV;
    float defaultRotationSpeed;
    int currentAmmo;

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

    void Start()
    {
        SwitchWeapon(startingWeapon);
        AdjustAmmo(currentWeaponSO.MagazineSize);
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


    public void AdjustAmmo(int amount)
    {
        currentAmmo += amount;

        if(currentAmmo > currentWeaponSO.MagazineSize)
        {
            currentAmmo = currentWeaponSO.MagazineSize;
        }

        ammoText.text = currentAmmo.ToString("D2");

    }


    public void SwitchWeapon(WeaponSO weaponSO)
    {
        if (currentWeapon)
        {
            Destroy(currentWeapon.gameObject);
        }

        GameObject newWeapon = Instantiate(weaponSO.weaponPrefab, transform);
        currentWeapon = newWeapon.GetComponent<Weapon>();
        this.currentWeaponSO = weaponSO;
        AdjustAmmo(currentWeaponSO.MagazineSize);
    }

    void HandleShoot()
    {
        timeSinceLastShot += Time.deltaTime;

        if (currentWeaponSO == null || currentWeapon == null)
        {
            starterAssetsInputs.ShootInput(false);
            return;
        }

        if (timeSinceLastShot >= currentWeaponSO.firerate && currentAmmo > 0)
        {
            currentWeapon.Shoot(currentWeaponSO);

            if (animator != null)
                animator.Play(SHOOT_STRING, 0, 0f);

            timeSinceLastShot = 0f;

            // Only reset input for semi-automatic weapons.
            if (!currentWeaponSO.isAutomatic)
            {
                starterAssetsInputs.ShootInput(false);
            }
            AdjustAmmo(-1);
        }
    }

    void HandleZoom()
    {
        if(!currentWeaponSO.CanZoom) return;

        if(starterAssetsInputs.zoom)
        {

            playerFollowCamera.Lens.FieldOfView = currentWeaponSO.ZoomAmount;
            zoomVignette.SetActive(true);
            firstPersonController.ChangeRotationSpeed(currentWeaponSO.ZoomRotationSpeed);
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