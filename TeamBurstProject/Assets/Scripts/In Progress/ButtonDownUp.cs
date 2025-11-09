using UnityEngine;
using System.Collections;

public class ButtonDownUp : MonoBehaviour
{
    public GameObject buttonModel;
    public float pressDistance = 0.1f;
    public float moveSpeed = 2f;
    [SerializeField] GameObject buttonResult;
    [SerializeField] AudioClip pressSound;

    private Vector3 startPosition;
    private Vector3 pressedPosition;
    private bool isPressed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = buttonModel.transform.position;
        pressedPosition = startPosition - new Vector3(0, pressDistance, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            SoundManager.Instance.PlayEffect(pressSound, 1);
            StartCoroutine(MoveButton(pressedPosition));
            if(buttonResult != null)
            {
                buttonResult.SetActive(true);
            }
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && isPressed)
        {
            isPressed = false;
            StartCoroutine(MoveButton(startPosition));
        }
    }

    IEnumerator MoveButton(Vector3 targetPosition)
    {
        while (Vector3.Distance(buttonModel.transform.position, targetPosition) > 0.01f)
        {
            buttonModel.transform.position = Vector3.MoveTowards(buttonModel.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        buttonModel.transform.position = targetPosition;
    }

}
