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

    public async void PokeDescription(int PokeNumero)
    {
        if (PokeBool == false)
        {
            string PokeSubDescription = GetComponent<InventoryManager>().items[PokeNumero].description;
            PokeText.text = null;
            PokeBool = true;
            for (int i = 0; i < PokeSubDescription.Length; i++)
            {
                PokeText.text += PokeSubDescription[i];
                await Task.Delay(PokeTemps);
            }
            PokeBool = false;
        }

    }
}
