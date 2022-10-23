using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    private void Awake()
    {
        if (gameObject.name == "Hit Environment(Clone)")
        {
            Destroy(gameObject, 5f);
        }
        else
        {
            Destroy(gameObject, 1f);
        }
    }
}
