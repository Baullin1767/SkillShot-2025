using System;
using UniRx;
using UnityEngine;

public class ExitGameScript : MonoBehaviour
{
    private IDisposable stream;
    public static ExitGameScript Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        stream = Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(KeyCode.Escape))
        .Subscribe(_ => Application.Quit());
    }
    private void OnDestroy()
    {
        stream.Dispose();
    }
}
