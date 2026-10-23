
using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
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
            Shoot();

            starterAssetsInputs.shoot = false;
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }
        else
        {
            Debug.Log("Shot fired, but nothing was hit.");
        }
    }
}
