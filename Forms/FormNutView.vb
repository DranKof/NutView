﻿Public Class FormNutView

    Private ShownPorts As New SortedSet(Of Integer)

    Private Enum CT
        CustomName
        IpAddress
        MacAddress
        Hardware
        Hostname
        Ping
        Ports
    End Enum

    Private Sub FormNutView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Darren's NutView " & ProgVersion
        RedoColumns()
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Select Case OpenFileDialog1.ShowDialog()
            Case DialogResult.Cancel, DialogResult.Abort
                ' Do Nothing
            Case Else
                Try
                    ParseFile(OpenFileDialog1.FileName)
                Catch ex As Exception
                    MsgBox("Error loading/parsing file:" & vbNewLine & ex.Message)
                End Try
                RedoColumns()
        End Select
    End Sub

    Private Sub ResetHosts()
        DataDisplay.Rows.Clear()
        For Each aHost As ClsHost In KnownHosts
            DataDisplay.Rows.Add()
            Dim jRow As Integer = DataDisplay.Rows.Count - 1 ' (-1 header & -1 adder)
            DataDisplay.Rows(jRow).Cells(CT.CustomName).Value = aHost.CustomName
            DataDisplay.Rows(jRow).Cells(CT.IpAddress).Value = aHost.IP
            DataDisplay.Rows(jRow).Cells(CT.MacAddress).Value = aHost.MacAddress
            DataDisplay.Rows(jRow).Cells(CT.Hardware).Value = aHost.Hardware
            DataDisplay.Rows(jRow).Cells(CT.Hostname).Value = aHost.HostName
            DataDisplay.Rows(jRow).Cells(CT.Ping).Style.BackColor = GetColorFromValue(aHost.Ping.Value)
            If aHost.Ping.Value > 0 Then DataDisplay.Rows(jRow).Cells(CT.Ping).Value = "!"
            For intA As Integer = 0 To ShownPorts.Count - 1
                DataDisplay.Rows(jRow).Cells(CT.Ports + intA).Style.BackColor = GetColorFromValue(aHost.Tcp.Value(ShownPorts(intA)))
                If aHost.Tcp.Value(ShownPorts(intA)) > 0 Then DataDisplay.Rows(jRow).Cells(CT.Ports + intA).Value = "!"
            Next
        Next
    End Sub

    Private Function GetColorFromValue(aVal As Single) As Color
        Select Case aVal
            Case Is >= 1
                Return Color.FromArgb(128, 255, 128)
            Case Is >= 0.9
                Return Color.FromArgb(0, 205, 0) ' GREEN
            Case Is >= 0.8
                Return Color.FromArgb(85, 205, 0)
            Case Is >= 0.7
                Return Color.FromArgb(171, 205, 0)
            Case Is >= 0.6
                Return Color.FromArgb(255, 205, 0) ' YELLOW
            Case Is >= 0.5
                Return Color.FromArgb(255, 171, 0)
            Case Is >= 0.4
                Return Color.FromArgb(255, 85, 0)
            Case Is >= 0.3
                Return Color.FromArgb(255, 0, 0) ' RED
            Case Is >= 0.2
                Return Color.FromArgb(171, 0, 0)
            Case Is >= 0.1
                Return Color.FromArgb(85, 0, 0)
            Case Is < 0.1
                Return Color.FromArgb(0, 0, 0)
        End Select
    End Function

    Private Sub ChkAutoPort_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAutoPort.CheckedChanged
        Select Case ChkAutoPort.Checked
            Case True
                TxtPorts.Enabled = False
            Case False
                TxtPorts.Enabled = True
        End Select
        RedoColumns()
    End Sub

    Private Sub RedoColumns()
        Do Until DataDisplay.Columns.Count <= 4
            DataDisplay.Columns.Remove(DataDisplay.Columns(4))
        Loop
        If ChkAutoPort.Checked Then
            ShownPorts.Clear()
            For Each aHost In KnownHosts
                For Each aPort In aHost.Tcp.OpenPorts
                    ShownPorts.Add(aPort)
                Next
            Next
        Else ' Do the port list
            ShownPorts.Clear()
            Dim strPorts() As String = Split(TxtPorts.Text, " ")
            For intA As Integer = 0 To strPorts.Count - 1
                ShownPorts.Add(Val(strPorts(intA)))
            Next
        End If
        For Each iPort As Integer In ShownPorts
            DataDisplay.Columns.Add("Column" & DataDisplay.Columns.Count + 1, iPort)
            DataDisplay.Columns(DataDisplay.Columns.Count - 1).Width = 24
        Next
        ResetHosts()
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        AllHosts.Clear()
        KnownHosts.Clear()
        EmptyHosts.Clear()
        RedoColumns()
    End Sub
End Class
