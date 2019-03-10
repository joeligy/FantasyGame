// LocomotionSimpleAgent.cs
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionAgent : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;

    Vector2 velocity = Vector2.zero;
    float locomotionTargetSpeed;

    [SerializeField] float maxLocomotionDelta = .2f;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
    }

    void Update()
    {
        print(anim.GetFloat("Locomotion"));
        CalculatVelocity();
        SetLocomotion();
        //LookAtPlayerIfClose();

        //GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }

    private void SetLocomotion()
    {
        SetTargetSpeed();

        UpdateLocomotionTowardsTarget();

        anim.SetFloat("Locomotion", velocity.magnitude);
    }

    private void CalculatVelocity()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;
    }

    private void SetTargetSpeed()
    {
        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.stoppingDistance;
        if (shouldMove)
        {
            locomotionTargetSpeed = velocity.magnitude;
        }
        else
        {
            locomotionTargetSpeed = 0f;
        }
    }

    private void UpdateLocomotionTowardsTarget()
    {
        float currentLocomotionSpeed = anim.GetFloat("Locomotion");
        if (Mathf.Abs(locomotionTargetSpeed - currentLocomotionSpeed) < maxLocomotionDelta)
        {
            anim.SetFloat("Locomotion", locomotionTargetSpeed);
        }
        else
        {
            if (locomotionTargetSpeed > currentLocomotionSpeed)
            {
                anim.SetFloat("Locomotion", currentLocomotionSpeed + maxLocomotionDelta);
            }
            else
            {
                anim.SetFloat("Locomotion", currentLocomotionSpeed - maxLocomotionDelta);
            }
        }
    }

    private void LookAtPlayerIfClose()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            transform.LookAt(Camera.main.transform);
        }
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }
}