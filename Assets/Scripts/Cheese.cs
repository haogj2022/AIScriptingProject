using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    GetCheeseState getCheeseState;
    private int speed = 1;

    private void Start()
    {
        getCheeseState = GameObject.Find("Get Cheese State").GetComponent<GetCheeseState>();
    }

    private void Update()
    {
        transform.Rotate(0, speed, 0, Space.World);       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mouse")
        {
            getCheeseState.numOfCheese += 1;
            Debug.Log("Cheese collected");
            gameObject.SetActive(false);
        }
    }
}
