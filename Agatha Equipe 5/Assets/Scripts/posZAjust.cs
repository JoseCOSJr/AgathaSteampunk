using UnityEngine;

public class posZAjust : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = pos.y;
        transform.position = pos;
    }
}
