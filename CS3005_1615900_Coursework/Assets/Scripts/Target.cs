using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When attached to a gameobject, it should be able to find a target that is touching this hitbox. This is used only to register and remove targets. Used by player and enemies.
/// </summary>
public class Target : MonoBehaviour
{
    [SerializeField] private List<Health> _targets = new List<Health>();    
    [SerializeField] private BoxCollider2D _hitbox;                         // This should be the Search hitbox gameobject. 
    [SerializeField] private string _targetTag = "";                         // Only the given tag should be allowed to be added as a target. Either it is an Enemy or a Player.
    [SerializeField] private string _targetLayer = "";

    public List<Health> GetTargets() { return this._targets; }
    public BoxCollider2D GetBoxCollider2D() { return this._hitbox; }

    #region Collision Detection Logic - Finding An Enemy In Our Character's Hitbox

    /// <summary>
    /// This method is called when the player attack class' myHitBox finds an enemy within its box collider range.
    /// Then the enemy is registered as the player's current target.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Adds 1 target for us to attack, used for when using any attack animation
        //if (collision.tag == targetTag && _hitbox.IsTouching(collision.gameObject.GetComponent<CircleCollider2D>()))
        //    AddToMyTargets(collision.gameObject.GetComponent<Health>());

        if (collision.tag == _targetTag && _hitbox.IsTouching(collision.gameObject.GetComponent<Collider2D>())
            && collision.gameObject.layer ==  LayerMask.NameToLayer(_targetLayer))
        {
            AddToMyTargets(collision.gameObject.GetComponent<Health>());
        }
            
    }

    /// <summary>
    /// Upon exiting the player attack class' myHitBox this method is called to remove the current target.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == _targetTag && !_hitbox.IsTouching(collision.gameObject.GetComponent<Collider2D>())
            && collision.gameObject.layer == LayerMask.NameToLayer(_targetLayer))
        {
            RemoveToMyTargets(collision.gameObject.GetComponent<Health>());
        }   
    }

    #endregion


    /// <summary>
    /// Add a target to the list that which can be damaged. There should be no copies of the same target.
    /// </summary>
    /// <param name="health"></param>
    private void AddToMyTargets(Health health)
    {
        if (health.GetHealth() <= 0f) { return; }

        // Cannot add duplicate targets, only unique ones.
        for (int i = 0; i < _targets.Count; i++)
            if (_targets[i].gameObject.GetInstanceID() == health.gameObject.GetInstanceID())
                return;

        _targets.Add(health);
    }

    /// <summary>
    /// Remove a target from the list. Used for removing targets when enemies are outside the player's targer range
    /// or when the target dies.
    /// </summary>
    /// <param name="health"></param>
    private void RemoveToMyTargets(Health health)
    {
        // Remove the unique enemy from the given index.
        for (int i = 0; i < _targets.Count; i++)
            if (_targets[i].gameObject.GetInstanceID() == health.gameObject.GetInstanceID())
                _targets.RemoveAt(i);
    }

    #region Called in Animation Event tab - player's sword attack 3 landed

    public void ResizeToLargeTargetHitbox()
    {
        this._hitbox.offset = Vector2.zero;
        this._hitbox.size = new Vector2(PlayerAttack.largeHitBoxX, this._hitbox.size.y);
    }

    public void ResizeToOriginalTargetHitbox()
    {
        this._hitbox.offset = new Vector2(PlayerAttack.SmallHitboxOffsetX, this._hitbox.offset.y);
        this._hitbox.size = new Vector2(PlayerAttack.SmallHitboxSizeX, this._hitbox.size.y);
    }

    #endregion

}
