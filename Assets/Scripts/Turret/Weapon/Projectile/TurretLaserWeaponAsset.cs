using UnityEngine;

namespace Turret.Weapon.Projectile
{
    [CreateAssetMenu(menuName = "Assets/Turret Laser Weapon Asset", fileName = "Turret Laser Weapon Asset")]
    public class TurretLaserWeaponAsset : TurretWeaponAssetBase
    {
        public float MaxDistance;
        public float Damage;
        public LineRenderer LineRendererPrefab;

        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretLaserWeapon(this, view);
        }
    }
}