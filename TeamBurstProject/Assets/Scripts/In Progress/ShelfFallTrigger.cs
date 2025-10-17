using UnityEngine;

public class ShelfFallTrigger : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] FallingObject shelf;
    [SerializeField] float triggerDistance;

    bool hasTriggered;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTriggered)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= triggerDistance)
        {
            shelf.TiltShelf();
            hasTriggered = true;
        }
    }
}
