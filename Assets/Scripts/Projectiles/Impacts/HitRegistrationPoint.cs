using UnityEngine;
using System.Collections;

public class HitRegistrationPoint : MonoBehaviour
{
    public float decayTime = 2f;

    void Start()
    {
        StartCoroutine(Decay());
    }

    IEnumerator Decay()
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }
}
