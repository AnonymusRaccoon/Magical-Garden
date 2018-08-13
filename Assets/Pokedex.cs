using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class Pokedex : MonoBehaviour
{
    public TextMeshProUGUI PokeText;
    public int PokeTemps = 10;
    private bool PokeBool = false;
    public bool ArretDéfilement = false;


    public async void PokeDescription(int PokeNumero)
    {
        if (PokeBool == false)
        {
            string PokeSubDescription = GetComponent<InventoryManager>().items[PokeNumero].description;
            PokeText.text = null;
            PokeBool = true;
            for (int i = 0; i < PokeSubDescription.Length; i++)
            {
                if (ArretDéfilement == true)
                {
                    ArretDéfilement = false;
                    PokeBool = false;
                    return;
                }
                PokeText.text += PokeSubDescription[i];
                await Task.Delay(PokeTemps);
                
            }
            PokeBool = false;
        }

    }
    public void UpdateMissionText()
    {
        ArretDéfilement = true;
        
        PokeText.text = GetComponent<Mission>().GetMissionText();
        ArretDéfilement = false;
    }
}
