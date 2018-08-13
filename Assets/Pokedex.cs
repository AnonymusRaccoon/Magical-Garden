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

    public async void PokeDescription(int PokeNumero)
    {
        ArretDéfilement = false;
        PokeSubDescription = GetComponent<InventoryManager>().items[PokeNumero].description;
        PokeText.text = null;
        for (int i = 0; i < PokeSubDescription.Length; i++)
        {
            if (ArretDéfilement == true)
            {
                return;
            }
            PokeText.text += PokeSubDescription[i];
            await Task.Delay(PokeTemps);
        }
    }
    public void UpdateMissionText()
    {
        ArretDéfilement = true;
        
        PokeText.text = GetComponent<Mission>().GetMissionText();
    }
}
