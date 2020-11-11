using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.Events;

public class LoginRegister : MonoBehaviour
{


    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI displayText;

    public UnityEvent onLoggedIn;

    [HideInInspector]
    public string playFabId;

    public static LoginRegister instance;
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

    // Start is called before the first frame update
    public void OnRegister()
    {


        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            DisplayName = usernameInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, result =>
        {
            SetDisplayText(result.PlayFabId, Color.green);
        },
        error =>
        {
            SetDisplayText(error.ErrorMessage, Color.red);
        }
        );
    }


    public void OnLoginButton()
    {
        LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest,
        result =>
        {
            SetDisplayText("Login as: " + result.PlayFabId, Color.green);
            onLoggedIn?.Invoke();
            playFabId = result.PlayFabId;
        },
        error => SetDisplayText(error.ErrorMessage, Color.red)
        );
    }


    void SetDisplayText(string text, Color color)
    {
        displayText.text = text;
        displayText.color = color;
    }
}
