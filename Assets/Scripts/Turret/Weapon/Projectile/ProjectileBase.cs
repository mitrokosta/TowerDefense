using UnityEngine;

namespace Turret.Weapon.Projectile
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        public abstract ProjectileBase CreateProjectile();
    }
}