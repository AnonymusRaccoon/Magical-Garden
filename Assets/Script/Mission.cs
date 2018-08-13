using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Mission : MonoBehaviour {

    string Missiontext = null;
    TreeItem[] items;
    public float difficulte;
    public int minDrop;
    public int itemDrop;
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
    private void GenerateMission()
    {
        for (int i = 0; i < difficulte; i++)
        {
            Objectifs.Add(ChoisirUnTypeDarbre(), Random.Range(MinArbre, MaxArbre)); 
        }
        foreach (KeyValuePair<TreeType,int> i in Objectifs)
        {
            Missiontext = Missiontext + "Place: " + i.Value + " " + i.Key.ToString() +"\n";
        }

        GiveTrees();
    }

    private void GiveTrees()
    {
        InventoryManager manager = GetComponent<InventoryManager>();
        for (int i = 0; i < manager.items.Length; i++)
        {
            if (manager.items[i].type == TreeType.Nothing)
                continue;

            manager.items[i].count = Random.Range(minDrop, itemDrop);

            if (Objectifs.ContainsKey(manager.items[i].type))
            {
                int minCount;
                Objectifs.TryGetValue(manager.items[i].type, out minCount);
                if(manager.items[i].count < minCount)
                    manager.items[i].count += minCount;
                
            }
        }
        manager.UpdateUI();
    }

    public string GetMissionText()
    {
        return Missiontext;  
    }

    private TreeType ChoisirUnTypeDarbre()
    {
        TreeType arbre = items.Where(x => !Objectifs.ContainsKey(x.type) && x.type != TreeType.Nothing).ToArray()[Random.Range(0, items.Where(x => !Objectifs.ContainsKey(x.type) && x.type != TreeType.Nothing).ToArray().Length)].type;
        return arbre;
    }

    public void HasWon()
    {

    }
}
