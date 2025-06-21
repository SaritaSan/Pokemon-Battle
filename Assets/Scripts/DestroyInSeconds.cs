using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    [SerializeField]
    private float _destroyAfterSeconds = 2f;
    private void Start()
    {
        Destroy(gameObject, _destroyAfterSeconds);
    }
}
