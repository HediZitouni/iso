using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectPos : MonoBehaviour
{
    [SerializeField]
    private InputAction mouseClickAction;
    [SerializeField]
    private float playerSpeed = 10f;
    private Camera mainCamera;
    private Coroutine coroutine;
    private Vector3 targetPositon;

    private void Awake() {
        mainCamera = Camera.main;
    }

   private void OnEnable() {
    mouseClickAction.Enable();
    mouseClickAction.performed += Move;
   }

   private void OnDisable() {
    mouseClickAction.performed -= Move;
    mouseClickAction.Disable();
   }

   private void Move(InputAction.CallbackContext context) {
    Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
    if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider) {
        if (coroutine != null) StopCoroutine(coroutine);
        targetPositon = RoundVector3(hit.point + new Vector3(0,5,0));
        coroutine = StartCoroutine(PlayerMove(targetPositon));
    }
   }

   private IEnumerator PlayerMove(Vector3 target) {
    while (Vector3.Distance(transform.position, target) > 0.1f) {
        Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
        transform.position = destination;
        yield return null;
    }
   }

   private void OnDrawGizmos() {
    Gizmos.color = Color.red;
    Gizmos.DrawSphere(targetPositon, 1);
   }

   private Vector3 RoundVector3(Vector3 v) {
    Vector3 roundedV = new Vector3();
    roundedV.x = Mathf.Round(v.x / 10) * 10;
    roundedV.z = Mathf.Round(v.z / 10) * 10;
    roundedV.y = v.y;
    Debug.Log(roundedV);
    return roundedV;
   }
}
