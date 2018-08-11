using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public class LaggManager : MonoBehaviour {

    
    private int FreezeJitter = 1;
    private float MinLaggJitter = 0.07f;
    private float MaxLaggJitter = 0.15f;

	// Use this for initialization
	void Start () {
        QualitySettings.vSyncCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K))
        {
            BugFreeze(20);
        }
	}
    public async void VitesseRalentit(float VitesseDeJeu)
    {
        float NewLaggJitter = Random.Range(MinLaggJitter, MaxLaggJitter);

        VitesseDeJeu = Random.Range( VitesseDeJeu-NewLaggJitter , VitesseDeJeu+NewLaggJitter );
        VitesseDeJeu = Mathf.Clamp(VitesseDeJeu, 0.1f, 0.9f);
        
        Time.timeScale = VitesseDeJeu;
        await Task.Delay(10); 
    }
    public void ResetLagg()
    {
        Time.timeScale = 1; 
    }
    public async void BugFreeze(float DuréeLagg)
    {
        Debug.Log("call");
        DuréeLagg = Mathf.Clamp( Random.Range(DuréeLagg - FreezeJitter, DuréeLagg + FreezeJitter), 0, Mathf.Infinity);
        Application.targetFrameRate = 10;
        await Task.Delay((int)DuréeLagg);
        ResetFramerate();
    }
    public void ResetFramerate()
    {
        Application.targetFrameRate = 60;
    }
}
