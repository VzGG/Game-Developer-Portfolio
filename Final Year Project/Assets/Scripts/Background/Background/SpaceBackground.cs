using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class to change the background image of the game AT EVERY START METHOD/ at every level change
/// </summary>
public class SpaceBackground : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        ChooseRandomBackground();
    }

    private void ChooseRandomBackground()
    {
        if (sprites == null) { return; }

        // Randomly choose a background
        int randomIndex = Random.Range(0, sprites.Count);
        Sprite randomBackground = sprites[randomIndex];

        // Set current sprite renderer to the random background
        spriteRenderer.sprite = randomBackground;
    }
}
