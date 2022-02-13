/**
 * Copyright (c) 2020 Aleksej Komarov
 * SPDX-License-Identifier: MIT
 * 
 * Original code in DM, code has been converted to C#
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tgui.Code
{
    public class TguiWindow
    {

        public enum TguiWindowState
        {
            TGUI_WINDOW_CLOSED = 0,
            TGUI_WINDOW_LOADING = 1,
            TGUI_WINDOW_READY = 2,
        }

        public string ID { get; }
        public bool Pooled { get; private set; }
        public int PoolIndex { get; private set; }
        public bool IsBrowser { get; }
        public TguiWindowState Status { get; set; }
        public bool Locked { get; private set; }
        public Tgui LockedBy { get; private set; }

        public bool FatallyErrored { get; private set; } = false;

        // Variables passed to initialize method (and saved for later)
        public string[] InlineAssets { get; private set; }

        private TcpListener windowWebListener;
        private int port = 5050;

        private Queue<string> messageQueue;

        public TguiWindow(string id, bool pooled = false)
        {
            ID = id;
            Pooled = pooled;
            if (pooled)
                PoolIndex = int.Parse(id.Substring(13));
        }

        /**
         * public
         *
         * Initializes the window with a fresh page. Puts window into the "loading"
         * state. You can begin sending messages right after initializing. Messages
         * will be put into the queue until the window finishes loading.
         *
         * optional inline_assets list List of assets to inline into the html.
         * optional inline_html string Custom HTML to inject.
         * optional fancy bool If TRUE, will hide the window titlebar.
         */
        public void Initialize(string[] inline_assets = null, string inline_html = "")
        {
            Log.WriteLine($"{ID}/initialize", LogType.DEBUG);
            InlineAssets = inline_assets ?? new string[0];
            Status = TguiWindowState.TGUI_WINDOW_LOADING;
            FatallyErrored = false;
            // Generate page html
            string html = TguiSubsystem.Singleton.BaseHtml;
            html = html.Replace("[tgui:windowId]", ID);
            // Inject custom HTML
            html = html.Replace("<!-- tgui:html -->\n", inline_html);
            //Open the window
            TguiSubsystem.Singleton.WebServer.Browse(ID, html);
        }

        /**
         * public
         *
         * Checks if the window is ready to receive data.
         *
         * return bool
         */
        public bool IsReady => Status == TguiWindowState.TGUI_WINDOW_READY;

        /**
         * public
         *
         * Checks if the window can be sanely suspended.
         *
         * return bool
         */
        public bool CanBeSuspended =>
            !FatallyErrored
            && Pooled
            && PoolIndex > 0
            && PoolIndex <= TguiSubsystem.TGUI_WINDOW_SOFT_LIMIT
            && Status == TguiWindowState.TGUI_WINDOW_READY;

        /**
         * public
         *
         * Acquire the window lock. Pool will not be able to provide this window
         * to other UIs for the duration of the lock.
         *
         * Can be given an optional tgui datum, which will be automatically
         * subscribed to incoming messages via the on_message proc.
         *
         * optional ui /datum/tgui
         */
        public void AcquireLock(Tgui tgui)
        {
            Locked = true;
            LockedBy = tgui;
        }

        /**
         * public
         *
         * Release the window lock.
         */
        public void ReleaseLock()
        {
            //if(Locked) SentAssets = new List<??>();
            Locked = false;
            LockedBy = null;
        }

        public void SendMessage(string type, object[] payload, bool force)
        {
            string message = TguiSubsystem.TguiCreateMessage(type, payload);
            //Place it into the queue if window is still loading
            if (!force && Status != TguiWindowState.TGUI_WINDOW_READY)
            {
                if (messageQueue == null)
                    messageQueue = new Queue<string>();
                messageQueue.Enqueue(message);
                return;
            }
            //Send the message

        }

    }
}
