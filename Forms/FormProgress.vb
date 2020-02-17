﻿Imports System.ComponentModel

Public Class FormProgress

    Public MyTask As Integer
    Private StartTime As DateTime

    Public Sub New(aTask As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MyTask = aTask
    End Sub

    Private Sub FormFileProgress_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartTime = DateTime.Now
        MainWindow.Enabled = False
        Select Case MyTask
            Case FileTask.Test
                Me.Enabled = False
                Me.Text = "Testing!"
                Label1.Text = "Testing...this should be super fast..."
            Case FileTask.Import
                Me.Enabled = False
                Me.Text = "Importing!"
                Label1.Text = "Importing...this should be fast..."
            Case FileTask.Save
                Me.Enabled = False
                Me.Text = "Saving!"
                Label1.Text = "Saving...this may take a while..."
        End Select
    End Sub

    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOK.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Select Case MyTask
            Case FileTask.Idle
                Timer1.Enabled = False
                BtnOK.Enabled = True
                BtnOK.Text = "CRAZY!"
            Case FileTask.Test
                ProgressBar1.Value += 1
                If ProgressBar1.Value >= 100 Then
                    ShowCompletion("Test")
                End If
            Case FileTask.Import
                ProgressBar1.Value = ContinueLoading()
                If ProgressBar1.Value >= 100 Then
                    ShowCompletion("Import")
                End If
            Case FileTask.Save
                ProgressBar1.Value = ContinueSaving()
                If ProgressBar1.Value >= 100 Then
                    ShowCompletion("Save")
                End If
        End Select
    End Sub

    Private Sub ShowCompletion(WhatDone As String)
        Me.Enabled = True
        Me.Text = WhatDone & " Complete!"
        Dim ts As TimeSpan = DateTime.Now - StartTime
        Label1.Text = WhatDone & " Complete! Time Elapsed: " & ts.TotalSeconds & " s"
        BtnOK.Enabled = True
        BtnOK.Text = "Great!"
        Timer1.Enabled = False
    End Sub

    Private Sub FormProgress_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        MainWindow.Enabled = True
    End Sub

    Private Sub FormProgress_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Me.Dispose()
    End Sub
End Class