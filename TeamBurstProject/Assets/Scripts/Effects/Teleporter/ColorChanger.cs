using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Renderer objRenderer;
    private bool enemiesSpawned = false;
    private int requiredGoalCount = 0;
    //public AudioClip GetToSchool;
   //bool hasPlayedSound = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        StartCoroutine(WaitForSpawn());
    }

    // Update is called once per frame

    void Update()
    {
        if (enemiesSpawned == false)
        {
            StartCoroutine(WaitForSpawn());
            enemiesSpawned = true;
        }

        if(gameManager.instance.GetGameGoalCount() <= requiredGoalCount)
        {
            objRenderer.material.color = Color.yellow;
            /*
            if (hasPlayedSound == false)
            {
                SoundManager.Instance.PlayEffect(GetToSchool, 1.0f);
                hasPlayedSound = true;
            }*/
         
        }
        else
        {
            objRenderer.material.color = Color.black; 
        }
      
    }

    IEnumerator WaitForSpawn()
    {
        
        yield return new WaitForSeconds(10.0f);
       
    }

}
