using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TogglePlayerButtonTesting : MonoBehaviour
{
    public Player player;
    public Toggle buildToggle;
    public Toggle mouseToggle;
    public Toggle rotateToggle;
    public void ToggleIsBuildModeOn(){
        player.isBuildModeOn = buildToggle.isOn;
    }

    public void ToggleIsMouseModeOn(){
        player.isMouseModeOn = mouseToggle.isOn;
    }

    public void ToggleIsRotateModeOn(){
        player.isRotateModeOn = rotateToggle.isOn;
    }
}
