using UnityEngine;

public class SpeedZone : MonoBehaviour
{
    [SerializeField] int boostAmount;
    [SerializeField] bool lockCamera = true;

    private Transform player;
    private Camera mainCam;

    private MonoBehaviour cameraScript;


    void Awake()
    {
        if(player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if(foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }

        if(mainCam == null)
        {
            mainCam = Camera.main;
        }
        if(mainCam != null && cameraScript == null)
        {
            cameraScript = mainCam.GetComponent<MonoBehaviour>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        gameManager.instance.playerScript.SpeedBoost(boostAmount);

        if(lockCamera && cameraScript != null )
        {
            cameraScript.enabled = false;
        }

      
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        gameManager.instance.playerScript.SpeedBoost(-boostAmount);

        if (lockCamera && cameraScript != null )
        {
            cameraScript.enabled = true;
        }
    }
}
