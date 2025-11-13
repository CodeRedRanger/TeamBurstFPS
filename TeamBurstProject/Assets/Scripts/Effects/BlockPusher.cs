using UnityEngine;

public class BlockPusher : MonoBehaviour
{
    public LayerMask layerTopush;
    public bool canPush;
    [Range(0.5f, 5f)] public float power = 2.0f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (canPush) PushObject(hit);
    }

    private void PushObject(ControllerColliderHit hit)
    {


        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;

        var bodyLayerMask = 1 << body.gameObject.layer;
        if ((bodyLayerMask & layerTopush.value) == 0) return;

        if (hit.moveDirection.y < -0.3f) return;

        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

        body.AddForce(pushDirection * power, ForceMode.Impulse);
    }
}
