using System.Collections;
using UnityEngine;

public class StaticRotationTurret_State : FiniteState
{
    [SerializeField] FiniteState shootState;
    [SerializeField] GameObject objectToRotate;
    [SerializeField] float rotationSpeed;
    [SerializeField] Vector3 minRotation, maxRotation;
    [Tooltip("The time that passes before the turret turns the other direction after reaching max or min rotation")]
    [SerializeField] float pauseDuration;
    bool rotationPaused = false;
    float rotationProgress; // between 0 and 1. Closer to 0 = closer to min, closer to 1 = closer to max
    int currentRotationDirection = 1; // +1 = towardMax, -1 = towardMin

    public override void OnUpdate()
    {
        base.OnUpdate();

        Rotate(); // DELETE LATER. JUST FOR STATE MACHINE TESTING
    }

    private void Rotate() // DELETE LATER. JUST FOR STATE MACHINE TESTING
    {
        if (rotationPaused) return;
        rotationProgress = Mathf.Clamp(rotationProgress + rotationSpeed * currentRotationDirection * Time.deltaTime, 0, 1);
        objectToRotate.transform.localEulerAngles = Vector3.Lerp(minRotation, maxRotation, rotationProgress);
        if((rotationProgress == 1 && currentRotationDirection == 1) || (rotationProgress == 0 && currentRotationDirection == -1))
        {
            currentRotationDirection = -currentRotationDirection;
            StartCoroutine(PauseCoroutine());
        }
    }

    IEnumerator PauseCoroutine()
    {
        rotationPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        rotationPaused = false;
    }
}
