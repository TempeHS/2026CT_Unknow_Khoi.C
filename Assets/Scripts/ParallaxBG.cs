using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    public GameObject player;
    private float playerX;

    void Update()
    {
        playerX = player.transform.position.x;

        transform.position = new Vector2(playerX/4, 0);
    }
}
