using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Text _text;
    [SerializeField]
    private string _showAnimationName = "ShowDamageText";

    public void ShowDamageText(DamageTarget damageTarget)
    {
        _text.text = damageTarget.damage.ToString("F0");
        _animator.Play(_showAnimationName);
        transform.position = Camera.main.WorldToScreenPoint(damageTarget.target.position);
    }

}

[System.Serializable]
public class DamageTarget
{
    public float damage;
    public Transform target;
    public void SetDamageTarget(float damage, Transform target)
    {
        this.damage = damage;
        this.target = target;
    }
}