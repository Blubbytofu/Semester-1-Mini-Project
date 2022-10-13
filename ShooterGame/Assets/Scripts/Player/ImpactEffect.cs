using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    void Awake()
    {
        Invoke("DestroyEffect", 1f);
    }

    private void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
