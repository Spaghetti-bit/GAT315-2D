using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelSelect : MonoBehaviour
{
    [System.Serializable]
    public struct PanelInfo
    {
        public GameObject panel;
        public Button button;
        public KeyCode shortcut;
    }

    public KeyCode toggleKey;
    public GameObject masterPanel;
    public PanelInfo[] panels;

    public void SetPanelActive(PanelInfo panelInfo)
    {
        //panelInfo.panel.SetActive(true);

        foreach (PanelInfo pInfo in panels)
        {
            bool active = (pInfo.Equals(panelInfo));
            pInfo.panel.SetActive(active);
        }

        //for (int i = 0; i < panels.Length; i++)
        //{
        //    bool active = (panelInfo.Equals(panels[i])) ? true : false;
        //    panels[i].panel.SetActive(active);
        //}
    }

    private void Start()
    {
        foreach (PanelInfo pInfo in panels)
        {
            pInfo.button.onClick.AddListener(delegate { ButtonEvent(pInfo); });
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(toggleKey))
        {
            masterPanel.SetActive(!masterPanel.activeSelf);
        }



        foreach (PanelInfo pInfo in panels)
        {
            if (Input.GetKeyDown(pInfo.shortcut))
            {
                SetPanelActive(pInfo);
            }

            if (pInfo.panel.active)
            {
                pInfo.button.gameObject.SetActive(false);
            }
            else
            {
                pInfo.button.gameObject.SetActive(true);
            }
        }
    }

    void ButtonEvent(PanelInfo pInfo)
    {
        SetPanelActive(pInfo);
    }
}
