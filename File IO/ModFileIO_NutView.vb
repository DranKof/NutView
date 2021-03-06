﻿Partial Module ModFileIO

    Private Sub NutViewImport(iPart() As String)
        Select Case LCase(iPart(0))
            Case "host"
                ' main process
                Dim aHost As New ClsHost
                Dim intA As Integer = 1

                Do
                    Select Case LCase(iPart(intA))
                        Case "nutview"
                            If iPart.Length > 1 Then
                                intA += 1
                                FileIoDateTime = CDate(iPart(intA))
                            End If
                        Case "custom name"
                            intA += 1
                            aHost.CustomName = iPart(intA)
                        Case "ip"
                            intA += 1
                            aHost.IP = iPart(intA)
                        Case "past ips"
                            intA += 1
                            aHost.PastIPs = iPart(intA)
                        Case "ip date"
                            intA += 1
                            aHost.IP_Date = CDate(iPart(intA))
                        Case "mac"
                            intA += 1
                            aHost.MacAddress = iPart(intA)
                        Case "manufacturer"
                            intA += 1
                            aHost.Manufacturer = iPart(intA)
                        Case "hostname"
                            intA += 1
                            aHost.HostName = iPart(intA)
                        Case "+ping"
                            intA += 1
                            aHost.Ping.Add(True, CDate(iPart(intA)))
                        Case "-ping"
                            intA += 1
                            aHost.Ping.Add(False, CDate(iPart(intA)))
                        Case "+tcp"
                            intA += 2
                            aHost.Tcp.Add(True, iPart(intA - 1), CDate(iPart(intA)))
                        Case "-tcp"
                            intA += 2
                            aHost.Tcp.Add(False, iPart(intA - 1), CDate(iPart(intA)))
                        Case "+udp"

                        Case "-udp"

                        Case "comment"
                            intA += 1
                            aHost.Comments.Add(iPart(intA))
                        Case Else
                            If iPart(intA).Contains(":") Then
                                Dim getVals() As String = Split(iPart(intA), ":")
                                Select Case LCase(getVals(0))
                                    Case "ping"
                                        aHost.Ping.SetVal(getVals(1), FileIoDateTime)
                                    Case "tcp"
                                        aHost.Tcp.SetVal(getVals(1), getVals(2), FileIoDateTime)
                                    Case "udp"
                                        'aHost.Udp.SetVal(getVals(1), getVals(2), FileIoDateTime)
                                End Select
                            End If
                    End Select
                    intA += 1
                Loop Until intA >= iPart.Count

                'Dim bHost As ClsHost = Nothing
                'If aHost.MacAddress <> "" Then
                'bHost = AllHosts.Find(Function(p) p.MacAddress = aHost.MacAddress And p.IP = aHost.IP)
                'ElseIf aHost.MacAddress = "" Then
                'bHost = AllHosts.Find(Function(p) p.IP = aHost.IP)
                'End If

                'If bHost Is Nothing Then
                AllHosts.Add(aHost)
                'Else
                '                MergeHosts(aHost, bHost)
                'End If

            Case Else
                'ignore
        End Select
    End Sub

    Private Sub MergeHosts(aHost As ClsHost, bHost As ClsHost)
        ' bhost is already in allhosts
        CheckMergeStrings(aHost.CustomName, bHost.CustomName)
        CheckMergeStrings(aHost.IP, bHost.IP)
        CheckMergeStrings(aHost.MacAddress, bHost.MacAddress)
        CheckMergeStrings(aHost.Manufacturer, bHost.Manufacturer)
        CheckMergeStrings(aHost.HostName, bHost.HostName)
        For intA As Integer = 0 To aHost.Ping.Hits.Count - 1
            If bHost.Ping.Times.Contains(aHost.Ping.Times(intA)) Then
                ' SOMETHING MIGHT BE MESSED UP -- BUT IGNORE FOR NOW
            Else
                Dim NewMax As Integer = bHost.Ping.Hits.Count
                ReDim bHost.Ping.Hits(NewMax), bHost.Ping.Times(NewMax)
                bHost.Ping.Hits(NewMax) = aHost.Ping.Hits(intA)
                bHost.Ping.Times(NewMax) = aHost.Ping.Times(intA)
            End If
        Next
        For intA As Integer = 0 To aHost.Tcp.Hits.Count - 1
            If bHost.Tcp.Times.Contains(aHost.Tcp.Times(intA)) Then
                ' SOMETHING MIGHT BE MESSED UP -- BUT IGNORE FOR NOW
            Else
                Dim NewMax As Integer = bHost.Tcp.Hits.Count
                ReDim bHost.Tcp.Hits(NewMax), bHost.Tcp.Ports(NewMax), bHost.Tcp.Times(NewMax)
                bHost.Tcp.Hits(NewMax) = aHost.Tcp.Hits(intA)
                bHost.Tcp.Ports(NewMax) = aHost.Tcp.Ports(intA)
                bHost.Tcp.Times(NewMax) = aHost.Tcp.Times(intA)
            End If
        Next
        For Each iComment As String In aHost.Comments
            bHost.Comments.Add(iComment) ' SortedSets will automatically delete redundant entries
        Next
    End Sub

    Private Sub CheckMergeStrings(ByRef StringAdd As String, ByRef StringTgt As String)
        If StringTgt <> StringAdd Then If Not StringTgt.Contains(StringAdd) Then StringTgt &= " & " & StringAdd
    End Sub

End Module
