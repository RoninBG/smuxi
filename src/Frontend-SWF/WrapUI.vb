Friend Class WrapUI
    Inherits Meebey.Smuxi.Engine.PermanentRemoteObject
    Implements Meebey.Smuxi.Engine.IFrontendUI

    Private UI As Meebey.Smuxi.Engine.IFrontendUI
    Friend Delegate Function InvokeMethod(ByVal method As System.Delegate, ByVal args() As Object) As Object
    Private Invoke As InvokeMethod

    Private Delegate Sub ManagePage(ByVal page As Meebey.Smuxi.Engine.Page)
    Private Delegate Sub AddPageText(ByVal page As Meebey.Smuxi.Engine.Page, ByVal text As String)
    Private Delegate Sub ManageUser(ByVal cpage As Meebey.Smuxi.Engine.ChannelPage, ByVal user As Meebey.Smuxi.Engine.User)
    Private Delegate Sub StatusText(ByVal text As String)
    Private Delegate Sub UpdateTopic(ByVal cpage As Meebey.Smuxi.Engine.ChannelPage, ByVal topic As String)
    Private Delegate Sub UpdateUser(ByVal cpage As Meebey.Smuxi.Engine.ChannelPage, ByVal olduser As Meebey.Smuxi.Engine.User, ByVal newuser As Meebey.Smuxi.Engine.User)


    Public Sub AddPage(ByVal page As Meebey.Smuxi.Engine.Page) Implements Meebey.Smuxi.Engine.IFrontendUI.AddPage
        Invoke(New ManagePage(AddressOf UI.AddPage), New Object() {page})
    End Sub

    Public Sub AddTextToPage(ByVal page As Meebey.Smuxi.Engine.Page, ByVal text As String) Implements Meebey.Smuxi.Engine.IFrontendUI.AddTextToPage
        Invoke(New AddPageText(AddressOf UI.AddTextToPage), New Object() {page, text})
    End Sub

    Public Sub AddUserToChannel(ByVal cpage As Meebey.Smuxi.Engine.ChannelPage, ByVal user As Meebey.Smuxi.Engine.User) Implements Meebey.Smuxi.Engine.IFrontendUI.AddUserToChannel
        Invoke(New ManageUser(AddressOf UI.AddUserToChannel), New Object() {cpage, user})
    End Sub

    Public Sub RemovePage(ByVal page As Meebey.Smuxi.Engine.Page) Implements Meebey.Smuxi.Engine.IFrontendUI.RemovePage
        Invoke(New ManagePage(AddressOf UI.RemovePage), New Object() {page})
    End Sub

    Public Sub RemoveUserFromChannel(ByVal cpage As Meebey.Smuxi.Engine.ChannelPage, ByVal user As Meebey.Smuxi.Engine.User) Implements Meebey.Smuxi.Engine.IFrontendUI.RemoveUserFromChannel
        Invoke(New ManageUser(AddressOf UI.RemoveUserFromChannel), New Object() {cpage, user})
    End Sub

    Public Sub SetNetworkStatus(ByVal status As String) Implements Meebey.Smuxi.Engine.IFrontendUI.SetNetworkStatus
        Invoke(New StatusText(AddressOf UI.SetNetworkStatus), New Object() {status})
    End Sub

    Public Sub SetStatus(ByVal status As String) Implements Meebey.Smuxi.Engine.IFrontendUI.SetStatus
        Invoke(New StatusText(AddressOf UI.SetStatus), New Object() {status})
    End Sub

    Public Sub SyncPage(ByVal page As Meebey.Smuxi.Engine.Page) Implements Meebey.Smuxi.Engine.IFrontendUI.SyncPage
        Invoke(New ManagePage(AddressOf UI.SyncPage), New Object() {page})
    End Sub

    Public Sub UpdateTopicInChannel(ByVal cpage As Meebey.Smuxi.Engine.ChannelPage, ByVal topic As String) Implements Meebey.Smuxi.Engine.IFrontendUI.UpdateTopicInChannel
        Invoke(New UpdateTopic(AddressOf UI.UpdateTopicInChannel), New Object() {cpage, topic})
    End Sub

    Public Sub UpdateUserInChannel(ByVal cpage As Meebey.Smuxi.Engine.ChannelPage, ByVal olduser As Meebey.Smuxi.Engine.User, ByVal newuser As Meebey.Smuxi.Engine.User) Implements Meebey.Smuxi.Engine.IFrontendUI.UpdateUserInChannel
        Invoke(New UpdateUser(AddressOf UI.UpdateUserInChannel), New Object() {cpage, olduser, newuser})
    End Sub

    Public ReadOnly Property Version() As Integer Implements Meebey.Smuxi.Engine.IFrontendUI.Version
        Get
            Return UI.Version
        End Get
    End Property

    Public Sub New(ByVal UI As Meebey.Smuxi.Engine.IFrontendUI, ByVal Invoke As InvokeMethod)
        Me.UI = UI
        Me.Invoke = Invoke
    End Sub
End Class
