using UnityEngine;
using Chronos;
using System.Collections;
using System.Collections.Generic;

public class TimeControl : BaseBehaviour
{
    public GameObject uiCanvas;
    private OnControlSelect controlSelect;
    private InGameManager game;
    private Sweeper sweeper;
    private UIControl ui;
    private ControlActive active;

    public bool isTimeControlled = false;
    private int previousRewinds;

    void Start()
    {
        controlSelect = FindObjectOfType<OnControlSelect>();
        game = FindObjectOfType<InGameManager>();
        sweeper = FindObjectOfType<Sweeper>();
        ui = FindObjectOfType<UIControl>();
        active = FindObjectOfType<ControlActive>();

        Timekeeper.instance.Clock("Root").localTimeScale = 1f;
    }

    public void Pause()
    {
        Clock clock = Timekeeper.instance.Clock("All Other Gameobjects");
        clock.localTimeScale = 0;
        isTimeControlled = true;
        Color color = new Color(0f, 0.55f, 1f, 0.3f);
        active.Display(color, controlSelect.controlTime, "pause");
        StartCoroutine(Resume(clock, controlSelect.controlTime));
    }

    public void Rewind(string context)
    {
        game.rewinding = true;
        Clock clock = Timekeeper.instance.Clock("Root");
        clock.localTimeScale = -1f;
        isTimeControlled = true;
        Color color = new Color(0.12f, 1f, 0f, 0.3f);
        uiCanvas.SetActive(false);

        previousRewinds = PlayerDataControl.data.uiControlDict["rewind"];

        if(time.time < controlSelect.controlTime)
        {
            StartCoroutine(ResumeRewind(clock, time.time, context));
            active.Display(color, time.time, "rewind");
        }
        else
        {
            StartCoroutine(ResumeRewind(clock, controlSelect.controlTime, context));
            active.Display(color, controlSelect.controlTime, "rewind");
        }
    }

    public void Slow()
    {
        Clock clock = Timekeeper.instance.Clock("All Other Gameobjects");
        clock.localTimeScale = 0.2f;
        isTimeControlled = true;
        Color color = new Color(1f, 0.81f, 0f, 0.3f);
        active.Display(color, controlSelect.controlTime, "slow");
        StartCoroutine(Resume(clock, controlSelect.controlTime));
    }

    public void Fast()
    {
        Clock clock = Timekeeper.instance.Clock("Root");
        clock.localTimeScale = 2f;
        isTimeControlled = true;
        Color color = new Color(0.36f, 0f, 1f, 0.3f);
        active.Display(color, controlSelect.controlTime, "fast");
        StartCoroutine(Resume(clock, controlSelect.controlTime));
    }

    public void Record()
    {
        Clock clock = Timekeeper.instance.Clock("Root");
        print("Recording...");
        isTimeControlled = true;
        Color color = new Color(1f, 0f, 0.06f, 0.3f);
        active.Display(color, controlSelect.controlTime, "record");
        StartCoroutine(Resume(clock, controlSelect.controlTime));
    }

    IEnumerator Resume(Clock clock, float time)
    {
        yield return new WaitForSeconds(time);
        clock.localTimeScale = 1f;
        isTimeControlled = false;
    }

    IEnumerator ResumeRewind(Clock clock, float time, string context)
    {
        yield return new WaitForSeconds(time);
        clock.localTimeScale = 1f;
        isTimeControlled = false;
        game.rewinding = false;
        game.controlIndex = 0;
        if (PlayerDataControl.data.uiControlDict["rewind"] == previousRewinds && context != "died")
        {
            PlayerDataControl.data.uiControlDict["rewind"]--;
        }
        uiCanvas.SetActive(true);
        ui.ClearControlSprites();
        game.InitialControlInstances("controlSelector");
        ui.DestroyCrystalsFast();
        game.CrystalInstances();
        ui.ClearTrashUI();
        sweeper.TrashInstances();
        ui.DestroyResets();
        ui.InstantiateResets("refresh");
    }
}
