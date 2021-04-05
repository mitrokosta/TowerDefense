using Turret.Weapon.Projectile;
using UnityEngine;

namespace Turret.Weapon
{
    [CreateAssetMenu(menuName = "Assets/Turret Field Weapon Asset", fileName = "Turret Field Weapon Asset")]
    public class TurretFieldWeaponAsset : TurretWeaponAssetBase
    {
        public float MaxDistance;

        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretFieldWeapon(this, view);
        }
    }
}