using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    private int speed = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mouse")
        {
            Debug.Log("Cheese collected");
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.Rotate(0, speed, 0, Space.World);
    }
}
