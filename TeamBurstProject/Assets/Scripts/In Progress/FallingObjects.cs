using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform shelfTilt;
    [SerializeField] Transform booksParent;
    [SerializeField] float tippingSpeed;
    [SerializeField] float fallAngle;
    [SerializeField] float booksFallAngle; // When the books should swap then fall out

    [SerializeField] GameObject bookPrefab;

    List<Rigidbody> groupedBooks = new List<Rigidbody>();


    bool isTipping;
    bool newBooksAdded;
    float currentAngle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if(booksParent != null)
        {
            Rigidbody[] propBooks = booksParent.GetComponentsInChildren<Rigidbody>();
            for(int i = 0; i < propBooks.Length; i++)
            {
                propBooks[i].isKinematic = true;
                propBooks[i].useGravity = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTipping)
        {
            currentAngle = Mathf.MoveTowards(currentAngle, fallAngle, tippingSpeed * Time.deltaTime);
            shelfTilt.localRotation = Quaternion.Euler(currentAngle, 0, 0);

            if (!newBooksAdded && Mathf.Abs(currentAngle) >= booksFallAngle)
            {
                ReplaceBooks();
            }

            if (Mathf.Approximately(currentAngle, fallAngle))
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

    void ReplaceBooks()
    {
        newBooksAdded = true;

        if(booksParent != null)
        {
            Destroy(booksParent.gameObject);
            booksParent = null;
        }

        GameObject group = Instantiate(bookPrefab, shelfTilt.position, shelfTilt.rotation, shelfTilt);

        Rigidbody[] bookRb = group.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < bookRb.Length; i++)
        {
            Rigidbody book = bookRb[i];
            book.isKinematic = false;
            book.useGravity = true;
        }
    }
}
