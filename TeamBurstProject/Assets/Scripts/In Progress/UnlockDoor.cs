using System.Timers;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    [SerializeField] KeyCode useKey = KeyCode.X;
    [SerializeField] GameObject[] keyPositions;
    [SerializeField] GameObject keyPrefab;
    [SerializeField] GameObject doorHinge;
    [SerializeField] float openSpeed = 180;
    [SerializeField] bool openOutward;
    [SerializeField] AudioClip cantOpen;
    [SerializeField] AudioClip keyInserted; 
    [SerializeField] AudioClip open; 

    int nextLock;
    float rotated;
    bool inRange;
    bool opening;
    Quaternion origRot;
    Quaternion rotateDir;
    PlayerController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = gameManager.instance.player.GetComponent<PlayerController>();
        origRot = doorHinge.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange && Input.GetKeyDown(useKey))
        {
            if (controller.getNumOfKeys() > 0)
            {
                controller.pickupKey(-1);
                gameManager.instance.keysFor3KeyDoor -= 1;
                gameManager.instance.keysFor3KeyDoorText.text = gameManager.instance.keysFor3KeyDoor.ToString("F0");

                GameObject key = Instantiate(keyPrefab, keyPositions[nextLock].transform);
                key.transform.localPosition = Vector3.zero;
                //play lock click sound
                SoundManager.Instance.PlayEffect(keyInserted, 1);
                nextLock++;
                if(nextLock >= keyPositions.Length)
                {
                    gameManager.instance.tryDoorPopup.SetActive(false);
                    //Debug.Log("open");
                    //play open sound effect
                    SoundManager.Instance.PlayEffect(open, 1);
                    opening = true;
                }
            }
            else
            {
                gameManager.instance.tryDoorPopup.SetActive(true);
                gameManager.instance.keysFor3KeyDoorText.text = gameManager.instance.keysFor3KeyDoor.ToString("F0");
                //error sound effect plays
                SoundManager.Instance.PlayEffect(cantOpen, 1);
            }
        }

        if(opening)
        {
            openDoor();
        }
    }

    void openDoor()
    {
        if(openOutward)
        {
            rotateDir = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            rotateDir = Quaternion.Euler(0, -90, 0);
        }

        doorHinge.transform.rotation = Quaternion.RotateTowards(
            doorHinge.transform.rotation, doorHinge.transform.rotation *= rotateDir, openSpeed * Time.deltaTime);

        rotated += openSpeed * Time.deltaTime;
        if (rotated >= 90)
        {
            opening = false;
            doorHinge.transform.rotation = origRot *= rotateDir;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("in range");
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
