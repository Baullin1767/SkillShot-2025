using UnityEngine;

public class PlayerInputPC : IPlayerInput
{
    public float GetHorizontalInput() => Input.GetAxis("Mouse X");
    public float GetVerticalInput() => Input.GetAxis("Mouse Y");
    public bool IsFirePressed() => Input.GetButtonDown("Fire1");
}
