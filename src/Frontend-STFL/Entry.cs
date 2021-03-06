/*
 * $Id: MainWindow.cs 192 2007-04-22 11:48:12Z meebey $
 * $URL: svn+ssh://svn.qnetp.net/svn/smuxi/smuxi/trunk/src/Frontend-GNOME/MainWindow.cs $
 * $Rev: 192 $
 * $Author: meebey $
 * $Date: 2007-04-22 13:48:12 +0200 (Sun, 22 Apr 2007) $
 *
 * smuxi - Smart MUltipleXed Irc
 *
 * Copyright (c) 2007 Mirco Bauer <meebey@meebey.net>
 *
 * Full GPL License: <http://www.gnu.org/licenses/gpl.txt>
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307 USA
 */

using System;
using System.IO;
using System.Reflection;
using Mono.Unix;
//using Stfl;
using Smuxi.Common;
using Smuxi.Engine;

namespace Smuxi.Frontend.Stfl
{
    public class Entry
    {
#if LOG4NET
        private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif
        private MainWindow _MainWindow;
        
        public event EventHandler Activated;
        
        public string Text {
            get {
                return _MainWindow["input_text"];
            }
            set {
                _MainWindow["input_text"] = value;
            }
        }
        
        public Entry(MainWindow mainWindow)
        {
            _MainWindow = mainWindow;
            _MainWindow.KeyPressed += new KeyPressedEventHandler(_OnKeyPressed);
        }
        
        private void _OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            Trace.Call(sender, e);
            
#if LOG4NET
            _Logger.Debug("_OnKeyPressed(): e.Key: " + e.Key + " e.Focus: " + e.Focus);
#endif
            switch (e.Key) {
                case "ENTER":
                    OnActivated(EventArgs.Empty);
                    break;
                case "NPAGE":
                    _MainWindow.ChatViewManager.ActiveChat.ScrollToEnd();
                    break;
            } 
        }

        public virtual void OnActivated(EventArgs e)
        {
            ExecuteCommand(Text);
            
            if (Activated != null) {
                Activated(this, EventArgs.Empty);
            }

            Text = String.Empty;
        }
        
        public void ExecuteCommand(string cmd)
        {
            if (!(cmd.Length > 0)) {
                return;
            }
            
            bool handled = false;
            CommandModel cd = new CommandModel(Frontend.FrontendManager,
                                               _MainWindow.ChatViewManager.ActiveChat.ChatModel,
                                               (string)Frontend.UserConfig["Interface/Entry/CommandCharacter"],
                                               cmd);
            //handled = _Command(cd);
            if (!handled) {
                handled = Frontend.Session.Command(cd);
            }
            if (!handled) {
                // we may have no network manager yet
                Engine.IProtocolManager nm = Frontend.FrontendManager.CurrentProtocolManager;
                if (nm != null) {
                    handled = nm.Command(cd);
                } else {
                    handled = false;
                }
            }
            if (!handled) {
               _CommandUnknown(cd);
            }
        }

        private void _CommandUnknown(CommandModel cd)
        {
            cd.FrontendManager.AddTextToCurrentChat("-!- " +
                                String.Format(Catalog.GetString(
                                              "Unknown Command: {0}"),
                                              cd.Command));
        }
    }
}
