using UnityEngine;

public class pressspaceforthrow : MonoBehaviour
{
    public Animator animator;

   void Update()
{
   
    if (Input.GetKeyDown(KeyCode.Space))
    {
        animator.ResetTrigger("Lemparr");
        animator.SetTrigger("Lemparr");
    }
}
}