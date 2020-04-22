using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoxelBusters.NativePlugins;
using VoxelBusters.Utility;

public class SignInScript : MonoBehaviour
{
    public UIDelgate ui;
    public MessageBox prompt;

    bool _isServiceAvailable;
    bool _isAuthenticated;

    private bool _isOffline = false;

    public eLeaderboardTimeScope myBoardScope;

    User[] myFriends;
    // Start is called before the first frame update
    void Start()
    {
        _isServiceAvailable = NPBinding.GameServices.IsAvailable();
        SignIn();  
    }

    void SignIn()
    {
        if (_isServiceAvailable)
        {
            _isAuthenticated = NPBinding.GameServices.LocalUser.IsAuthenticated;
            if (!_isAuthenticated)
            {
                // Authenticate Local User
                NPBinding.GameServices.LocalUser.Authenticate((bool _success, string _error) => {

                    if (_success)
                    {
                        Debug.Log("Sign-In Successfully");
                        Debug.Log("Local User Details : " + NPBinding.GameServices.LocalUser.ToString());
                        _isOffline = false;
                        ui.HasAuthenitcated();
                        ui.toggleOnlineButtons(true);
                        AdManager.instance.ToggleTracking(true);
                        CloudSaving.instance.LoadGame();

                        var swapers = FindObjectsOfType<MonoBehaviour>().OfType<ISwapper>();
                        foreach (ISwapper swaps in swapers)
                        {
                            swaps.SwapIt();
                        }
                    }
                    else
                    {
                        Debug.Log("Sign-In Failed with error " + _error);

                        if (!_isOffline)
                        {
                            ui.HasAuthenitcated();
                            //SaveManager.instance.DefaultLoad();
                            CloudSaving.instance.DefaultLoad();
                            OfflineMode();
                        }
                        else
                        {
                            prompt.SetPrompt("Could Not sign In", "Authentication has failed.");
                        }
                    }
                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Call this function if we want to use the friends list features of gamecenter or google play games
    void LoadFriends()
    {
        NPBinding.GameServices.LocalUser.LoadFriends((User[] _friendsList, string _error) => {
            if (_friendsList != null)
            {
                Debug.Log("Succesfully loaded user friends.");
                myFriends = _friendsList;
            }
            else
            {
                Debug.Log("Failed to load user friends with error " + _error);
            }

        });
    }
    
    //Link to signout button - per google documentation - this is recommended
    public void SignOut()
    {
        NPBinding.GameServices.LocalUser.SignOut((bool _success, string _error) => {

            if (_success)
            {
                Debug.Log("Local user is signed out successfully!");
            }
            else
            {
                Debug.Log("Request to signout local user failed.");
                Debug.Log(string.Format("Error= {0}.", _error.GetPrintableString()));
            }
        });
    }

    private void OfflineMode()
    {
        // custom code here for when your application is offline
        if (!_isOffline)
        {
            _isOffline = true;
            // disable all online buttons
            ui.toggleOnlineButtons(false);

            // disable ads
            AdManager.instance.ToggleTracking(false);

            // display message
            prompt.SetPrompt("Could Not sign In", "All progress will not be saved.\n You can attemp to sign in again at the settings screen.");
        }

    }
    
    //show leaderboard - should be linked to a button.
    public void ShowLeaderboard(string leaderboardName)
    {
        NPBinding.GameServices.ShowLeaderboardUIWithGlobalID(leaderboardName, myBoardScope, (string _error) => {
            Debug.Log("Leaderboard view dismissed.");
            Debug.Log(string.Format("Error= {0}.", _error.GetPrintableString()));
        });
    }

    //Report the score to the leaderboard.
    public static void ReportScore(string leaderboardName, long leaderboardValue)
    {
        NPBinding.GameServices.ReportScoreWithGlobalID(leaderboardName, leaderboardValue, (bool _success, string _error) => {

            if (_success)
            {
                Debug.Log(string.Format("New score= {0}.", leaderboardValue));
            }
            else
            {
                Debug.Log(string.Format("Error= {0}.", _error.GetPrintableString()));
            }
        });
    }
}
