using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollision : MonoBehaviour
{

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Cube")
        {
            FindObjectOfType<AudioManager>().Play("Thud");
        }
    }
}
