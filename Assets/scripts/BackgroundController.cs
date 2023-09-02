using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void AnimateBackground()
    {
        animator.SetTrigger("random");
    }
}
