using System.Collections;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamageable
{
    public int health = 10;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Collider dummyCollider;
    [SerializeField]
    Collider headCollider;
    Coroutine coroutine;
    public void TakeShoot()
    {
        Debug.Log("Bull's eye!");
        if (health > 0)
        {
            animator.SetTrigger("Hit");
            health--;
        }
        else
        {
            animator.SetBool("Die", true);
            dummyCollider.enabled = false;
            headCollider.enabled = false;
            if(coroutine == null)
                coroutine = StartCoroutine(ReturnToAlive());
        }
    }
    public void TakeHeadShoot()
    {
        TakeShoot();

        health -= 4;
    }
    IEnumerator ReturnToAlive()
    {
        yield return new WaitForSeconds(10f);
        health = 10;
        animator.SetBool("Die", false);
        dummyCollider.enabled = true;
        headCollider.enabled = true;
    }
}
