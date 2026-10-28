using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO", order = 0)]
public class WeaponSO : ScriptableObject 
{
    public GameObject weaponPrefab;
    public int damage = 1;
    public float firerate = 1f;
    public GameObject HitVFXPrefab;
    public bool isAutomatic = false;
    public bool CanZoom = false;
}

