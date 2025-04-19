using UnityEngine;

public class PressSpaceThrow : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spasi ditekan: Lempar = true");
            animator.SetBool("Lempar", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Spasi dilepas: Lempar = false");
            animator.SetBool("Lempar", false);
        }
    }
}
