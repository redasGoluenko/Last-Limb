using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reloading : MonoBehaviour
{
    public Shooting _gun;
    public string _bulletType;

    void OnTriggerEnter(Collider other)
    {
        BulletItem bullet = other.GetComponent<BulletItem>();
        if (bullet != null && CanAddBullet(_gun._bulletCount, _gun._maxBullets) && bullet._bulletType.Equals(_bulletType))
        {
            _gun.AddBullet();
            Destroy(bullet.gameObject);
        }
    }

    private bool CanAddBullet(int ammo, int maxCapacity)
    {
        return ammo < maxCapacity;
    }
}
