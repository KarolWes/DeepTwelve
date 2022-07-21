using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGeneral : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator animator;

    protected void Swim() {
        if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime > 1 && !animator.IsInTransition (0))
        {
            animator.SetTrigger ("swim");
        }
    }
}
