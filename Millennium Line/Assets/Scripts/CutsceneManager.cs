using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("Beginning Bus Cutscene Settings")]
    public Transform startBusMarker;
    public Transform endBusMarker;
    public GameObject busObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartingCutscene());
    }

    IEnumerator StartingCutscene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.SetParent(busObject.transform.GetChild(0));
        player.transform.localPosition = Vector3.zero;
        player.GetComponent<PlayerController>().ChangeMovementState(1);

        busObject.transform.position = startBusMarker.position;

        float time = 0;

        while (busObject.transform.position.x < endBusMarker.position.x)
        {
            // The formula for the bus speed is equal to a(1-b^-x/c), where x is the distance from the bus to the end 
            // a = max speed, b exponential constant, c = time constant (decrease b and/or increase c for a longer deceleration)
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            Debug.Log(3 * (1 - Mathf.Pow(1.1f, -(Mathf.Abs(busObject.transform.position.x - endBusMarker.position.x) / 30f))));
            busObject.transform.position += Vector3.right * Mathf.Max(3*(1-Mathf.Pow(1.1f, -(Mathf.Abs(busObject.transform.position.x - endBusMarker.position.x) / 30f))), 0.001f);

            //transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0,0,0, 1 - Mathf.Min(time / 3f, 1));
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2);

        player.transform.SetParent(null);
        player.GetComponent<PlayerController>().ChangeMovementState(0);

        yield return new WaitForSeconds(1);

        time = 0;

        while(time < 20)
        {
            // Formula for bus speed is similar to above, but x = time
            busObject.transform.position += Vector3.right * 3 * (1 - Mathf.Pow(1.2f, -(time / 10)));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }
    
}
