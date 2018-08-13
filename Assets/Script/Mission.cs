using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Mission : MonoBehaviour {

    string Missiontext = null;
    TreeItem[] items;
    public float difficulte;
    private Dictionary<string, int> Objectifs = new Dictionary<string, int>();
    public int MinArbre = 1;
    public int MaxArbre = 10;
    private void Start()
    {
        items = GetComponent<InventoryManager>().items;
        if(items == null)
        {
            Debug.Log("eroor");
        }
         //Debug.Log(items[0].type);
       if(difficulte> items.Length)
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
        foreach (KeyValuePair<string,int> i in Objectifs)
        {
            Missiontext = Missiontext + "Place: " + i.Value+ " " + i.Key +"\n";
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
        string arbre = items.Where(x => !Objectifs.ContainsKey(x.type.ToString()) && x.type != TreeType.Nothing).ToArray()[Random.Range(0, items.Where(x => !Objectifs.ContainsKey(x.type.ToString()) && x.type != TreeType.Nothing).ToArray().Length)].type.ToString();
        //if (Objectifs.ContainsKey(arbre))
        //{
        //    return ChoisirUnTypeDarbre();

        //}
        //else
        //{
            return arbre;
        //}
    }
}
