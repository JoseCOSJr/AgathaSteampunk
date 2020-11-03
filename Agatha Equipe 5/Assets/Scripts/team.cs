using System.Collections.Generic;
using UnityEngine;

public class team : MonoBehaviour
{
    [SerializeField]
    private List<attributesCombat> teamMembers = new List<attributesCombat>();

    public void AddMembers(attributesCombat who)
    {
        teamMembers.Add(who);
    }

    public void RemoveMembers(attributesCombat who)
    {
        teamMembers.Remove(who);
    }

    public List<attributesCombat> GetMembers()
    {
        return teamMembers.FindAll(x => x);
    }
}
