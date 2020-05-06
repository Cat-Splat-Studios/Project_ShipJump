/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the share logic using the Voxel Plugin
**/

using UnityEngine;
using VoxelBusters.NativePlugins;

public class ShareScore : MonoBehaviour
{
    public GameObject[] viewObjects;
    public GameObject adButton;

    public MessageBox message;

    private bool showAdButton;

    public void ShareViaShareSheet()
    {
        // Create new instance and populate fields
        ShareSheet shareSheet = new ShareSheet();
        
        DisableView();
        shareSheet.AttachScreenShot();
        shareSheet.Text = "Here is my high score on Rocket Recover! Think you can beat it?!";
        shareSheet.URL = "https://play.google.com/store/apps/details?id=com.CatSplatStudios.RocketRecover&hl=en";
        // On iPad, popover view is used to show share sheet. So we need to set its position
        NPBinding.UI.SetPopoverPointAtLastTouchPosition();
        // Show composer
        NPBinding.Sharing.ShowView(shareSheet, OnFinishedSharing);
    }
    private void OnFinishedSharing(eShareResult result)
    {
        EnableView();
    }

    private void EnableView()
    {
        ToggleViews(true);

        adButton.SetActive(showAdButton);
    }
    
    private void DisableView()
    {
        showAdButton = adButton.activeSelf;

        ToggleViews(false);
    }

    private void ToggleViews(bool value)
    {
        foreach(GameObject view in viewObjects)
        {
            view.SetActive(value);
        }
    }
}
