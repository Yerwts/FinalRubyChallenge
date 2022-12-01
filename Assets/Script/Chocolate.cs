using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chocolate : MonoBehaviour
{
    public AudioClip ChocoClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.ChangeSpeed(1);
            Destroy(gameObject);

            controller.PlaySound(ChocoClip);
        }

    }
}