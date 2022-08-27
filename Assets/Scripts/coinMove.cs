using UnityEngine;

public class coinMove : MonoBehaviour
{
    float detectDistance = 1.8f;
    float distanceToPlayer;
    GameObject player;
    Vector3 coinPos, playerPos;



    private void Start() => player = GameObject.FindGameObjectWithTag("Player");


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.down, detectDistance);
    }



    private void FixedUpdate()
    {
        coinPos = gameObject.transform.position;
        playerPos = player.transform.position + Vector3.up;
        distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        if(distanceToPlayer < detectDistance)
            transform.position = Vector3.Lerp(coinPos, playerPos, 15f * Time.fixedDeltaTime);
    }
}
