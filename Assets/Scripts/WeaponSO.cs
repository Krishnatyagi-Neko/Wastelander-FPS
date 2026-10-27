using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO", order = 0)]
public class WeaponSO : ScriptableObject 
{
    public int damage = 1;
    public float firerate = 1f;

}

