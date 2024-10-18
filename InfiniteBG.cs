using UnityEngine;

public class InfiniteBG : MonoBehaviour
{
    public Transform player;        // Reference to the player or camera that moves
    public float parallaxEffect;    // Control how much the background should move (0 = no movement, 1 = same speed as player)

    private float length, startPosX;

    void Start()
    {
        startPosX = transform.position.x; // Initial position of the background
        length = GetComponent<SpriteRenderer>().bounds.size.x; // Width of the background sprite
    }

    void Update()
    {
        // Calculate the parallax based on player movement
        float distance = (player.position.x * parallaxEffect);
        transform.position = new Vector3(startPosX + distance, transform.position.y, transform.position.z);

        // Check if the background has moved too far and reset it to loop seamlessly
        float temp = player.position.x * (1 - parallaxEffect);
        if (temp > startPosX + length) startPosX += length;
        else if (temp < startPosX - length) startPosX -= length;
    }
}