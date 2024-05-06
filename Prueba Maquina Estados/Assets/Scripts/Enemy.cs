using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Vector3 min, max;
    Vector3 destination;
    //bool playerDetected = false;
    public float playerDetectionDistance, playerAttackDistance;
    Transform player;
    public float visionAngle;

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        RandomDestination();
        StartCoroutine("Patrol");
        StartCoroutine("Alert");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Alert()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, player.position) < playerDetectionDistance)
            {
                Vector3 vectorPlayer = player.position - transform.position;
                if (Vector3.Angle(vectorPlayer.normalized, transform.forward) < visionAngle)
                {
                    StopCoroutine("Patrol");
                    //transform.LookAt(player.transform);
                    navMeshAgent.SetDestination(player.position);
                }
                else
                {
                    StartCoroutine("Patrol");
                }
            }
            else
            {
                StartCoroutine("Patrol");
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, destination) < 1.5f)
            {
                animator.SetFloat("velocity", 0);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                RandomDestination();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            navMeshAgent.SetDestination(player.position);

            if(Vector3.Distance(transform.position, player.position) < playerAttackDistance)
            {

            }


            yield return new WaitForEndOfFrame();
        }
    }
    public void RandomDestination()
    {
        destination = new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.z, max.z));
        navMeshAgent.SetDestination(destination);
        animator.SetFloat("velocity", 2);
    }

    #region Deteccion por trigger
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           //playerDetected = true;
            StopCoroutine("Patrol");
            transform.LookAt(other.transform);
            navMeshAgent.SetDestination(other.transform.position);
            Debug.Log("personaje detectado");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           //playerDetected = false;
            Debug.Log("personaje fuera de la detección");
        }
    }*/
    #endregion
}
