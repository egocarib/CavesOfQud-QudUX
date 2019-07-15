﻿using System;
using System.Collections;
using UnityEngine;

namespace Egocarib.Code
{
    public class Egcb_UIMonitor : MonoBehaviour
    {
        [NonSerialized] public static Egcb_UIMonitor Instance;
        private static bool bInitialized;
        private string UIMode;
        private Egcb_JournalExtender JournalExtender;
        private Egcb_InventoryExtender InventoryExtender;

        public bool Initialize()
        {
            if (!Egcb_UIMonitor.bInitialized)
            {
                Egcb_UIMonitor.bInitialized = true;
                Egcb_UIMonitor.Instance = this;
                this.enabled = false;
                base.StartCoroutine(this.UIMonitorLoop());
                return true;
            }
            return false;
        }

        public static bool IsActive
        {
            get
            {
                return Egcb_UIMonitor.bInitialized;
            }
        }

        private void Update() //runs only when this.enabled = true
        {
            if (GameManager.Instance.CurrentGameView != this.UIMode)
            {
                this.enabled = false;
                this.JournalExtender = null;
                this.InventoryExtender = null;
                return;
            }

            if (this.UIMode == "Inventory")
            {
                if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt) || Input.GetKeyDown(KeyCode.AltGr))
                {
                    this.InventoryExtender.ReactToAltKeyRequest();
                }
                else
                {
                    this.InventoryExtender.FrameCheck();
                }
            }
            else if (this.UIMode == "Journal")
            {
                this.JournalExtender.FrameCheck();
            }
        }

        private IEnumerator UIMonitorLoop()
        {
            Debug.Log("QudUX Mod: UI Monitor Activated.");
            for (;;)
            {
                if (GameManager.Instance.CurrentGameView == "Inventory" || GameManager.Instance.CurrentGameView == "Journal")
                {
                    this.UIMode = GameManager.Instance.CurrentGameView;
                    //TODO: should check whether the overlay inventory option is enabled, and don't do anything if it is.
                    Debug.Log("QudUX Mod: Detected " + this.UIMode + " menu");
                    if (this.UIMode == "Journal")
                    {
                        this.JournalExtender = new Egcb_JournalExtender();
                    }
                    else if (this.UIMode == "Inventory")
                    {
                        this.InventoryExtender = new Egcb_InventoryExtender();
                    }
                    this.enabled = true;
                    do { yield return new WaitForSeconds(0.2f); } while (this.enabled == true);
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}