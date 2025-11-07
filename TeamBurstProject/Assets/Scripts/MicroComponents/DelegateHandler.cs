using UnityEngine;

public class DelegateHandler : MonoBehaviour
{
    delegate void DelegateMethod();
    [SerializeField] DelegateMethod[] methods;

    public void CallMethod(int _eventIndex)
    {
        methods[_eventIndex].Invoke();
    }
}
