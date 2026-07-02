using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private Vector3 randomPos;

    private GameObject target;
    private NavMeshAgent agent;

    public AudioSource groanSFX;
    private Animator anim;

    private bool isRunning;
    private bool isWalking;
    private void Start()
    {
        randomPos = transform.position;
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (anim == null) anim = GetComponentInChildren<Animator>();

        WalkRandomly();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position,target.transform.position)<=5.0f)
        {
            ChasePlayer();
        }
        else if(isRunning)
        {
            WalkRandomly();
        }
        else if(isWalking)
        {
            if(Vector3.Distance(transform.position, randomPos) <= 2.0f)
            {
                WalkRandomly();
            }
        }
        if(Vector3.Distance(transform.position, target.transform.position) <= 1.0f)
        {
            anim.SetTrigger("attack");
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(target.transform.position);

        if(!isRunning)
        {
            groanSFX.loop = true;
            groanSFX.Play();
            isRunning = true;
            isWalking = false;
            agent.speed = 3.5f;
            anim.SetBool("isRunning",isRunning);
            anim.SetBool("isWalking",isWalking);
        }
    }

    private void WalkRandomly()
    {
        agent.speed = 0.65f;
        randomPos = MapManager.instance.GetRandomPos();
        agent.SetDestination(randomPos);

        groanSFX.Stop();

        isRunning = false;
        isWalking = true;

        anim.SetBool("isRunning",isRunning);
        anim.SetBool("isWalking",isWalking);
    }

}
