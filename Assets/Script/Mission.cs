using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mission : MonoBehaviour {


    public int easy;
    public int hard;

    [Space]
    public int minDrop;
    public int maxDrop;

    [Space]
    public int fewTree;
    public int manyTree;

    public Dictionary<TreeType, int> Objectifs = new Dictionary<TreeType, int>();
    TreeItem[] items;
    public GameObject WinUI; 
    public GameObject LooseUI; 
    private void Start()
    {
        items = GetComponent<InventoryManager>().items;
        if(items == null)
        {
            Debug.Log("eroor");
        }

        if (hard > items.Length)
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
        Objectifs = new Dictionary<TreeType, int>();
        int loop = Random.Range(easy, hard);
        for (int i = 0; i < loop; i++)
        {
            AddTree();
        }
        string mission = null;
        foreach (KeyValuePair<TreeType,int> i in Objectifs)
        {
            mission = mission + "Place: " + i.Value + " " + i.Key.ToString() +"\n";
        }

        GetComponent<InventoryManager>().PlaceRandomTrees(Random.Range(fewTree, manyTree));
        GiveTrees();
        GetComponent<Pokedex>().UpdateMissionText();
    }

    private void GiveTrees()
    {
        InventoryManager manager = GetComponent<InventoryManager>();
        for (int i = 0; i < manager.items.Length; i++)
        {
            if (manager.items[i].type == TreeType.Nothing)
                continue;

            manager.items[i].count = Random.Range(minDrop, maxDrop);

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
        string mission = null;
        foreach (KeyValuePair<TreeType, int> i in Objectifs)
        {
            mission = mission + "Place: " + i.Value + " " + i.Key.ToString() + " (" + (i.Value - GetComponent<InventoryManager>().TreePlaced(i.Key)) + " Left)\n";
        }
        return mission;
    }

    private void AddTree()
    {
        TreeItem[] trees = items.Where(x => !Objectifs.ContainsKey(x.type) && x.type != TreeType.Nothing).ToArray();
        if (trees.Length > 0)
        {
            int i = Random.Range(0, trees.Length);
            TreeType type = trees[i].type;
            int number = Random.Range(items[i].maxInstanceForWin / 2 + 1, items[i].maxInstanceForWin);
            Objectifs.Add(type, number);
        }
    }

    public void HasWon()
    {
        WinUI.SetActive(true);
    }
}
