using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Patrolling : MonoBehaviour
{

    public float detectionRadius = 5.0f;
    public LayerMask ghostLayer;
    public Transform ghostManager;

    public GameObject alertIconPrefab; 
    private List<AlertIcon> alertIcons = new List<AlertIcon>();
    public float alertIconHeightOffset = 4.0f;
    
    public Light spotlight;
    public float flickerIntensity = 5.0f; // Intensity for flickering
    public float flickerSpeed = 1.0f; // Speed of the flickering
    private bool isFlickering = false;
    private float initialIntensity;

    public GameObject copIconPrefab;
    private GameObject copIcon;
    public float copIconHeightOffset = 1.0f; 
    public float copIconHorizontalOffset = 1.0f;
    
    public List<Transform> waypoints; // List of waypoints the AI will patrol
    private int currentWaypointIndex = -1;
    private bool isPatrolling = false;

    public Transform[] points;
        private int destPoint = 0;
        private NavMeshAgent agent;


        void Start () {
            agent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;

            if (waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned to the AI agent.");
            enabled = false; // Disable the script if no waypoints are assigned.
            return;
        }

        // Randomly select the initial waypoint and direction.
        currentWaypointIndex = Random.Range(0, waypoints.Count);
        SetDestination(waypoints[currentWaypointIndex].position);

            foreach (Transform ghostAgent in ghostManager)
            {
                GameObject alertIconObject = Instantiate(alertIconPrefab, ghostAgent.position, Quaternion.identity);
                AlertIcon alertIcon = alertIconObject.GetComponent<AlertIcon>();
                alertIcon.Hide();
                alertIcons.Add(alertIcon);
            }

            initialIntensity = spotlight.intensity;

            Vector3 initialCopIconPosition = transform.position + Vector3.up * copIconHeightOffset + Vector3.right * copIconHorizontalOffset;

            copIcon = Instantiate(copIconPrefab, initialCopIconPosition, Quaternion.identity);
            copIcon.SetActive(false);
        }


        void SetDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
            isPatrolling = true;
        }

        void Update () {
            // Check if the AI has reached its current waypoint.
            if (isPatrolling && !agent.pathPending && agent.remainingDistance < 0.1f)
            {
            // Move to the next waypoint in a loop.
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            SetDestination(waypoints[currentWaypointIndex].position);
            }

            if (isFlickering)
            {
             ToggleSpotlightFlicker();
            }

            UpdateAlertIconPositions();

            if (copIcon != null)
            {
                Vector3 copIconPosition = transform.position + Vector3.up * copIconHeightOffset + Vector3.right * copIconHorizontalOffset;
                copIcon.transform.position = copIconPosition;
            }

            DetectGhosts();

        }

        private void DetectGhosts()
        {
        bool ghostsDetected = false;

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
                ghostsDetected = true;
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

        // Handle spotlight flickering based on ghost detection
        if (ghostsDetected)
        {
            ShowCopIcon();
            StartFlickering();
            
        }
        else
        {
            HideCopIcon();
            StopFlickering();
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

    // Start spotlight flickering
    private void StartFlickering()
    {
        isFlickering = true;
        InvokeRepeating("ToggleSpotlightFlicker", 0f, flickerSpeed);
    }

    // Stop spotlight flickering
    private void StopFlickering()
    {
        isFlickering = false;
        CancelInvoke("ToggleSpotlightFlicker");
        spotlight.intensity = initialIntensity; // Restore the initial intensity
    }

    // Toggle the spotlight intensity to create a flicker effect
    private void ToggleSpotlightFlicker()
    {
        if (spotlight.intensity == initialIntensity)
        {
            spotlight.intensity = flickerIntensity;
        }
        else
        {
            spotlight.intensity = initialIntensity;
        }
    }

    private void ShowCopIcon()
    {
        if (copIcon != null)
        {
            copIcon.SetActive(true);
        }
    }

    // Hide the cop icon
    private void HideCopIcon()
    {
        if (copIcon != null)
        {
            copIcon.SetActive(false);
        }
    }
}
