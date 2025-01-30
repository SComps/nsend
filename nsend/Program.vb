Imports System.Net.Sockets
Imports System.IO
Imports System.Threading
Imports System.Text

Module nsend
    Sub Main(args As String())
        ' Ensure correct number of arguments
        If args.Length <> 2 Then
            Console.WriteLine("Usage: nsend <hostname> <port>")
            Console.WriteLine("redirect your input file with < filename")
            Console.WriteLine(vbCrLf & "Special thanks to Rene Ferland.  For everything you do." & vbCrLf)
        End If

        Dim hostname As String = args(0)
        Dim port As Integer

        ' Validate port number
        If Not Integer.TryParse(args(1), port) OrElse port < 1 OrElse port > 65535 Then
            Console.WriteLine("Invalid port number.")
            Return
        End If

        Try
            ' Read all input from standard input
            Dim input As String = Console.In.ReadToEnd()
            ' Connect to the specified host and port
            Using client As New TcpClient(hostname, port)
                Using stream As NetworkStream = client.GetStream()
                    Using writer As New StreamWriter(stream, Encoding.ASCII) With {.AutoFlush = True},
                          reader As New StreamReader(stream, Encoding.ASCII)
                        ' Send input to the server
                        writer.Write(input)
                        ' Read and display response from the server with timeout
                        Dim buffer As Char() = New Char(1023) {}
                        Dim response As New StringBuilder()
                        Dim lastReadTime As DateTime = DateTime.Now

                        While True
                            If stream.DataAvailable Then
                                Dim bytesRead As Integer = reader.Read(buffer, 0, buffer.Length)
                                If bytesRead > 0 Then
                                    response.Append(buffer, 0, bytesRead)
                                    lastReadTime = DateTime.Now ' Reset timeout timer
                                End If
                            End If

                            ' Exit loop if no data received for 1 second
                            If (DateTime.Now - lastReadTime).TotalSeconds > 1 Then
                                Exit While
                            End If

                            Thread.Sleep(100) ' Small delay to prevent excessive CPU usage
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try
    End Sub
End Module
