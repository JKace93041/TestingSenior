using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DisplayText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private PlayerStateMachine playerState;

    // Start is called before the first frame update
    void Start()
    {
        playerState = new PlayerStateMachine();
    }

    // Update is called once per frame
    void Update()
    {

        //textMesh.text = playerState._currentState.ToString();
        textMesh.SetText(playerState._currentState.ToString());
    }
}
