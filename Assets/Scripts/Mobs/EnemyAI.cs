using UnityEngine;
using UnityEngine.AI;
using EnemyUtils;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float walkingDistanceMax = 7f;
    [SerializeField] private float walkingDistanceMin = 3f;
    [SerializeField] private float walkingTimerMax = 2f;

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTimes = 0;
    private Vector3 roamPosition;
    private Vector3 startingPosition;


    private enum State
    {        
        Roaming
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = startingState;
    }
        
    private void Update()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                roamingTimes -= Time.deltaTime;
                if (roamingTimes < 0)
                {
                    Roaming();
                    roamingTimes = walkingTimerMax;
                }                                

                break;

        }
        

    }


    private void Roaming()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamPosition);
        ChangeFacingDirection(startingPosition, roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + MobsUtils.GetRandomDir() * UnityEngine.Random.Range(walkingDistanceMin, walkingDistanceMax);


    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }



    }


}
