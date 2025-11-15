using UnityEngine;

public class MagneticChunk : MonoBehaviour
{
    [SerializeField] float moveAmplitude = 0.05f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] Vector3 moveAxis = Vector3.up;

    [SerializeField] float rotAmplitude = 2f;
    [SerializeField] float rotSpeed = 1f;
    [SerializeField] Vector3 rotAxis = Vector3.up;

    Vector3 startLocalPos;
    Quaternion startLocalRot;
    float t;
    float m;
    float r;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startLocalPos = transform.localPosition;
        startLocalRot = transform.localRotation;

        //offset
        t = Random.value * Mathf.PI * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
        t += Time.deltaTime;

        if (moveAmplitude > 0f && moveAxis != Vector3.zero)
        {
            m = Mathf.Sin(t * moveSpeed) * moveAmplitude;
            transform.localPosition = startLocalPos + moveAxis.normalized * m;
        }

        if (rotAmplitude > 0f && rotAxis != Vector3.zero)
        {
            r = Mathf.Sin(t * rotSpeed) * rotAmplitude;
            transform.localRotation = startLocalRot * Quaternion.AngleAxis(r, rotAxis.normalized);
        }
    }
}
