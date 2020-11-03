using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repository : MonoBehaviour
{
    public TextMesh textMeshHp = null;
    private List<TextMesh> textMeshesList = new List<TextMesh>();
    private static repository repositoryX;
    // Start is called before the first frame update
    void Awake()
    {
        repositoryX = this;
        for(int i = 0; i < 8; i++)
        {
            TextMesh textM = Instantiate(textMeshHp);
            textM.gameObject.SetActive(false);
            textMeshesList.Add(textM);
            textM.transform.SetParent(transform);
        }
    }

   
    public static TextMesh GetTextMeshHp()
    {
        return repositoryX.textMeshesList.Find(x => !x.gameObject.activeInHierarchy);
    }
}
