/**
**/

using UnityEngine;
using System.Collections;

public class ImpactRemoval : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!ps.IsAlive())
            Destroy(gameObject);
    }
}
