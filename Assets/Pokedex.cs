using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class Pokedex : MonoBehaviour
{
    public TextMeshProUGUI PokeText;
    public int PokeTemps = 5;
    private bool ArretDéfilement = false;
    private string PokeSubDescription;

    //Remove comments for windows pokedefilement
    public /*async*/ void PokeDescription(int PokeNumero)
    {
        PokeSubDescription = GetComponent<InventoryManager>().items[PokeNumero].description;
        string descriptionSave = PokeSubDescription;
        PokeText.text = PokeSubDescription; //null

        //if (PokeSubDescription != descriptionSave)
        //    return;

        //for (int i = 0; i < PokeSubDescription.Length; i++)
        //{
        //    if (PokeSubDescription != descriptionSave)
        //        return;

        //    PokeText.text += PokeSubDescription[i];
        //    await Task.Delay(PokeTemps);
        //}
    }
    public void UpdateMissionText()
    {
        PokeSubDescription = GetComponent<Mission>().GetMissionText();
        PokeText.text = PokeSubDescription;
    }
}
