using UnityEngine;

public class TimedSpikes : MonoBehaviour
{
    [SerializeField] float downDuration = 2f;
    [SerializeField] float upDuration = 3f;
    [SerializeField] float moveDownTime = 0.8f;
    [SerializeField] float moveUpTime = 0.1f;
    [SerializeField] float height = 2.8f;

    float timer;
    float t;
    bool isUp;
    bool moving;
    Vector3 origPos;
    Vector3 newPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (!isUp && timer >= downDuration && !moving)
        {
            origPos = transform.position;
            newPos = origPos + new Vector3(0, height, 0);
            moving = true;
        }
        else if (isUp && timer >= upDuration && !moving)
        {
            origPos = transform.position;
            newPos = origPos + new Vector3(0, -height, 0);
            moving = true;
        }

        if (moving)
        {
            move();
        }
    }

    void move()
    {
        if (!isUp)
        {
            t += Time.deltaTime / moveUpTime;
            transform.localPosition = Vector3.Lerp(origPos, newPos, t);
        }
        else
        {
            t += Time.deltaTime / moveDownTime;
            transform.localPosition = Vector3.Lerp(origPos, newPos, t);
        }

        if(t >= 1f)
        {
            moving = false;
            isUp = !isUp;
            timer = 0f;
            t = 0f;
        }
    }
}
