using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectible : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.ammo <= controller.currentAmmo)
            {
                controller.ChangeAmmo(4);
                controller.AmmoText();
                Destroy(gameObject);


            }
        }
    }
}