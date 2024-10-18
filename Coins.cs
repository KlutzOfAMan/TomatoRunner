using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public float bobbingHeight = 0.1f; // The height of the bobbing
    public float bobbingSpeed = 2f; // The speed of the bobbing

    private Vector3 startPosition; // The starting position of the coin

    void Start()
    {
        // Store the initial position of the coin
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position based on a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;

        // Update the position of the coin
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

}
