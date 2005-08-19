'*
'* $Id$
'* $URL$
'* $Rev$
'* $Author$
'* $Date$
'*
'* smuxi - Smart MUltipleXed Irc
'*
'* Copyright (c) 2005 Jeffrey Richardson <themann@indyfantasysports.net>
'* Copyright (c) 2005 Smuxi Project <http://smuxi.meebey.net>
'*
'* Full GPL License: <http://www.gnu.org/licenses/gpl.txt>
'*
'* This program is free software; you can redistribute it and/or modify
'* it under the terms of the GNU General Public License as published by
'* the Free Software Foundation; either version 2 of the License, or
'* (at your option) any later version.
'*
'* This program is distributed in the hope that it will be useful,
'* but WITHOUT ANY WARRANTY; without even the implied warranty of
'* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'* GNU General Public License for more details.
'*
'* You should have received a copy of the GNU General Public License
'* along with this program; if not, write to the Free Software
'* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307 USA
'*

Public Class PageForm
    Public Page As Meebey.Smuxi.Engine.Page


    Friend Sub New(ByVal Page As Meebey.Smuxi.Engine.Page)
        MyClass.New()
        Me.Page = Page
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'For Each s As String In Page.Buffer

        'Next
    End Sub

    Public Sub AddMessage(ByVal msg As Meebey.Smuxi.Engine.FormattedMessage)
        For Each item As FormattedMessageItem In msg.Items
            Select Case item.Type
                Case FormattedMessageItemType.Text
                    Dim Style As FontStyle
                    Dim TextItem As FormattedTextMessage = DirectCast(item.Value, FormattedTextMessage)
                    If TextItem.Bold Then Style = Style Or FontStyle.Bold
                    If TextItem.Underline Then Style = Style Or FontStyle.Underline
                    'PageBuffer.SelectionBackColor = Color.FromArgb(TextItem.BackgroundColor.HexCode And &HFF000000)
                    'PageBuffer.SelectionColor = Color.FromArgb(TextItem.Color.HexCode And &HFF000000)
                    PageBuffer.SelectionFont = New Font(PageBuffer.Font, Style)
                    PageBuffer.AppendText(TextItem.Text)
            End Select
        Next
        PageBuffer.AppendText(vbNewLine)
        PageBuffer.ScrollToCaret()
    End Sub

    Public Sub UpdateTopic(ByVal Topic As String)
        PageBuffer.AppendText(String.Format("Topic updated to ""{0}""", Topic))
        PageBuffer.AppendText(vbNewLine)
        PageBuffer.ScrollToCaret()
    End Sub
    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        e.Cancel = True
        Me.Hide()
        MyBase.OnClosing(e)
    End Sub

    Private Sub SendCommand(ByVal sender As Object, ByVal e As EventArgs) Handles Send.Click
        SendCommand(CommandLine.Text)
        CommandLine.Text = String.Empty
        Send.Enabled = False
    End Sub

    Private Sub EnableSend(ByVal sender As Object, ByVal e As EventArgs) Handles CommandLine.TextChanged
        If CommandLine.Text = String.Empty Then Send.Enabled = False Else Send.Enabled = True
        Send.Enabled = Not (CommandLine.Text = String.Empty)
    End Sub

    Protected Overridable Sub SendCommand(ByVal command As String)
        Dim cmd As New Meebey.Smuxi.Engine.CommandData(Globals.FManager, "/", command)
        If Not (Globals.Session.Command(cmd)) Then
            If FManager.CurrentNetworkManager IsNot Nothing Then
                If Not (FManager.CurrentNetworkManager.Command(cmd)) Then
                    PageBuffer.AppendText("Invalid command")
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnActivated(ByVal e As System.EventArgs)
        Globals.FManager.CurrentPage = Page
        'Globals.FManager.CurrentNetworkManager = Page.NetworkManager
        MyBase.OnActivated(e)
    End Sub

    Protected Overrides Sub OnDeactivate(ByVal e As System.EventArgs)
        If Me.WindowState = FormWindowState.Minimized Then Me.Hide()
        MyBase.OnDeactivate(e)
    End Sub

    Public Overridable Sub Sync()
        Return
    End Sub

End Class
