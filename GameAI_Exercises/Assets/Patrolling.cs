using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Patrolling : MonoBehaviour
{

    public float detectionRadius = 5.0f;
    public LayerMask ghostLayer;
    public GameObject alertIconPrefab; 
    private List<AlertIcon> alertIcons = new List<AlertIcon>();
    public Transform ghostManager;
    public float alertIconHeightOffset = 4.0f;

    public Transform[] points;
        private int destPoint = 0;
        private NavMeshAgent agent;


        void Start () {
            agent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;

            foreach (Transform ghostAgent in ghostManager)
        {
            GameObject alertIconObject = Instantiate(alertIconPrefab, ghostAgent.position, Quaternion.identity);
            AlertIcon alertIcon = alertIconObject.GetComponent<AlertIcon>();
            alertIcon.Hide();
            alertIcons.Add(alertIcon);
        }
        }


        void GotoNextPoint() {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;
        }


        void Update () {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (!agent.pathPending && agent.remainingDistance < 0.5f){
                GotoNextPoint();
            }
            UpdateAlertIconPositions();
            DetectGhosts();

        }

        private void DetectGhosts()
    {
        foreach (Transform ghostAgent in ghostManager)
        {
            float distanceToGhost = Vector3.Distance(transform.position, ghostAgent.position);

            if (distanceToGhost <= detectionRadius)
            {
                // A ghost is detected, show the corresponding alert icon
                int ghostIndex = GetChildIndex(ghostManager, ghostAgent);
                if (ghostIndex >= 0 && ghostIndex < alertIcons.Count)
                {
                    alertIcons[ghostIndex].Show();
                }
            }
            else
            {
                // No ghosts detected, hide the corresponding alert icon
                int ghostIndex = GetChildIndex(ghostManager, ghostAgent);
                if (ghostIndex >= 0 && ghostIndex < alertIcons.Count)
                {
                    alertIcons[ghostIndex].Hide();
                }
            }
        }
    }

    private void UpdateAlertIconPositions()
    {
        for (int i = 0; i < alertIcons.Count; i++)
        {
            alertIcons[i].transform.position = ghostManager.GetChild(i).position + Vector3.up * alertIconHeightOffset;
        }
    }

    // Get the index of a child within a Transform component
    private int GetChildIndex(Transform parent, Transform child)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i) == child)
            {
                return i;
            }
        }
        return -1; // Child not found
    }
}
