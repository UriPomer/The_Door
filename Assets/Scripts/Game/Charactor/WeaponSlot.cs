using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public GameObject AddWeapon(GameObject weaponPrefab)
    {
        GameObject weapon = Instantiate(weaponPrefab);
        weapon.transform.SetParent(this.transform);
        return weaponPrefab;
	}
}
