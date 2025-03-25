using UnityEngine;
using Zenject;
using Photon.Pun;

public class PlayerCamera : MonoBehaviourPun
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform weaponRoot;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float smoothTime = 0.08f;

    private float xRotation = 0f;

    private IPlayerInput _input;

    private Vector3 currentVelocity;

    [Inject]
    public void Construct(IPlayerInput input)
    {
        _input = input;
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            cameraRoot.gameObject.SetActive(false);
            return;
        }

        // Присоединяем Main Camera к cameraRoot
        Camera.main.transform.SetParent(cameraRoot);
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.localRotation = Quaternion.identity;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!photonView.IsMine || _input == null) return;

        float mouseX = _input.GetHorizontalInput() * sensitivity;
        float mouseY = _input.GetVerticalInput() * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Только cameraRoot отвечает за вращение
        var targetRotation = Quaternion.Euler(xRotation, cameraRoot.localEulerAngles.y + mouseX, 0f);
        cameraRoot.localRotation = targetRotation;

        SmoothFollowWeapon();
    }


    private void SmoothFollowWeapon()
    {
        if (weaponRoot == null) return;

        weaponRoot.rotation = Quaternion.Slerp(
            weaponRoot.rotation,
            cameraRoot.rotation,
            Time.deltaTime / smoothTime
        );

    }
}
