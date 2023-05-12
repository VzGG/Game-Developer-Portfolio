using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to a gameobject prefab to display a number and add movement with gravity pulling it down at the end.
/// </summary>
public class DamageNumbers : MonoBehaviour
{
    public bool IsCritical = false;
    [SerializeField] private Text _textComponent;
    private float _displayTextDuration = 2f;
    private string _damageText = "";

    public string DamageText 
    { 
        get
        {
            return _damageText;
        }
        set
        {
            _damageText = value;

            // When we change this, call the coroutine
            StopCoroutine("DisplayDamageNumber");
            StartCoroutine("DisplayDamageNumber");
        }
    }

    private IEnumerator DisplayDamageNumber()
    {
        _textComponent.text = "";

        yield return new WaitForSeconds(0.01f);


        _textComponent.text = DamageText;

        if (IsCritical)
        {
            _textComponent.color = Color.yellow;
            _textComponent.text += "!";
        }

        yield return new WaitForSeconds(_displayTextDuration);

        // Clear the text after a few seconds
        _textComponent.color = Color.white;
        _textComponent.text = "";
    }
}