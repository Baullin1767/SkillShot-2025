using Photon.Pun;
using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

public class WeaponShooter : MonoBehaviourPun
{
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private float shootDistance = 100f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private Animator animator;
    [SerializeField] private float fireRate = 5;

    [SerializeField] private LineRenderer lineRenderer;

    [Inject]
    private IPlayerInput _input;

    [Inject]
    private ScoreCounter _scoreCounter;

    private IDisposable _shootStream;

    private void Start()
    {
        _shootStream = Observable.EveryUpdate()
        .Where(_ => photonView.IsMine && _input.IsFirePressed())
        .ThrottleFirst(TimeSpan.FromSeconds(fireRate))
        .Subscribe(_ => Fire());        
    }
    private void OnDestroy()
    {
        _shootStream.Dispose();
    }

    void Update()
    {
        if (!photonView.IsMine || _input == null) return;

        UpdateRay();
    }

    void UpdateRay()
    {
        Vector3 start = shootOrigin.position;
        Vector3 end = start + shootOrigin.right * 100f;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
    void Fire()
    {
        muzzleFlash?.Play();
        shootSound?.Play();
        animator.SetTrigger("Shoot");

        if (Physics.Raycast(shootOrigin.position, shootOrigin.right, out RaycastHit hit, shootDistance, hitMask))
        {
            Debug.DrawRay(shootOrigin.position, shootOrigin.right * shootDistance, Color.green);
            IDamageable target = hit.collider.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                if (hit.collider.name.Contains("head"))
                {
                    target.TakeHeadShoot();
                    _scoreCounter.AddPoints(5);
                }
                else
                {
                    target.TakeShoot();
                    _scoreCounter.AddPoints(1);
                }
            }
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        else
        {
            Debug.DrawRay(shootOrigin.position, shootOrigin.right * shootDistance, Color.red);
        }
    }
}
