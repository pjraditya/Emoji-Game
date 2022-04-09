using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ImageCollider : MonoBehaviour
{
    public bool completed;
    public int totalItems;
    public List<GameObject> itemsList;
    public List<GameObject> dummyItemsList;
    public TextMeshProUGUI countText;
    public List<Vector2> anchorPts;

    private void Start()
    {
        countText.text = dummyItemsList.Count + "/" + totalItems / 2;
        
    }

    private void Update()
    {
        completed = CheckTile();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("1") || other.CompareTag("0"))
        {
            if (!itemsList.Contains(other.gameObject))
            {
                itemsList.Add(other.gameObject); 
                other.transform.localScale = new Vector3 (.6f, .6f,.6f);
                Debug.Log(itemsList.Count);
            }
        }

        CheckDummyList(false, other.gameObject);

        if (itemsList.Count < totalItems / 2)
            return;

        //Debug.Log(CheckTile() ? "Won" : "Lost");
        GameManager.instance.CheckLC();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("1") || other.CompareTag("0"))
        {
            CheckDummyList(true, other.gameObject);
            if (itemsList.Contains(other.gameObject))
                itemsList.Remove(other.gameObject);
        }
    }

    void CheckDummyList(bool remove, GameObject temp)
    {
        if (!CompareTag(temp.tag))
            return;

        if (remove)
        {
            if (dummyItemsList.Contains(temp))
            {
                dummyItemsList.Remove(temp);
                countText.text = dummyItemsList.Count + "/" + totalItems / 2;
            }
        }
        else
        {
            if (!dummyItemsList.Contains(temp))
            {
                dummyItemsList.Add(temp);
                countText.text = dummyItemsList.Count + "/" + totalItems / 2;
            }
        }
    }

    public bool CheckTile()
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            if (!CompareTag(itemsList[i].tag))
                return false;
        }

        return true;
    }

    public void ResetPos()
    {
        if(itemsList.Count<=0)
            return;
        
        for (int i = 0; i < anchorPts.Count; i++)
        {
            itemsList[i].transform.DOMove(anchorPts[i], .5f);
        }
    }
}