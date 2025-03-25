using UnityEngine;
using Zenject;
using Photon.Pun;
using Unity.VisualScripting;

public class PlayerCamera : MonoBehaviourPun
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform weaponRoot;
    [SerializeField] private LayerMask obstacleLayerMask;
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

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            cameraRoot.gameObject.SetActive(false);
            return;
        }

        // Присоединяем Main Camera к cameraRoot
        Camera.main.transform.SetParent(cameraRoot);

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
        Ray ray = new Ray(cameraRoot.position, cameraRoot.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, obstacleLayerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * 2f, Color.green);
            float distance = hit.distance;
            float pushBack = Mathf.Lerp(1f, 0.5f, 1f + (distance / 1.5f));
            weaponRoot.localPosition = new Vector3(0f, 0f, -pushBack);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red);
            weaponRoot.localPosition = Vector3.Lerp(weaponRoot.localPosition, Vector3.zero, Time.deltaTime * 10f);
        }
    }
}
