using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDownCollider : MonoBehaviour
{
    public List<GameObject> downList;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("1") || other.gameObject.CompareTag("0"))
        {
            downList.Add(other.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("1") || other.gameObject.CompareTag("0"))
        {
            downList.Remove(other.gameObject);
        }
    }
}
