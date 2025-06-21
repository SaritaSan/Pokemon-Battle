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
    private DamageTarget _damageTarget = new DamageTarget();

    public void AddFighter(Fighter fighter)
    {
        _fighters.Add(fighter);
        CheckFighters();
    }

    public void RemoveFighter(Fighter fighter)
    {
        _fighters.Remove(fighter);
        if (_battleCoroutine != null)
        {
            StopCoroutine(_battleCoroutine);
            _battleCoroutine = null;
        }
    }
    private void CheckFighters()
    {
        if (_fighters.Count < _numberOfFighters)
        {
            return;
        }
        _onFightersReady?.Invoke();
        StartBattle();
    }
    public void StartBattle()
    {
        foreach (Fighter fighter in _fighters)
        {
            fighter.InitializeFighter();
        }
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
            attacker.transform.LookAt(defender.transform);
            defender.transform.LookAt(attacker.transform);
            Attack attack = attacker.Attacks.GetRandomAttack();
            SoundManager.instance.Play(attack.soundName);
            attacker.CharacterAnimator.Play(attack.animationName);
            GameObject attackParticles = Instantiate(attack.particlesPrefab, attacker.transform.position, Quaternion.identity);
            attackParticles.transform.SetParent(attacker.transform);
            Instantiate(attack.particlesPrefab, attacker.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(attack.attackTime);
            float damage = Random.Range(attack.minDamage, attack.maxDamage);
            GameObject defenderParticles = Instantiate(attack.hitParticlesPrefab, defender.transform.position, Quaternion.identity);
            defenderParticles.transform.SetParent(defender.transform);
            Instantiate(attack.hitParticlesPrefab, defender.transform.position, Quaternion.identity);
            _damageTarget.SetDamageTarget(damage, defender.transform);
            defender.Health.TakeDamage(_damageTarget);
            if (defender.Health.CurrentHealth <= 0)
            {
                _fighters.Remove(defender);
                
            }
            yield return new WaitForSeconds(1f);
        }
        _onBattleFinished?.Invoke();
    }
}
