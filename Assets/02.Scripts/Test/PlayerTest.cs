using UnityEngine;
using UnityEngine.AI;

public class PlayerTest : MonoBehaviour
{
    public CharacterController CharacterController;
    public NavMeshAgent Agent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);

            if(hitInfo.collider != null)
            {
                Agent.SetDestination(hitInfo.point);
            }
        }   
    }

}
