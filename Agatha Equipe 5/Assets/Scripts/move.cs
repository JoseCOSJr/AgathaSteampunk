using UnityEngine;

public class move : MonoBehaviour
{
    private static float velocity = 3f;
    private Rigidbody2D body;
    private Vector2 dire, posGo;
    private bool goToPosition = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RigidbodyConstraints2D constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 direX = dire;

        if (goToPosition)
        {
            direX = posGo - (Vector2)transform.position;
            float distPos = direX.magnitude;
            direX.Normalize();
            float distMove = velocity * Time.fixedDeltaTime;
            if (distPos < distMove)
            {
                direX = direX * distPos / distMove;
                goToPosition = false;
            }
        }

        if (direX.sqrMagnitude > 0f)
        {
            body.constraints = constraints;
            Vector2 vNew = direX * velocity;
            body.velocity = vNew;
            if (body.velocity.x > 0f)
            {
                Turn(true);
            }
            else if (body.velocity.x < 0f)
            {
                Turn(false);
            }
        }
        else
        {
            constraints= constraints | RigidbodyConstraints2D.FreezePosition;

            if (body.constraints != constraints)
            {
                body.constraints = constraints;
            }
        }
    }

    public void MoveDirect(Vector2 diretion)
    {
        if (diretion.magnitude > 1f)
        {
            diretion.Normalize();
        }
        dire = diretion;
    }

    public void Turn(bool right)
    {
        if (right)
        {
            if (transform.eulerAngles.y != 0f)
            {
                transform.eulerAngles = Vector3.zero;
            }
        }
        else if (transform.eulerAngles.y != 180f)
        {
            transform.eulerAngles = Vector3.up * 180f;
        }
    }

    public void GoToPostion(Vector2 pos)
    {
        posGo = pos;
        goToPosition = true;
    }

    public bool GoingToPosition()
    {
        return goToPosition;
    }
}
