using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float MoveSpeed;
    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public bool IsEntityInRange;
    public DetectionRange DetectionRange;

    public Action<int> OnStageChange;

    [HideInInspector]
    public bool IsWalking;
    [HideInInspector]
    public bool IsIdling;
    [HideInInspector]
    public bool IsAttacking;
    [HideInInspector]
    public bool IsDead;

    private bool isSpeed;
    void Awake()
    {
        DetectionRange.OnInRange += SetEntityInRange;
        DetectionRange.OnOutRange += SetEntityOutRange;

        SetStage(2);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // MoveSpeed = enemyData.speed;
    }

    void Update()
    {
        // SetStage(1);
        // if (Target != null && IsEntityInRange == true)
        // {
        //     Vector3 directionToPlayer = (Target.position - transform.position).normalized;
        //     transform.parent.position += directionToPlayer * MoveSpeed * Time.deltaTime;
        //     SetStage(2);
        // }
        // else
        // {
        //     transform.parent.position += Vector3.left * MoveSpeed * Time.deltaTime;
        //     SetStage(2);
        // }
    }

    public void Walk()
    {
        Vector3 directionToPlayer = (Target.position - transform.position).normalized;
        transform.parent.position += MoveSpeed * Time.deltaTime * directionToPlayer;
    }

    public void GoToKindom()
    {
        transform.parent.position += MoveSpeed * Time.deltaTime * Vector3.left;
    }

    public void SetStage(int index)
    {
        if (index == 1 && IsIdling == true) return;
        if (index == 2 && IsWalking == true) return;
        if (index == 3 && IsAttacking == true) return;
        if (index == 4 && IsDead == true) return;

        IsIdling = index == 1;
        IsWalking = index == 2;
        IsAttacking = index == 3;
        IsDead = index == 4;

        OnStageChange?.Invoke(index);
    }

    void SetEntityInRange(Transform transform)
    {
        IsEntityInRange = true;
        Target = transform;
    }

    void SetEntityOutRange()
    {
        IsEntityInRange = false;
        Target = null;
    }
    public IEnumerator SetSpeed(float addSpeed)
    {
        if (isSpeed)
            yield break;
        isSpeed = true;
        MoveSpeed += addSpeed;
        yield return new WaitForSeconds(3f);
        MoveSpeed -= addSpeed;
        isSpeed = false;
    }
}