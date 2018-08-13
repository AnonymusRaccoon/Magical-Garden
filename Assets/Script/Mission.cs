using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Mission : MonoBehaviour {

    string Missiontext = null;
    TreeItem[] items;
    public float difficulte;
    public Dictionary<TreeType, int> Objectifs = new Dictionary<TreeType, int>();
    public int MinArbre = 1;
    public int MaxArbre = 10;
    private void Start()
    {
        items = GetComponent<InventoryManager>().items;
        if(items == null)
        {
            Debug.Log("eroor");
        }

        if (difficulte> items.Length)
        {
            Debug.LogError("difficulté trop grande");
        }
        else
        {
            GenerateMission();
        }
       
    }
    public void GenerateMission()
    {
       
        for (int i = 0; i < difficulte; i++)
        {
            Objectifs.Add(ChoisirUnTypeDarbre(), Random.Range(MinArbre, MaxArbre)); 
        }
        foreach (KeyValuePair<TreeType,int> i in Objectifs)
        {
            Missiontext = Missiontext + "Place: " + i.Value + " " + i.Key.ToString() +"\n";
        }
    }

    public string GetMissionText()
    {
        return Missiontext;  
    }

    public TreeType ChoisirUnTypeDarbre()
    {
        TreeType arbre = items.Where(x => !Objectifs.ContainsKey(x.type) && x.type != TreeType.Nothing).ToArray()[Random.Range(0, items.Where(x => !Objectifs.ContainsKey(x.type) && x.type != TreeType.Nothing).ToArray().Length)].type;
        return arbre;
    }

    public void HasWon()
    {

    }
}
