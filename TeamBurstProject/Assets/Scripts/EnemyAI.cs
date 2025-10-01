using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    [SerializeField] int HP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

    }
}
