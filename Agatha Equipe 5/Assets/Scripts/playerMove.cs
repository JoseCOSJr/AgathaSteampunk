using UnityEngine;

public class playerMove : MonoBehaviour
{
    private move move;
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<move>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerCombat.loadScene)
        {
            Vector2 dire = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            move.MoveDirect(dire);
        }
    }
}
