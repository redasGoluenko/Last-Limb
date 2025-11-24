using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.VFX;

public class BulletHitManager : MonoBehaviour
{
    [SerializeField] GameObject _bulletHole;
    [SerializeField] VisualEffect _bloodParticle;

    public void CreateBulletHole(Vector3 origin, Vector3 direction, float maxDistance, LayerMask layerMask)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, maxDistance, layerMask))
        {
            if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
            {
                VisualEffect blood = Instantiate(_bloodParticle, hit.point, Quaternion.LookRotation(hit.normal));
                blood.Play();
                Destroy(blood.gameObject, 2f);

            }
            else
            {
                float rotationDegree = Random.Range(0, 360);
                GameObject hole = Instantiate(_bulletHole, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
                hole.transform.Rotate(hit.normal, rotationDegree, Space.World);
                hole.transform.SetParent(hit.collider.transform);
                Destroy(hole, 15f);
            }
        }
    }


}
