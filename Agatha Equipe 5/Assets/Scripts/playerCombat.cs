using UnityEngine;
using UnityEngine.SceneManagement;

public class playerCombat : MonoBehaviour
{
    public static bool loadScene = false;
    private team team;

    private void Start()
    {
        team = GetComponent<team>();
        loadScene = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!loadScene && !turnCombat.GetTurnCombat())
        {
            team tb = collision.GetComponentInParent<team>();

            if (tb && tb.CompareTag("Enemie"))
            {
                loadScene = true;
                
                turnCombat.SetFighters(team, true);
                turnCombat.SetFighters(tb, false);

                SceneManager.LoadSceneAsync("CombatScene");
            }
        }
    }
}
