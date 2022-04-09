using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<ImageCollider> upDownBoxes;
    public bool levelCompleted;
    public List<GameObject> gameSprites;
     private void Awake()
    {
        instance = this;
        levelCompleted = false;
    }

    private void Update()
    {
        CheckLC();
    }

    public void CheckLC()
    {
       if(!ReturnTrue())
           return;
       if (levelCompleted)
           return;

       levelCompleted = true; 
       
     
       
       for (int i = 0; i < upDownBoxes.Count; i++)
       { 
           upDownBoxes[i].ResetPos();
       }
       print("Completed");
       UIManager.instance.WinPanel();
       

       StartCoroutine(TurnOffSprite());
      
    }

    bool ReturnTrue()
    {
        for (int i = 0; i < upDownBoxes.Count; i++)
        {
           if (!upDownBoxes[i].completed)
                return false;
        }

        return true;
    }

    IEnumerator TurnOffSprite()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < gameSprites.Count; i++)
        {
            gameSprites[i].SetActive(false);
        }
       
    }
}
