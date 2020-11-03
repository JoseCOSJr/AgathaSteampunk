using UnityEngine;

public class attributesCombat : MonoBehaviour
{
    [SerializeField]
    private string idName = "";
    [Header("Atributos de combate")]
    [SerializeField]
    private int hpMax = 10;
    private int hpNow;
    [SerializeField]
    private int agility = 10;
    private move move;
    private TextMesh textHp = null;
    private float timeHpShow = 0f;

    public string GetIdName()
    {
        return idName;
    }

    public int GetAgility()
    {
        return agility;
    }

    public int GetHpNow()
    {
        return hpNow;
    }

    public move GetMove()
    {
        return move;
    }

    // Start is called before the first frame update
    void Awake()
    {
        hpNow = hpMax;
        move = GetComponent<move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (textHp)
        {
            timeHpShow -= Time.deltaTime;

            if (timeHpShow <= 0f)
            {
                textHp.gameObject.SetActive(false);
                textHp = null;
            }
        }
    }

    private void SetHpValue(int value)
    {
        hpNow = value;
    }

    public void AddHp(int value)
    {
        int v = hpNow + value;
        if (v > hpMax)
        {
            v = hpMax;
        }
        else if (v <= 0)
        {
            v = 0;
            gameObject.SetActive(false);
        }

        SetHpValue(v);
        if (!textHp)
        {
            textHp = repository.GetTextMeshHp();
            textHp.gameObject.SetActive(true);
            textHp.transform.SetParent(transform);
            textHp.transform.localPosition = Vector3.up * 1.5f;
        }
        textHp.text = hpNow + "/" + hpMax;
        timeHpShow = 2f;
    }
}
