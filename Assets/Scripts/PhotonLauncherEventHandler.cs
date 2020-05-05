using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLauncherEventHandler : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public PhotonLauncher launcher;

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        Debug.Log("PUN: OnEvent - " + eventCode);

        if (eventCode == PhotonEventCodes.SET_READY_STATUS)
        {
            object[] data = (object[])photonEvent.CustomData;

            bool isReady = (bool)data[0];

            launcher.SetReadyStatus(isReady, photonEvent.Sender);
        }
    }
}
