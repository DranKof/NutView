﻿Public Class FormProgress

    Public MyTask As Integer

    Public Sub New(aTask As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MyTask = aTask
    End Sub

    Private Sub FormFileProgress_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MainWindow.Enabled = False
        Select Case MyTask
            Case FileTask.Load
                Label1.Text = "Loading...this should be fast..."
            Case FileTask.Save
                Label1.Text = "Saving...this may take a while..."
        End Select
    End Sub

    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOK.Click
        MainWindow.Enabled = True
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Select Case MyTask
            Case FileTask.Idle
                Timer1.Enabled = False
                BtnOK.Enabled = True
                BtnOK.Text = "CRAZY!"
            Case FileTask.Load
                ' Not done yet.
                'ProgressBar1.Value = ContinueLoading()
                If ProgressBar1.Value >= 100 Then
                    BtnOK.Enabled = True
                    BtnOK.Text = "Great!"
                    Timer1.Enabled = False
                End If
            Case FileTask.Save
                ProgressBar1.Value = ContinueSaving()
                If ProgressBar1.Value >= 100 Then
                    BtnOK.Enabled = True
                    BtnOK.Text = "Great!"
                    Timer1.Enabled = False
                End If
        End Select
    End Sub
End Class