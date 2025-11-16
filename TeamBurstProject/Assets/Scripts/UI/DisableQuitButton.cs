using UnityEngine;

public class DisableQuitButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Awake ()
    {
       CheckForWebMode(); 
    }

    private void CheckForWebMode()
    {
#if UNITY_WEBGL
      
    gameObject.SetActive (false);

#endif

    }
}
