using UnityEngine;

namespace Turret.Weapon.Projectile
{
    [CreateAssetMenu(menuName = "Assets/Turret Field Weapon Asset", fileName = "Turret Field Weapon Asset")]
    public class TurretFieldWeaponAsset : TurretWeaponAssetBase
    {
        public float MaxDistance;
        public float Damage;

        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretFieldWeapon(this, view);
        }
    }
}