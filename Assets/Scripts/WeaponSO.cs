
using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponSO",
    menuName = "ScriptableObjects/WeaponSO",
    order = 0
)]
public class WeaponSO : ScriptableObject
{
    [Header("Weapon Stats")]
    public int damage = 1;

    [Tooltip("Time between shots in seconds")]
    public float firerate = 1f;

    [Header("Effects")]
    public GameObject HitVFXPrefab;
}
