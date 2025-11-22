using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    [Header("Tuning")]
    [SerializeField]
    public string _bulletType;
    [SerializeField]
    public bool _reloadable = false;
    [SerializeField]
    public float _damage;
}
