using System.Collections.Generic;
using UnityEngine;

public class Boss3WallTrigger : MonoBehaviour
{
    public List<Collider> WallColliderList;
    public List<GameObject> WallList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(var wall in WallList)
            {
                wall.GetComponent<BoxCollider>().enabled = false;
            }
            foreach(var wallCollider in WallColliderList)
            {
                wallCollider.enabled = true;
            }
        }
    }
}
