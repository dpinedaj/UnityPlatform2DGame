using UnityEngine;
using System.Collections;

namespace Backup
{
    public class AttackController: MonoBehaviour
    {
        // Our own animator
        private Animator _unitAnimator;

        // Target attack controller, used temporary when playing attack animations
        private AttackController _target;

        // Flag that will help us with some animation timings
        private bool _waitingForTargetDamageAnimation;

        private void Awake()
        {
            // Let’s cache the animator so we don’t have to grab it every time
            this._unitAnimator = GetComponent<Animator>();
        }

        public IEnumerator PlayAttackAnimations(int attackTimes, AttackController targetAttackController)
        {
            // Temporary store our _target attack controller.
            this._target = targetAttackController;
            // And setup our flag
            this._waitingForTargetDamageAnimation = true;

            // Play this attack sequence as many times as requested
            for (var i = 0; i < attackTimes; i++)
            {
                // Fire the “attack” trigger
                _unitAnimator.SetTrigger("attack");

                // We know that we will eventually transition back to Idle, so we wait
                while (!_unitAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    yield return null; // Wait a single frame and try again.
                }

                // We also want to wait for our _target to finish playing its Damage animation
                while (this._waitingForTargetDamageAnimation)
                {
                    yield return null;
                }
            }

            // Clean up
            this._target = null;
        }

        // This is called automatically by Unity when the “DamageTarget” animation event fires
        // (method name has to be the same as the animation event name).
        private void DamageTarget()
        {
            StartCoroutine(TriggerTargetDamageAnimation());
        }

        // We want to wait for the _target damage animation to complete before moving on, so we need a nested coroutine
        private IEnumerator TriggerTargetDamageAnimation()
        {
            // Nested coroutine, notice that we “yield return StartCoroutine” instead of just “StartCoroutine”.
            // Basically, we will not continue this method until the _target coroutine is complete.
            yield return StartCoroutine(this._target.PlayDamageAnimation());

            // Target coroutine is complete, let’s tell the attack animation coroutine that we are no longer waiting
            this._waitingForTargetDamageAnimation = false;
        }

        // Plays the damage taken animation.
        private IEnumerator PlayDamageAnimation()
        {
            this._unitAnimator.SetTrigger("DamageTaken");

            // We know that the state machine will transition to the Attack state and then back to Idle, so we wait.
            while (!_unitAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                yield return null; // Wait a single frame and try again.
            }
        }
    }
}