using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaurdSpawner : MonoBehaviour
{
    GameController gameController;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        StartCoroutine(SpawnGaurdOnDuty());
    }

    IEnumerator SpawnGaurdOnDuty()
    {
        yield return new WaitForSeconds(4);
        GameObject go;
        foreach (Transform t in gameObject.transform)
        {
            int child = t.transform.childCount;
            if (child > 4)
            {
                go = Instantiate(gameController.GaurdAI, gameObject.transform.position, gameObject.transform.rotation);
                go.GetComponent<GaurdController>().AssignedGaurdPoints = t;
                yield return new WaitForSeconds(0.1f);
                go = Instantiate(gameController.GaurdAI, gameObject.transform.position, gameObject.transform.rotation);
                go.GetComponent<GaurdController>().AssignedGaurdPoints = t;
                go.GetComponent<GaurdController>().serialinversePatrol = true;
            }
            else
            {
                go = Instantiate(gameController.GaurdAI, gameObject.transform.position, gameObject.transform.rotation);
                go.GetComponent<GaurdController>().AssignedGaurdPoints = t;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
