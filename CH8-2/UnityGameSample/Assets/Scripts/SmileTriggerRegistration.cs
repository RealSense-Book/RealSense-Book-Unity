using UnityEngine;
using System.Collections;
using RSUnityToolkit;

public class SmileTriggerRegistration : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTrigger(Trigger trigger)
    {
        Done_PlayerController playerController = GetComponent<Done_PlayerController>();
        playerController.SendMessage("setSmile");
    }

}
