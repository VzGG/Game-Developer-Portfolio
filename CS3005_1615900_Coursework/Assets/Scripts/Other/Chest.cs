using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractableEnvironment
{
    [SerializeField] private Animator _animator;
    [SerializeField] private BoxCollider2D _boxCollider2DForUI;
    [SerializeField] private GameObject _promptUI;
    private Vector2 _initialMovement = new Vector2(0f, 3.5f);
    private float _waitDuration = 0.5f;

    public GameObject spawnedEquipmentGameObj;

    private void OpenChestAnimation()
    {
        // There is only 1 parameter for all chests!
        AnimatorControllerParameter animatorControllerParameter = _animator.GetParameter(0);

        _animator.SetTrigger(animatorControllerParameter.name);
    }

    #region Unity Animator Event methods
    /// <summary>
    /// Called in the Unity's Animator Event tab
    /// </summary>
    private void OnChestOpen()
    {
        StartCoroutine(WaitForChestToOpen());
    }

    private IEnumerator WaitForChestToOpen()
    {
        yield return new WaitForSeconds(_waitDuration);

        // Call equipment randomizer to generate you an item
        //GameObject equipmentGameObj = FindObjectOfType<EquipmentRandomizer>().GenerateEquipment(transform.position).gameObject;

        spawnedEquipmentGameObj = FindObjectOfType<EquipmentRandomizer>().GenerateEquipment(transform.position).gameObject;

        // Add an opening effect
        //equipmentGameObj.GetComponent<Rigidbody2D>().velocity = new Vector2(_initialMovement.x, _initialMovement.y);
        spawnedEquipmentGameObj.GetComponent<Rigidbody2D>().velocity = new Vector2(_initialMovement.x, _initialMovement.y);

        // Disable this behaviour
        this.enabled = false;
        _animator.enabled = false;
        _boxCollider2DForUI.enabled = false;

        // Disable equipment collider for a moment to prevent players instantly equipping it.
        //BoxCollider2D equipmentBody = equipmentGameObj.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        BoxCollider2D equipmentBody = spawnedEquipmentGameObj.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        equipmentBody.enabled = false;

        yield return new WaitForSeconds(1.5f);

        equipmentBody.enabled = true;
    }

    #endregion Unity Animator Event methods

    #region Collision Detection

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ShowInteractionUI();

            // Set player's reference to this IInteractable
            Oswald.Player.PlayerController player = collision.gameObject.GetComponent<Oswald.Player.PlayerController>();
            player.interactableEnvironment = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HideInteractionUI();

            // Remove reference so we cannot use the interaction method
            Oswald.Player.PlayerController player = collision.gameObject.GetComponent<Oswald.Player.PlayerController>();
            player.interactableEnvironment = null;
        }
    }

    #endregion Collision Detection

    #region Interface methods

    public void Interaction(Object obj)
    {
        Debug.Log("About to open chest!");
        OpenChestAnimation();
        HideInteractionUI();
    }

    /// <summary>
    /// Show UI
    /// </summary>
    public void ShowInteractionUI()
    {
        // When someone is within range for a while, continously show UI
        _promptUI.SetActive(true);
    }

    public void HideInteractionUI()
    {
        _promptUI.SetActive(false);
    }

    #endregion Interface methods
}