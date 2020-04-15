using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;
using VoxelBusters.Utility;

public class SignInScript : MonoBehaviour
{
    bool _isServiceAvailable;
    bool _isAuthenticated;

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
                    }
                    else
                    {
                        Debug.Log("Sign-In Failed with error " + _error);
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
}
