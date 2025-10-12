using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform shelfTilt;
    [SerializeField] Transform books;
    [SerializeField] float tippingSpeed;
    [SerializeField] float fallAngle;
    [SerializeField] float booksFallAngle;

    bool isTipping;
    bool isbooksFalling;
    float currentAngle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        isbooksFalling = false;

        foreach (Transform book in books)
        {
            Rigidbody rbBook = book.GetComponent<Rigidbody>();

            if (rbBook != null)
            {
                rbBook.isKinematic = true;
                rbBook.useGravity = false;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if(isTipping)
        {
            currentAngle = Mathf.MoveTowards(currentAngle, fallAngle, tippingSpeed * Time.deltaTime);
            shelfTilt.localRotation = Quaternion.Euler(currentAngle, 0, 0);

            if(! isbooksFalling && Mathf.Abs(currentAngle) >= booksFallAngle)
            {
                ReleaseBooks();
            }

            if(Mathf.Approximately(currentAngle, fallAngle))
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                isTipping = false;
            }
        }
    }

    public void TiltShelf(float angle)
    {
        if(!isTipping)
        {
            isTipping = true;
            fallAngle = angle;
        }
    }

    void ReleaseBooks()
    {
        isbooksFalling = true;
        Collider shelfCollider = rb.GetComponent<Collider>();
        
        foreach(Transform book in books)
        {
            Rigidbody rbBook = book.GetComponent<Rigidbody>();
            Collider bookCollider = book.GetComponent<Collider>();

            if(rbBook != null)
            {
                book.parent = null;

                rbBook.isKinematic =false;
                rbBook.useGravity = true;
            }

            if (shelfCollider != null && bookCollider != null)
                Physics.IgnoreCollision(bookCollider, shelfCollider);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TiltShelf(45);
        }
    }

}
