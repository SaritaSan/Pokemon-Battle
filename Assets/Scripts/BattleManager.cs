using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private int _numberOfFighters = 2;
    [SerializeField]
    private UnityEvent _onBattleStopped;
    [SerializeField]
    private UnityEvent _onBattleFinished;
    [SerializeField]
    private UnityEvent _onBattleStarted;
    [SerializeField]
    private UnityEvent _OnFighter;
    [SerializeField]
    private UnityEvent _NoFighter;
    private List<Fighter> _fighters = new List<Fighter>();
    private Coroutine _battleCoroutine;
    private DamageTarget _damageTarget = new DamageTarget();
    public void AddFighter(Fighter fighter)
    {
        MessageFrame.Instance.ShowMessage($"{fighter.Name} has joined the battle!");
        _fighters.Add(fighter);
        CheckFighters();
        _OnFighter?.Invoke();
    }
    public void RemoveFighter(Fighter fighter)
    {
        _fighters.Remove(fighter);
        if (_fighters.Count < 1)
        {
            _NoFighter?.Invoke();
        }
        if (_fighters.Count < 2)
            {
                StopBattle();
            }
    }
    private void StopBattle()
    {
        if (_battleCoroutine != null)
            {
                StopCoroutine(_battleCoroutine);
                _battleCoroutine = null;
            }
            _onBattleStopped?.Invoke();
    }
    private void CheckFighters()
    {
        if (_fighters.Count < _numberOfFighters)
        {
            return;
        }
        StopBattle();
        InitializeFighters();
        _onBattleStarted?.Invoke();
    }
    private void InitializeFighters()
    {
        foreach (Fighter fighter in _fighters)
        {
            fighter.InitializeFighter();
        }
    }
    public void StartBattle()
    {
        _battleCoroutine = StartCoroutine(BattleCoroutine());
    }
    private IEnumerator BattleCoroutine()
    {
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
            MessageFrame.Instance.ShowMessage($"{attacker.Name} attacks with {attack.attackName}!");
            SoundManager.instance.Play(attack.soundName);
            attacker.CharacterAnimator.Play(attack.animationName);
            GameObject attackParticles = Instantiate(attack.particlesPrefab, attacker.transform.position, Quaternion.identity);
            attackParticles.transform.SetParent(attacker.transform);
            yield return new WaitForSeconds(attack.attackTime);
            float damage = Random.Range(attack.minDamage, attack.maxDamage);
            GameObject defendParticles = Instantiate(attack.hitParticlesPrefab, defender.transform.position, Quaternion.identity);
            defendParticles.transform.SetParent(defender.transform);
            _damageTarget.SetDamageTarget(damage, defender.transform);
            defender.Health.TakeDamage(_damageTarget);
            if (defender.Health.CurrentHealth <= 0)
            {
                _fighters.Remove(defender);
            }
            yield return new WaitForSeconds(1f);
        }
        EndBattle(_fighters[0]);
    }
    private void EndBattle(Fighter winner)
    {
        winner.transform.LookAt(Camera.main.transform);
        MessageFrame.Instance.ShowMessage($"{winner.Name} wins the battle!");
        SoundManager.instance.Play(winner.WinSoundName);
        winner.CharacterAnimator.Play(winner.WinAnimationName);
        _onBattleFinished?.Invoke();
    }
}
