using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingAI : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float minWanderTimer = 3f;
    public float maxWanderTimer = 7f;
    public float detectionRadius = 5f;
    public Transform targetAgent; // The agent to follow
    public float runSpeed = 10f;
    public float walkSpeed = 3.5f;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = Random.Range(minWanderTimer, maxWanderTimer);
    }

    void Update()
    {
        if (targetAgent != null)
        {
            float distanceToAgent = Vector3.Distance(transform.position, targetAgent.position);

            if (distanceToAgent <= detectionRadius)
            {
                // The agent is within the detection radius, so follow it.
                agent.speed = runSpeed;
                agent.SetDestination(targetAgent.position);
                return; // Don't perform wandering behavior when following.
            }
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Generate a random position within a circle on the XZ plane
            Vector3 randomDirection = Random.insideUnitCircle.normalized * wanderRadius;
            Vector3 newPos = new Vector3(randomDirection.x, 0f, randomDirection.y) + transform.position;

            agent.speed = walkSpeed; // Switch back to walking speed for wandering.
            agent.SetDestination(newPos);
            timer = Random.Range(minWanderTimer, maxWanderTimer);
        }
    }
}
