using UnityEngine;

public interface IDamage
{
    void TakeDamage(int amount);

    void Heal(int amount);
}
