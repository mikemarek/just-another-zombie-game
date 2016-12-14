/**
* HitRegistrationPoint.cs
* Created by Michael Marek (2016)
*
* A small visual marking to denote where the impact of a projectile took place. Removes itself
* after a set amount of time.
**/

using UnityEngine;
using System.Collections;

public class HitRegistrationPoint : MonoBehaviour
{
    public float decayTime = 2f;

    /**
    **/
    void Start()
    {
        StartCoroutine(Decay());
    }

    /**
    **/
    IEnumerator Decay()
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }
}
