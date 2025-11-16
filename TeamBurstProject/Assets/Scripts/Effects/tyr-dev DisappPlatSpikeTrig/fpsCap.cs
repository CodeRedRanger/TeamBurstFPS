using UnityEngine;

[ExecuteInEditMode]

public class fpsCap : MonoBehaviour
{
    [SerializeField] private int frameRate = 60;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;

#else
    //added to eliminate warning for unused variable. 
    frameRate = 60; 

#endif
    }
}
