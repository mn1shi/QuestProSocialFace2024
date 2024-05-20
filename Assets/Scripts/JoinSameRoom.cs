using System.Collections;
using System.Collections.Generic;
using Ubiq.Rooms;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Net.Mail;
using Ubiq.Samples;

public class JoinSameRoom : MonoBehaviour
{
    public SocialMenu mainMenu;
    public TMP_Text joincodeText;
    private string lastRequestedJoincode;


    public void Join()
    {
        lastRequestedJoincode = joincodeText.text.ToLowerInvariant();
        Debug.Log("code: " + lastRequestedJoincode);
        mainMenu.roomClient.Join(joincode: lastRequestedJoincode);
    }

}
