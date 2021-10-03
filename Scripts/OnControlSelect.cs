using UnityEngine;
using System.Collections;

public class OnControlSelect : MonoBehaviour 
{
    private ControlSelector selector;
    private InGameManager game;
    private UIControl ui;
    private TimeControl timeControl;

    public float controlTime;

	void Start () 
    {
        selector = FindObjectOfType<ControlSelector>();
        ui = FindObjectOfType<UIControl>();
        game = FindObjectOfType<InGameManager>();
        timeControl = FindObjectOfType<TimeControl>();
	}
	
	void Update () 
    {
	    
	}

    private void ControlSelected()
    {
        game.controlIndex = 0;
        selector.OnSelectorClosed();
        ui.ClearControlSprites();
        game.InitialControlInstances("controlSelector");
    }

    public void Fast()
    {
        print("Fast control activated");
        PlayerDataControl.data.uiControlDict["fast"]--;
        ControlSelected();
        timeControl.Fast();
    }

    public void Pause()
    {
        print("Pause control activated");
        PlayerDataControl.data.uiControlDict["pause"]--;
        ControlSelected();
        timeControl.Pause();
    }

    public void Record()
    {
        print("Record control activated");
        PlayerDataControl.data.uiControlDict["record"]--;
        ControlSelected();
        timeControl.Record();
    }

    public void Rewind()
    {
        print("Rewind control activated");
        selector.OnSelectorClosed();
        timeControl.Rewind("selected");
    }

    public void Slow()
    {
        print("Slow control activated");
        PlayerDataControl.data.uiControlDict["slow"]--;
        ControlSelected();
        timeControl.Slow();
    }
}
