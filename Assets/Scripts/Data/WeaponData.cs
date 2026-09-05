using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int damage = 10;
    public float fireCooldown = 0.3f;
    public int projectilesPerShot = 1;
    public float spreadAngle = 0f;
    public float bulletSpeed = 12f;
    public float bulletLifetime = 2f;
}
