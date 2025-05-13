using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Monster_Move : MonoBehaviour
{
    public List<Transform> movePoints;
    public int nextPoint = 0;

    public NavMeshAgent Monster_Agent;
    public Animator animator;
    public Vector3 targetPosition;

    public Transform heroTr;
    private WaitForSeconds wfs;
    public bool isPatrolling;

    void OnEnable()
    {
        StartCoroutine(CheckMonster());
        Debug.Log("OnEnable");
    }

    IEnumerator CheckMonster()
    {
        while (true)
        {
            yield return wfs;

            float distance = Vector3.Distance(transform.position, heroTr.position);

            if (distance <= 2.0f)
            {
                // 가까이 있으면 멈춤
                Monster_Agent.speed = 0.1f;
                Monster_Agent.autoBraking = false;
            }
            else if (distance > 2.0f && distance <= 8.0f)
            {
                // 추적 모드
                Monster_Agent.autoBraking = false;
                Monster_Agent.speed = 1.0f;
                isPatrolling = false;

                ApproachTarget(heroTr.position);
            }
            else
            {
                // 순찰 모드
                Monster_Agent.speed = 0.6f;
                isPatrolling = true;
                Monster_Agent.destination = movePoints[nextPoint].position;
            }
        }
    }

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        heroTr = player.GetComponent<Transform>();
        wfs = new WaitForSeconds(0.4f);
    }

    void Start()
    {
        Monster_Agent = GetComponent<NavMeshAgent>();
        Monster_Agent.autoBraking = false;

        animator = GetComponent<Animator>();

        var p_group = GameObject.Find("EnemyMovePos");
        p_group.GetComponentsInChildren<Transform>(movePoints);
        movePoints.RemoveAt(0); // 부모 제거

        isPatrolling = true;
        MoveMonster();

        Monster_Agent.speed = 1.0f;
    }

    void MoveMonster()
    {
        if (isPatrolling)
        {
            Monster_Agent.destination = movePoints[nextPoint].position;
            Monster_Agent.isStopped = false;
        }
    }

    void ApproachTarget(Vector3 pos)
    {
        if (Monster_Agent.isPathStale) return;
        Monster_Agent.isStopped = false;
        Monster_Agent.destination = pos;
    }

    void Update()
    {
        if (!isPatrolling) return;

        if (Monster_Agent.remainingDistance <= 0.5f)
        {
            nextPoint = ++nextPoint % movePoints.Count;
            MoveMonster();
        }
    }
}