using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sumbarine_animate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator _animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.W))
        {
            if (_animator.GetCurrentAnimatorStateInfo (1).normalizedTime > 1 && !_animator.IsInTransition (1))
            {
                _animator.SetTrigger ("spin");
            }
        }
        if (Input.GetKey (KeyCode.S))
        {
            if (_animator.GetCurrentAnimatorStateInfo (1).normalizedTime > 1 && !_animator.IsInTransition (1))
            {
                _animator.SetTrigger ("spin_back");
            }
        }
        if (Input.GetKeyDown (KeyCode.A))
        {
            _animator.SetTrigger ("left");
        }
        if (Input.GetKeyUp (KeyCode.A))
        {
            _animator.SetTrigger ("left_back");
        }
        if (Input.GetKeyDown (KeyCode.D))
        {
            _animator.SetTrigger ("right");
        }
        if (Input.GetKeyUp (KeyCode.D))
        {
            _animator.SetTrigger ("right_back");
        }
        
    }
}
