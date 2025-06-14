using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private int _numberOfFighters = 2;
    [SerializeField]
    private UnityEvent _onFightersReady;
    [SerializeField]
    private UnityEvent _onBattleFinished;
    [SerializeField]
    private UnityEvent _onBattleStarted;


    private List<Fighter> _fighters = new List<Fighter>();
    private Coroutine _battleCoroutine;
    public void AddFighter(Fighter fighter)
    {
        _fighters.Add(fighter);
        CheckFighters();
    }

    public void RemoveFighter(Fighter fighter)
    {
        _fighters.Remove(fighter);
    }
    private void CheckFighters()
    {
        if (_fighters.Count < _numberOfFighters)
        {
            return;
        }
        _onFightersReady?.Invoke();
    }
    public void StartBattle()
    {
        _battleCoroutine = StartCoroutine(BattleCoroutine());

    }
    private IEnumerator BattleCoroutine()
    {
        _onBattleStarted?.Invoke();
        while (_fighters.Count > 1)
        {
            Fighter attacker = _fighters[Random.Range(0, _fighters.Count)];
            Fighter defender = attacker;
            while (defender == attacker)
            {
                defender = _fighters[Random.Range(0, _fighters.Count)];
            }
            Attack attack = attacker.Attacks.GetRandomAttack();
            yield return new WaitForSeconds(attack.attackTime);
            defender.Health.TakeDamage(Random.Range(attack.minDamage, attack.maxDamage));
            if (defender.Health.CurrentHealth <= 0)
            {
                _fighters.Remove(defender);
                
            }
            yield return null;
        }
        _onBattleFinished?.Invoke();
    }
}
