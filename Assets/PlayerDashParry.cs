using System.Collections;
using UnityEngine;

public class PlayerDashParry : MonoBehaviour
{
    public float dashSpeed = 25f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    public float parryPushForce = 10f;
    public float parryJumpBoost = 10f;

    private CharacterController controller;
    private bool isDashing = false;
    private bool canDash = true;
    private Vector3 dashDirection;

    // Referencia al controlador de movimiento real
    private EasyPeasyFirstPersonController.FirstPersonController fpc;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        fpc = GetComponent<EasyPeasyFirstPersonController.FirstPersonController>();
    }

    void Update()
    {
        // Activar Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            StartCoroutine(DoDash());
    }

    private IEnumerator DoDash()
    {
        canDash = false;
        isDashing = true;

        dashDirection = transform.forward;

        float t = 0;
        while (t < dashDuration)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isDashing && hit.collider.CompareTag("ParryObject"))
        {
            // Empujar objeto
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
                rb.AddForce(transform.forward * parryPushForce, ForceMode.Impulse);

            // Impulso de salto usando el controlador real
            if (fpc != null)
            {
                fpc.SetMoveControl(true);
                // Accedemos a moveDirection del FPC mediante método público
                var jumpBoost = parryJumpBoost;

                // Método para aplicarle un impulso vertical al FPS Controller
                typeof(EasyPeasyFirstPersonController.FirstPersonController)
                    .GetField("moveDirection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(fpc, new Vector3(0, jumpBoost, 0));
            }

            isDashing = false;
        }
    }
}
