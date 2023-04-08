using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Enemy;

namespace Oswald.Player
{
    /*
 * Animation Event tutorial and inspired by:
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) 'Complete C# Unity Game Developer 2D - Glitch Garden' ***2019 Course. 2021 course version provides different learning materials*** [Course]
 * Available at: https://www.udemy.com/course/unitycourse/
 * 
 */
    /// <summary>
    /// Contains all attack behaviours i.e., normal grounded attacks, midair attacks and bow attacks. 
    /// Also contain animation events which are called within the Animator's animation event tab.
    /// </summary>
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] public Weapon MyWeapon;

        [Header("My normal attack")]
        [SerializeField] float attackEnergy = 5f;                                               // How much the attack cost.
        [SerializeField] float myDamage = 5f;                                                   // How much damage can the player deal to the target.
        private float jumpAttackLandedBonus = 0f;                                               // After landing with the sword air attack 3, increase the total damage by this. This bonus is increased the longer the player is in mid air and while in sword air attack 3.
        [SerializeField] private float jumpAttackLandedBonusIncreaser = 0.5f;                   // Define in the inspector how much the bonus is given when sword air attack 3 lands.
        public static float largeHitBoxX { get; } = 0.5f;                                       // Define in the inspector how large the landed sword air 3 attack is.
        public static float SmallHitboxSizeX { get; } = 0.2337848f;                             // Define in the inspector how small the hitbox after sword air 3 attack has landed and finished animation.
        public static float SmallHitboxOffsetX { get; } = 0.1804348f;                           // Define in the inspector the offset X of the large hitbox.
        [SerializeField] Energy myEnergy;
        [SerializeField] BoxCollider2D myHitBox;                                                // The player's hitbox - when the player attacks, anyone in it should get damaged.

        public List<Health> MyTargets { private get; set; } = new List<Health>();

        [Header("Sound")]
        [SerializeField] AudioSource audioSource;                                               // The audio player.
        [SerializeField] AudioClip[] audioClips;                                                // Define this in the Editor.

        [Header("My special attack")]
        [SerializeField] GameObject arrowProjectile;
        [SerializeField] private bool hasBow = false;
        [SerializeField] private float bowDamage = 0f;
        [SerializeField] private float bowEnergy = 20f;
        [SerializeField] public ArrowProjectile.OnHitEffect localOnHit;
        [SerializeField] public Vector2 localFirstPushback;
        [SerializeField] public Vector2 localStayHitPushback;

        [SerializeField] public float skillDamage = 0f;
        [SerializeField] public Skill[] skills = new Skill[2];  // Define what to add in this skill list, can be bow skill, sword attack skill (TO BE ADDED)
        public float SkillEnergy { private get; set; } = 0f;

        public bool canCritical {  private get; set; } = false;
        public float criticalDamageMultiplier { private get; set; } = 1f;
        public float criticalChance { private get; set; } = 0f;
        private bool isCritical = false;

        #region Getter and Setters

        public void SetBowDamage(float bowDamage) { this.bowDamage = bowDamage; }
        public float GetMyBowDamage() { return this.bowDamage; }
        public void SetMyDamage(float damage) { this.myDamage = damage; }
        public float GetMyDamage() { return myDamage; }
        public void SetHasBow(bool status) { this.hasBow = status; }
        public BoxCollider2D GetMyHitBox() { return this.myHitBox; }

        #endregion

        private void CriticalHit()
        {
            if (!canCritical) { return; }

            int randomChance = Random.Range(0, 100);
            if (randomChance < criticalChance)
            {
                Debug.Log("Critical hit!");
                isCritical = true;
                criticalDamageMultiplier = 2.5f;
            }
        }

        private void ResetCriticalDamageMultiplier()
        {
            criticalDamageMultiplier = 1f;
            isCritical = false;
        }

        #region Sword Attack

        /// <summary>
        /// The updated attack based from the weapon class
        /// </summary>
        /// <param name="myRB2D"></param>
        /// <param name="isMidAir"></param>
        /// <param name="isNextAttackSword"></param>
        public void SwordAttack(Rigidbody2D myRB2D, bool isMidAir, bool isNextAttackSword)
        {
            Animator animator = GetComponent<Animator>();
            if (isMidAir)
            {
                // Change the anim controller to the anim controller specialised for air attacks.
                animator.runtimeAnimatorController = MyWeapon.AirComboAnimController;
                animator.SetTrigger("isAttacking");

                if (isNextAttackSword)
                {
                    myRB2D.velocity = new Vector2(0f, 3f);
                }
            }
            else
            {
                animator.runtimeAnimatorController = MyWeapon.GroundedComboAnimController;
                animator.SetTrigger("isAttacking");
            }
        }


        /// <summary>
        /// Change the player's current animation to an attack animation, which uses energy. If the player is mid air while attacking, it changes to mid air attack animations. 
        /// Otherwise, grounded attack animations are shown.
        /// </summary>
        /// <param name="myRB2D"></param>
        /// <param name="animatorController"></param>
        /// <param name="isMidAir"></param>
        /// <param name="isNextAttackSword"></param>
        public void SwordAttack(Rigidbody2D myRB2D, AnimatorController animatorController, bool isMidAir, bool isNextAttackSword)
        {
            animatorController.ChangeAnimationTrigger("isAttacking");
            if (isMidAir)
            {
                if (isNextAttackSword)
                {
                    myRB2D.velocity = new Vector2(0f, 3f);
                }
            }
        }

        #endregion Sword Attack

        #region Bow Attack

        // Probably need to rename this method to secondary attack
        public void BowAttack(Rigidbody2D myRB2D, bool isMidAir, bool isNextAttackBow)
        {
            if (MyWeapon.SecondaryWeapon == null) { return; }
            Animator animator = GetComponent<Animator>();
            if (isMidAir)
            {
                animator.runtimeAnimatorController = MyWeapon.AirComboAnimController;
                animator.SetTrigger("isBowAttacking");
                if (isNextAttackBow)
                {
                    myRB2D.velocity = new Vector2(0f, 3f);
                }
            }
            else
            {
                animator.runtimeAnimatorController = MyWeapon.GroundedComboAnimController;
                animator.SetTrigger("isBowAttacking");
            }
        }

        /// <summary>
        /// Change the player's current animation to Bow attack animation which launches an arrow projectile and decreases the player's energy.
        /// </summary>
        /// <param name="myRB2D"></param>
        /// <param name="animatorController"></param>
        /// <param name="isMidAir"></param>
        /// <param name="isNextAttackBow"></param>
        public void BowAttack(Rigidbody2D myRB2D, AnimatorController animatorController, bool isMidAir, bool isNextAttackBow)
        {
            // Only able to do so when player pick up has a bow picked up.
            if (!hasBow) { return; }

            animatorController.ChangeAnimationTrigger("isBowAttacking");
            if (isMidAir)
            {
                if (isNextAttackBow)
                {
                    myRB2D.velocity = new Vector2(0f, 3f);
                }
            }
        }

        #endregion Bow Attack

        #region Skill Attacks - Bow, Sword, Etc.

        public void SkillAttack(AnimatorController animatorController, int skillSlot)
        {
            StartCoroutine(skills[skillSlot].Effect(animatorController));
        }

        #endregion

        #region Called in the Animator's Animation Event - specifically the sword attack and bow attack animations

        // Called in the animation tab - from Character's animation.
        private void AnimationEventAttack_Bow()
        {
            CriticalHit();

            GameObject newArrowProjectile = Instantiate(arrowProjectile, transform.position, Quaternion.identity);
            ArrowProjectile aP = newArrowProjectile.GetComponent<ArrowProjectile>();

            aP.isCritical = this.isCritical;

            aP.SetPlayerController(this.GetComponent<PlayerController>());
            //aP.SetBowDamage(bowDamage + bowSkillDamage);
            aP.SetBowDamage( (bowDamage + skillDamage) * criticalDamageMultiplier );
            aP.SetPlayerScale(this.transform.localScale);
            // Determine the hit effect when spawning projectile. The value is changed by the PlayerNextAnim class.
            aP.SetOnHitEffect(localOnHit);
            aP.SetFirstHitPushback(localFirstPushback);
            aP.SetStayHitPushback(localStayHitPushback);

            // Add an SFX for releasing the arrow
            audioSource.PlayOneShot(audioClips[2]);
            // USe energy when we successfully launched a projectile
            Debug.Log("USING ENERGY");
            //myEnergy.UseEnergy(bowEnergy);
            myEnergy.UseEnergy(bowEnergy + SkillEnergy);

            ResetCriticalDamageMultiplier();
        }

        // Called in the animation tab - from Character's animation. Multiplier is set in the animation tab also.
        private void AnimationEventAttack_Sword(float multiplier)
        {
            if (MyTargets == null) { Debug.Log("Attacking the air..."); return; }

            CriticalHit();

            // Anyone in the hitbox (they should all be in the list of targets) should get damaged.
            // Also add pushback to the enemy.
            for (int i = 0; i < MyTargets.Count; i++)
            {
                MyTargets[i].gameObject.GetComponent<EnemyController>().
                            EnemyTakeDamage( ((myDamage * multiplier) + jumpAttackLandedBonus + skillDamage) * criticalDamageMultiplier,
                            gameObject,
                            localFirstPushback,
                            isCritical);

                if (MyTargets[i].GetHealth() <= 0)
                    MyTargets.RemoveAt(i);
            }
            Debug.Log("using energy, you might be forgetting incase of a bug");
            myEnergy.UseEnergy(attackEnergy + SkillEnergy);

            ResetCriticalDamageMultiplier();
        }
        // Called in the animation tab - the player's sword air attack 3.
        private void IncreaseJump3LandAttackValue()
        {
            jumpAttackLandedBonus += jumpAttackLandedBonusIncreaser;
        }

        // Called in the animation tab - the player's sword air attack 3 landed.
        private void ResetJump3LandAttackValue()
        {
            jumpAttackLandedBonus = 0f;
        }

        // Called in the animation tab - from Character's landed animation attack.
        private void ResizeToLargeHitBox()
        {
            // Increase the size of the hitbox.
            myHitBox.offset = Vector2.zero;
            myHitBox.size = new Vector2(largeHitBoxX, myHitBox.size.y);
        }

        // Called in the animation tab - from Character's landed animation attack.
        private void ResizeToSmallHitBox()
        {
            // Decrease the size and offset of the hitbox.
            myHitBox.offset = new Vector2(SmallHitboxOffsetX, myHitBox.offset.y);
            myHitBox.size = new Vector2(SmallHitboxSizeX, myHitBox.size.y);
        }

        private void PlaySFX(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }

        #endregion

    }
}