using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectible : MonoBehaviour
{
    public AudioClip GearClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.ammo <= controller.currentAmmo)
            {
                controller.ChangeAmmo(4); // Adds 4 ammo
                controller.AmmoText(); // Changes Ammo UI
                Destroy(gameObject);

                controller.PlaySound(GearClip);
            }
        }
    }
}