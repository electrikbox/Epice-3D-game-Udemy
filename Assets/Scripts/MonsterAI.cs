using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [Range(0.5f, 50)]
    public float detectDistance = 4;
    public bool isRuning = false;
    public static MonsterAI ai;
    public List<Transform> points = new List<Transform>();
    public NavMeshAgent agent;

    GameObject[] navPoints;
    Transform player;
    Animator anim;

    int index = 0;



    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        navPoints = GameObject.FindGameObjectsWithTag("navMeshPoint");
        index = Random.Range(0, points.Count);
        agent = GetComponent<NavMeshAgent>();
        ai = this;

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
            isRuning = true;
            agent.destination = player.position;
            agent.speed = 3;
        }
        else
        {
            isRuning = false;
            agent.destination = points[index].position;
            agent.speed = 1;
        }
    }



    private void Update()
    {
        anim.SetBool("isRuning", isRuning);
        Walk();
        SearchPlayer();
    }
}
