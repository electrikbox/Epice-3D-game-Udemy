using System.Collections;
using UnityEngine;

public class WinTextAnim : MonoBehaviour
{
    void FixedUpdate()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        yield return new WaitForSeconds(4f);
    }
}
