/*
 * $Id: GroupChatView.cs 188 2007-04-21 22:03:54Z meebey $
 * $URL: svn+ssh://svn.qnetp.net/svn/smuxi/smuxi/trunk/src/Frontend-GNOME/GroupChatView.cs $
 * $Rev: 188 $
 * $Author: meebey $
 * $Date: 2007-04-22 00:03:54 +0200 (Sun, 22 Apr 2007) $
 *
 * smuxi - Smart MUltipleXed Irc
 *
 * Copyright (c) 2008 Mirco Bauer <meebey@meebey.net>
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
using System.Globalization;
using Smuxi.Engine;
using Smuxi.Common;
using Gtk;

namespace Smuxi.Frontend.Gnome
{
    [ChatViewInfo(ChatType = ChatType.Person, ProtocolManagerType = typeof(IrcProtocolManager))]
    public class IrcPersonChatView : PersonChatView
    {
        private PersonChatModel    _personChat;
        private IrcProtocolManager _IrcProtocolManager;

        public IrcPersonChatView(PersonChatModel personChat) : base(personChat)
        {
            Trace.Call(personChat);
            _personChat = personChat;
            _IrcProtocolManager = (IrcProtocolManager)personChat.ProtocolManager;
            OutputTextView.PopulatePopup += new PopulatePopupHandler(_OnOutputTextViewPopulatePopup, IntPtr.Zero);
        }
        
        public override void Close()
        {
            Trace.Call();
            
            base.Close();
            
            // BUG: out of scope?
            Frontend.Session.RemoveChat(ChatModel);
        }

        public void _OnOutputTextViewPopulatePopup(object sender, PopulatePopupArgs a)
        {
            Menu menu = a.Menu;

            ImageMenuItem whois_item = new ImageMenuItem(_("_Whois"));
            whois_item.Activated += new EventHandler(_OnContextMenuWhois);
            menu.Append(whois_item);

            Gtk.ImageMenuItem ctcp_item = new Gtk.ImageMenuItem(_("_CTCP"));
            Gtk.Menu ctcp_submenu = new Menu();
            Gtk.ImageMenuItem ctcp_version_item = new Gtk.ImageMenuItem(_("_Version"));
            ctcp_version_item.Activated += _OnContextMenuCTCPVersionActivated;
            ctcp_submenu.Append(ctcp_version_item);

            Gtk.ImageMenuItem ctcp_ping_item = new Gtk.ImageMenuItem(_("_Ping"));
            ctcp_ping_item.Activated += _OnContextMenuCTCPPingActivated;
            ctcp_submenu.Append(ctcp_ping_item);

            Gtk.ImageMenuItem ctcp_time_item = new Gtk.ImageMenuItem(_("_Time"));
            ctcp_time_item.Activated += _OnContextMenuCTCPTimeActivated;
            ctcp_submenu.Append(ctcp_time_item);

            ctcp_item.Submenu = ctcp_submenu;
            menu.Append(ctcp_item);

            menu.ShowAll();
        }

        private void _OnContextMenuWhois(object sender, EventArgs e)
        {
            Trace.Call(sender, e);

            _IrcProtocolManager.CommandWhoIs(
                new CommandModel(
                    Frontend.FrontendManager,
                    ChatModel,
                    _personChat.Person.ID
                )
            );
        }

        private void _OnContextMenuCTCPVersionActivated(object sender, EventArgs e)
        {
            Trace.Call(sender, e);

            _IrcProtocolManager.CommandVersion(
                new CommandModel(
                    Frontend.FrontendManager,
                    ChatModel,
                    _personChat.Person.ID
                )
            );
        }

        private void _OnContextMenuCTCPPingActivated(object sender, EventArgs e)
        {
            Trace.Call(sender, e);

            _IrcProtocolManager.CommandPing(
                new CommandModel(
                    Frontend.FrontendManager,
                    ChatModel,
                    _personChat.Person.ID
                )
            );
        }

        private void _OnContextMenuCTCPTimeActivated(object sender, EventArgs e)
        {
            Trace.Call(sender, e);

            _IrcProtocolManager.CommandTime(
                new CommandModel(
                    Frontend.FrontendManager,
                    ChatModel,
                    _personChat.Person.ID
                )
            );
        }
    }
}
