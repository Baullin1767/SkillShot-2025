using System;
using System.Collections;
using UniRx;
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
    private IDisposable _takeShootStream;

    private void Start()
    {
        _takeShootStream = Observable.EveryUpdate()
        .Where(_ => health <= 0)
        .Subscribe(_ => {
            animator.SetBool("Die", true);
            dummyCollider.enabled = false;
            headCollider.enabled = false;
            if (coroutine == null)
                coroutine = StartCoroutine(ReturnToAlive());
        });
    }

    private void OnDestroy()
    {
        _takeShootStream?.Dispose();
    }

    public void TakeShoot()
    {
        Debug.Log("Bull's eye!");
        if (!animator.GetBool("Die"))
        {
            animator.SetTrigger("Hit");
            health--;
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
