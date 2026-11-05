using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{


    private void Awake()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit: " + collision.gameObject.name);
        }
    }
}


