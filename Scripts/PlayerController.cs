using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    Timer timer;
    public Camera cam;
    public NavMeshAgent agent;

    public GameObject[] waypoints;
    private int waypointInd;
    float speed;


    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        FieldOfView fov = GetComponent<FieldOfView>();

        speed = agent.velocity.magnitude;

        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        waypointInd = Random.Range(0, waypoints.Length);

        if (fov.visible)
        {
            Vector3 targetLocation = new Vector3(fov.visibleTargets[0].position.x, transform.position.y, fov.visibleTargets[0].position.z);
            agent.SetDestination(targetLocation);
        }
        else
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            waypointInd = Random.Range(0, waypoints.Length);

            if (speed < 0.1)
            {
                agent.SetDestination(waypoints[waypointInd].transform.position);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Evader")
        {
            Debug.Log("Evader collider hit.");
            timer.Finish();
        }
    }
}
