using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    float detectDistance = 6;

    Transform player;
    GameObject[] navPoints;
    
    public List<Transform> points = new List<Transform>();
    public NavMeshAgent agent;
    
    int index = 0;

    private void Start()
    {
        navPoints = GameObject.FindGameObjectsWithTag("bossNav");
        index = Random.Range(0, points.Count);
        agent = GetComponent<NavMeshAgent>();

        foreach(GameObject navPoint in navPoints)
            points.Add(navPoint.transform);

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if(agent != null)
            agent.destination = points[index].position;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectDistance);
    }



    public void Walk()
    {
        float dist = agent.remainingDistance;

        if(dist <= 0.05f)
        {
            index++;
            if(index > points.Count - 1)
                index = 0;
            
            agent.destination = points[index].position;
        }
    }



    public void SearchPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceToPlayer <= detectDistance)
        {
            agent.destination = player.position;
            agent.speed = 6;
        }
    }



    private void FixedUpdate() => Walk();
}
