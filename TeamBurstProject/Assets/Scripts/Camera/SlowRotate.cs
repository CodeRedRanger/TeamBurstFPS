using UnityEngine;

public class SlowRotate : MonoBehaviour
{
    [SerializeField] int speed;
   
    void Update()
    {
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, speed * Time.deltaTime, 0);
    }
}
