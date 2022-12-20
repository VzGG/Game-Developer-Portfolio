using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to a gameobject prefab to display a number and add movement with gravity pulling it down at the end.
/// </summary>
public class DamageNumbers : MonoBehaviour
{
    [SerializeField] private Text damageText;
    [SerializeField] float deleteTime = 3f;
    [SerializeField] Vector2 randomX;
    [SerializeField] Vector2 randomY;
    [SerializeField] Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        Movement();
    }

    private void Movement()
    {
        float randX = Random.Range(randomX.x, randomX.y);
        float randY = Random.Range(randomY.x, randomY.y);

        rb2d.velocity = new Vector2(randX, randY);

        Destroy(gameObject, deleteTime);
    }
}