Imports System.IO
Imports System.Threading
Imports System.Data.SqlClient

Public Class labelSvc : Inherits myService

    Private cnstring As String = "Integrated Security=true;Initial Catalog=b060413;Server=LOCALHOST\PRI"

#Region "Override Service Methods"

    Overrides Sub svcStart(ByVal args As Dictionary(Of String, String))

        Log("Starting...")
        With New Thread(AddressOf hinit)
            .Start()
        End With

    End Sub

#End Region

#Region "Threads"

    Private Sub hinit()

        Do
            Try
                Using cn As New SqlConnection(cnstring)
                    cn.Open()
                    Using cmd As New SqlCommand( _
                        "SELECT DOC from dbo.ZROD_PRINTABLELABELS() ", _
                        cn _
                    )
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Print(reader(0))

                            End While

                        End Using

                    End Using

                End Using

            Catch ex As Exception
                Log(ex.Message)

            Finally
                Thread.Sleep(2000)

            End Try

        Loop

    End Sub

    Private Sub Print(ByVal id As Integer)

        Using cn As New SqlConnection(cnstring)
            cn.Open()
            Using print As New SqlCommand( _
                String.Format( _
                    "exec dbo.sp_rmPrint {0}", _
                    id.ToString _
                ) _
                , cn _
            )
                print.ExecuteNonQuery()

            End Using
        End Using

    End Sub

#End Region

End Class
