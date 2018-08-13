using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mission : MonoBehaviour {

    string Missiontext = null;
    TreeItem[] items;
    public float difficulte;
    private Dictionary<string, int> Objectifs = new Dictionary<string, int>();
    public int MinArbre = 1;
    public int MaxArbre = 4;
    private void Start()
    {
        items = GetComponent<InventoryManager>().items;
        if(items == null)
        {
            Debug.Log("eroor");
        }
         //Debug.Log(items[0].type);
        GenerateMission();
    }
    public void GenerateMission()
    {
       
        for (int i = 0; i < difficulte; i++)
        {
            Objectifs.Add(ChoisirUnTypeDarbre(), Random.Range(MinArbre, MaxArbre)); 
        }
        foreach (KeyValuePair<string,int> i in Objectifs)
        {
            Missiontext = Missiontext + "Place" + i.Key + i.Value;
        }


        //ChoisirUnTypeDarbre()
        //Random.Range(MinArbre, MaxArbre)


    }
    public string GetMissionText()
    {
        return Missiontext;  
    }
    public string ChoisirUnTypeDarbre()
    {
       return items[Random.Range(0, items.Length)].ToString();
    }
}
