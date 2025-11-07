using UnityEngine;
using UnityEngine.Events;

public class UnityEventsHandler : MonoBehaviour
{
    [SerializeField] UnityEvent[] events;

    public void CallEvent(int _eventIndex)
    {
        events[_eventIndex].Invoke();
    }
}
