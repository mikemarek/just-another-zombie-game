using UnityEngine;
using System.Collections;

public class BigFuckingDoor : MonoBehaviour
{
    public  Vector3 openPosition;
    [Space(10)]
    public  Vector3 closedPosition;
    [Space(10)]
    public  float   speed;

    private bool    isOpen = false;

    public void Open()
    {
        StopAllCoroutines();
        StartCoroutine(OpenDoor());

        isOpen = true;
    }

    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(CloseDoor());

        isOpen = false;
    }

    private IEnumerator OpenDoor()
    {
        while (Vector3.Distance(transform.position, openPosition) > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, speed);
            yield return null;
        }
    }

    private IEnumerator CloseDoor()
    {
        while (Vector3.Distance(transform.position, closedPosition) > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, speed);
            yield return null;
        }
    }

    public bool open { get { return isOpen; } }
}
