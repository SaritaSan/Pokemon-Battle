using UnityEngine;
using UnityEngine.Events;

public class Fighter : MonoBehaviour
{
    [SerializeField]
    private string _name;
    public string Name => _name;
    [SerializeField]
    private Health _health;
    [SerializeField]
    private Animator _characterAnimator;
    [SerializeField]
    private Attacks _attacks;
    public Health Health => _health;
    public Attacks Attacks => _attacks;
    public Animator CharacterAnimator => _characterAnimator;
    [SerializeField]
    private UnityEvent _onFighterInitialized;
    [SerializeField]
    private string _winAnimationName = "Win";
    public string WinAnimationName => _winAnimationName;
    [SerializeField]
    private string _winSoundName = "WindSound";
    public string WindSoundName => _winSoundName;
    public void InitializeFighter()
    {
        _onFighterInitialized?.Invoke();
    }
}
