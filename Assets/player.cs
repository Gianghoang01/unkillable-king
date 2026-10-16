using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class player : MonoBehaviour
{
    private TrailRenderer tr;
    public float moveSpeed;
    public Rigidbody2D rigidbody;
    public Camera cam;

    public Vector2 movement;
    public Vector2 mousePos;

    [Header("Cấu hình Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public bool isInvulnerable = false;

    private bool canDash = true;
    private bool isDashing = false;
    private Vector2 dashDir;

    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        if (tr != null) tr.emitting = false; // Mặc định tắt khi chưa dash
    }

    void Update()
    {
        if (isDashing) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Bấm Space hoặc chuột phải để Dash
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1)) && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rigidbody.MovePosition(rigidbody.position + dashDir * dashSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rigidbody.MovePosition(rigidbody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }

        Vector2 lookDir = mousePos - rigidbody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rigidbody.rotation = angle;
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;
        isInvulnerable = true;

        if (tr != null) tr.emitting = true;

        if (movement != Vector2.zero)
        {
            dashDir = movement.normalized;
        }
        else
        {
            dashDir = (mousePos - rigidbody.position).normalized;
        }

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        isInvulnerable = false;
        if (tr != null) tr.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}