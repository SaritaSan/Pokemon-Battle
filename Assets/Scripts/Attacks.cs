using UnityEngine;

[CreateAssetMenu(fileName = "Attacks", menuName = "Scriptable Objects/Attacks")]
public class Attacks : ScriptableObject
{
    public Attack[] attacks;
    public Attack GetRandomAttack()
    {
        if (attacks == null || attacks.Length == 0)
        {
            Debug.LogWarning("No attacks available.");
            return null;

        }
        int randomIndex = Random.Range(0, attacks.Length);
        return attacks[randomIndex];
    }

}

[System.Serializable]
public class Attack
{
    public float minDamage;
    public float maxDamage;
    public float attackTime;
    public string animationName;
    public string soundName;
    public GameObject particlesPrefab;
}
