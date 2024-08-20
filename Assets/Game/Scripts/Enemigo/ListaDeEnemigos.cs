using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class ListaDeEnemigos : MonoBehaviour
{
    public GameObject enemigoPrefab;
    private const float ElapsedFramesPerSecond = 16.67f;

    public Transform[] spawnPoints;

    private bool enemigosActivados = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!enemigosActivados && other.CompareTag("Player"))
        {
            ActivarEnemigos();
            enemigosActivados = true;
        }
        this.GetComponent<BoxCollider>().enabled = false;
    }

    public void ActivarEnemigos()
    {
        StartCoroutine(CreateList(spawnPoints.Length, SpawnEnemigoPrefab, ActivarEnemigosEnSlices));
    }

    private GameObject SpawnEnemigoPrefab()
    {
        return Instantiate(enemigoPrefab);
    }

    private IEnumerator CreateList<T>(int count, Func<T> spawnItem, Action<List<T>> OnEndCallBack)
    {
        var tempList = new List<T>();
        Stopwatch sw = new Stopwatch();
        sw.Start();

        for (int i = 0; i < count; i++)
        {
            tempList.Add(spawnItem());

            if (sw.ElapsedMilliseconds >= ElapsedFramesPerSecond)
            {
                yield return null;
                sw.Restart();
            }
        }

        OnEndCallBack?.Invoke(tempList);
    }

    private void ActivarEnemigosEnSlices<T>(List<T> tempList)
    {
        for (int i = 0; i < tempList.Count; i++)
        {
            GameObject enemigo = tempList[i] as GameObject;
            if (enemigo != null)
            {
                enemigo.transform.position = spawnPoints[i].position;
                enemigo.SetActive(true);
            }
        }
    }
}






