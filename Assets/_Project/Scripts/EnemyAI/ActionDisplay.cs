using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDisplay : MonoBehaviour
{
    [SerializeField] private EnemyAIController enemyAIController;
    [SerializeField] private GameObject exclamation;
    [SerializeField] private float actionShowTime = 2f;

    private float _timer;
    private bool _showExclamationPoint;
    private EnemyAIController.AIState _currentState;

    private void Start()
    {
        enemyAIController.OnStateChanged += EnemyAIController_OnStateChanged;
        exclamation.SetActive(false);
    }

    private void EnemyAIController_OnStateChanged(EnemyAIController.AIState state)
    {
        if (EnemyAIController.AIState.alerting == state && _currentState != state)
        {
            _timer = 0;
            print("Start action");
            _showExclamationPoint = true;
            exclamation.SetActive(true);
        }
    }

    private void Update()
    {
        if (!_showExclamationPoint) { return; }
        _timer += Time.deltaTime;
        if (_timer > actionShowTime)
        {
            print("Stop Action");
            _showExclamationPoint = false;
            exclamation.SetActive(false);
            _timer = 0;
        }
    }
}
