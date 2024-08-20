using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimation : MonoBehaviour
{

    public GameObject Player;
    public GameObject Camera;
    public GameObject HUD;
    public GameObject HUD2;

    private void Start()
    {
        Player.SetActive(false);
    }

    void Update()
    {
        StartCoroutine(time());
       
    }

    IEnumerator time()
    {
        yield return new WaitForSeconds(4.1f);
        Destroy(Camera);
        Player.SetActive(true);
        HUD.SetActive(true);
        HUD2.SetActive(true);

    }

}
