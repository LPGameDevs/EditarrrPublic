using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    chase,
    sentry,
    flying
}

[CreateAssetMenu(fileName = "EnemyAI", menuName = "EnemyAIData")]
public class EnemyAIDataSO : ScriptableObject
{
    public float normalMoveSpeed = 5f;
    public float sawPlayerMoveSpeed = 10f;
    public float detectionRange = 10f;
    public float waitToMove = 2f;
    public EnemyType enemyType = EnemyType.chase;
}
