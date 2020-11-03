using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class turnCombat : MonoBehaviour
{
    private Vector2[] posPlayer = new Vector2[4], posEnemies = new Vector2[4];
    private static turnCombat combatControll = null;
    private bool wait = false;
    private static List<attributesCombat> attributesPlayers = new List<attributesCombat>(), attributesEnemies = new List<attributesCombat>();
    private attributesCombat atbTurn = null, atbTarget = null;
    public enum combatState { begin, choice, action, wait, final};
    private combatState state = combatState.begin;
    private float timeCount = 0f;
    private int turnId = 0;

    // Start is called before the first frame update
    void Awake()
    {
        combatControll = this;
    }

    private void Start()
    {
        playerCombat.loadScene = false;

        posPlayer[0] = -Vector2.right * 2f - Vector2.up / 4f;
        posPlayer[1] = posPlayer[0] + Vector2.up/2f - Vector2.right;
        posPlayer[2] = posPlayer[0] - Vector2.up/2f - Vector2.right;
        posPlayer[3] = posPlayer[0] - Vector2.up;

        for (int i = 0; i < posPlayer.Length; i++)
        {
            Vector2 pos = posPlayer[i];
            pos.x *= -1f;
            posEnemies[i] = pos;
        }

        List<attributesCombat> listAux = new List<attributesCombat>();
        for(int i = 0; i < attributesPlayers.Count; i++)
        {
            attributesCombat atb = Instantiate(attributesPlayers[i]);
            atb.tag = "Player";
            atb.name = atb.GetIdName();
            Vector3 pos = posPlayer[i];
            atb.transform.position = pos + Vector3.left * 5f;
            listAux.Add(atb);
            atb.GetMove().GoToPostion(pos);
        }
        attributesPlayers = listAux.FindAll(x => x);
        listAux.Clear();

        for (int i = 0; i < attributesEnemies.Count; i++)
        {
            attributesCombat atb = Instantiate(attributesEnemies[i]);
            atb.name = atb.GetIdName();
            atb.tag = "Enemie";
            Vector3 pos = posEnemies[i];
            atb.transform.position = pos + Vector3.right * 5f;
            listAux.Add(atb);
            atb.GetMove().GoToPostion(pos);
        }
        attributesEnemies = listAux.FindAll(x => x);

        GoBackToPosition();
    }

    public static turnCombat GetTurnCombat()
    {
        return combatControll;
    }

    public bool GetWait()
    {
        return wait;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == combatState.begin)
        {
            List<attributesCombat> atbList = GetAtbsList();

            if (!atbList.Exists(x => x.GetMove().GoingToPosition()))
            {
                state = combatState.wait;
            }
        }
        else if(state == combatState.wait)
        {
            if (timeCount < 1f)
            {
                timeCount += Time.deltaTime;
            }
            else
            {
                timeCount = 0f;
                while(!atbTurn || (atbTurn && atbTurn.GetHpNow() <= 0))
                {
                    atbTurn = GetAttributeTurn();
                    turnId += 1;
                    if (turnId >= GetAtbsList().Count)
                    {
                        turnId = 0;
                    }
                }
                state = combatState.choice;
            }
        }
        else if(state == combatState.choice)
        {
            string tagTarget = "Player";
            if (atbTurn.CompareTag(tagTarget))
            {
                tagTarget = "Enemie";
            }
            List<attributesCombat> list = GetAtbsList(tagTarget).FindAll(x => x.GetHpNow() > 0);
            int id = Random.Range(0, list.Count);
            atbTarget = list[id];
            state = combatState.action;
        }
        else if(state == combatState.action)
        {
            atbTarget.AddHp(-1);
            atbTurn = null;
            atbTarget =null;
            if (SomeoneWon())
            {
                state = combatState.final;
            }
            else
            {
                state = combatState.wait;
            }
        }
        else if(state == combatState.final)
        {
            if (!playerCombat.loadScene)
            {
                Debug.Log("Venceu");
                SceneManager.LoadSceneAsync("SampleScene");
                playerCombat.loadScene = true;
                combatControll = null;
            }
        }
    }

   public static void SetFighters(team team, bool player)
    {
        if (player)
        {
            attributesPlayers = team.GetMembers();
        }
        else
        {
            attributesEnemies = team.GetMembers();
        }
    }

    private List<attributesCombat> GetAtbsList(string tag = "")
    {
        List<attributesCombat> atbList = attributesPlayers.FindAll(x => x);
        atbList.AddRange(attributesEnemies);

        if (tag == "")
            return atbList;

        return atbList.FindAll(x => x.CompareTag(tag));
    }

    private attributesCombat GetAttributeTurn()
    {
        List<attributesCombat> listAtbOrder = GetAtbsList();
        listAtbOrder.Sort(delegate (attributesCombat x, attributesCombat y)
        {
            if (x.GetAgility() < y.GetAgility())
            {
                return 1;
            }
            else if(x.GetAgility() > y.GetAgility())
            {
                return -1;
            }

            return 0;
        });

        return listAtbOrder[turnId];
    }


    private void GoBackToPosition()
    {
        List<attributesCombat> listAtb = GetAtbsList("Enemie");
        for(int i = 0; i < listAtb.Count; i++)
        {
            attributesCombat atb = listAtb[i];
            Vector3 pos = posEnemies[i];
            pos.z = pos.y;

            if(atb.GetHpNow() > 0 && atb.transform.position != pos)
            {
                atb.GetMove().GoToPostion(pos);
            }
        }

        listAtb = GetAtbsList("Player");
        for (int i = 0; i < listAtb.Count; i++)
        {
            attributesCombat atb = listAtb[i];
            Vector3 pos = posPlayer[i];
            pos.z = pos.y;

            if (atb.GetHpNow() > 0 && atb.transform.position != pos)
            {
                atb.GetMove().GoToPostion(pos);
            }
        }
    }

    private bool SomeoneWon()
    {
        List<attributesCombat> list = GetAtbsList("Player");
        if (!list.Exists(x => x.GetHpNow() > 0))
        {
            return true;
        }

        list = GetAtbsList("Enemie");

        return !list.Exists(x => x.GetHpNow() > 0);
    }
}
