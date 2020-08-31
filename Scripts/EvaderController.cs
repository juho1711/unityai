using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvaderController : MonoBehaviour
{
    public float wanderRadius = 7f;
    public float hideRadius = 15f;
    float speed;

    private Vector3 wanderPoint;

    public GameObject player;
    public NavMeshAgent evader;

    private float timeoutTimer = 0;

    //Renderer rend;


    // Start is called before the first frame update
    void Start()
    {
        wanderPoint = RandomWanderPoint();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 raycastDir2 = player.transform.position - transform.position;
        //Debug.DrawRay(transform.position, raycastDir2, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, raycastDir2, out hit, 10f))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                //rend.material.color = Color.magenta;
                speed = evader.velocity.magnitude;
                evader.speed = 5f;
                if (speed < 0.1f)
                {
                    evader.SetDestination(Hide());
                }
            }
            else
            {
                //rend.material.color = Color.blue;
                evader.speed = 3.5f;
                Wander();
            }
        }

        timeoutTimer++;
    }

    public Vector3 Hide()
    {
        Vector3 hidingSpot = transform.position;
        Vector3 hidePoint;
        Vector3 initialHidePoint = (Random.insideUnitSphere * hideRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(initialHidePoint, out navHit, hideRadius, -1);

        hidePoint = new Vector3(navHit.position.x, transform.position.y, navHit.position.z);

        Vector3 raycastDir = player.transform.position - hidePoint;
        //Debug.DrawRay(hidePoint, raycastDir, Color.red, 5f);

        RaycastHit hit;
        if (Physics.Raycast(hidePoint, raycastDir, out hit))
        {
            if (hit.collider.tag.Equals("Player"))
            {
                hidingSpot = transform.position;
            }
            else
            {
                hidingSpot = hidePoint;
            }
        }
        return hidingSpot;
    }

    public void Wander()
    {
        if (Vector3.Distance(transform.position, wanderPoint) < 2f)
        {
            wanderPoint = RandomWanderPoint();
            timeoutTimer = 0;
        }
        else
        {
            if (timeoutTimer < 250)
            {
                evader.SetDestination(wanderPoint);
            }
            else
            {
                wanderPoint = RandomWanderPoint();
                timeoutTimer = 0;
            }

        }
    }

    public Vector3 RandomWanderPoint()
    {
        Vector3 wanderPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(wanderPoint, out navHit, wanderRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }
}
