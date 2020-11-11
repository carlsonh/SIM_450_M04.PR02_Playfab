using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayerInfo : MonoBehaviour
{
    [HideInInspector]
    public PlayerProfileModel profile;

    public static PlayerInfo instance;
    void Awake()
    {
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnLoggedIn()
    {
        GetPlayerProfileRequest getProfileRequest = new GetPlayerProfileRequest
        {
            PlayFabId = LoginRegister.instance.playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true

            }
        };

        PlayFabClientAPI.GetPlayerProfile(getProfileRequest,
        result =>
        {
            profile = result.PlayerProfile;
            Debug.Log("Loaded player: " + profile.DisplayName);
        },
        error => Debug.Log(error.ErrorMessage)
        );
    }

}
