using UnityEngine;

public class MapItemRotation : MonoBehaviour
{
    void FixedUpdate() => gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(0, -90, 0));
}
