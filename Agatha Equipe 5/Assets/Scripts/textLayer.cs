using UnityEngine;

public class textLayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Sistema";
    }

}
