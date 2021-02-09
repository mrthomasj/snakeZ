using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    public GameHandler gameHandler;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Food":
                gameHandler.Eat();
                break;
            case "Tail":
                print("Morreu");
                gameHandler.GameOver();
                break;
        }
    }
}