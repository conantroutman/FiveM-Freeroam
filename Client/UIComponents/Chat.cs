using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UIComponents
{
    class Chat
    {
        private Scaleform scaleform;

        private bool isTyping = false;
        private bool isChatOpen = false;

        public Chat()
        {
            Init();
        }

        private void Init()
        {
            scaleform = new Scaleform("MULTIPLAYER_CHAT");
        }

        private void SendMessage()
        {
            
            CloseChat();
        }

        private void AddMessage()
        {

        }

        private void BeginType()
        {

        }

        private void OpenChat()
        {
            isChatOpen = true;
            scaleform.CallFunction("SET_FOCUS", 2, "", "all");
            API.SetNuiFocus(true, false);
            API.SendNuiMessage("CHAT_OPENED");
        }

        private void CloseChat()
        {
            isChatOpen = false;
            scaleform.CallFunction("SET_FOCUS", 1);
            API.SetNuiFocus(false, false);
        }

        public async void Update()
        {
            scaleform.Render2D();
            if (API.IsControlJustPressed(0, (int)Control.MpTextChatAll))
            {
                if (!isChatOpen)
                {
                    OpenChat();
                } else
                {
                    CloseChat();
                }
            }

            if (isChatOpen)
            {
                if (API.IsControlJustPressed(0, (int)Control.Enter))
                {
                    SendMessage();
                }
            }
        }

        public void RegisterNuiCallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            API.RegisterNuiCallbackType(msg);
        }
    }
}
