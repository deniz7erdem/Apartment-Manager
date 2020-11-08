Imports System.Data.OleDb
Module vt
    Function Baglan() As OleDbConnection
        Dim baglanti = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & Application.StartupPath & "/copy.mdb")
        baglanti.Open()
        Return baglanti
    End Function

    Function dtGetir(sql As String) As DataTable
        Dim baglanti As OleDbConnection = Baglan()
        Dim cmd = New OleDbCommand
        cmd.Connection = baglanti
        cmd.CommandText = sql
        Dim adp = New OleDbDataAdapter
        adp.SelectCommand = cmd
        Dim dt As New DataTable
        adp.Fill(dt)
        cmd.Connection.Close()
        baglanti.Close()
        cmd.Dispose()
        adp.Dispose()

        Return dt
    End Function

    Function dsGetir(sql As String) As DataSet
        Dim baglanti As OleDbConnection = Baglan()
        Dim cmd = New OleDbCommand
        cmd.Connection = baglanti
        cmd.CommandText = sql
        Dim adp = New OleDbDataAdapter
        adp.SelectCommand = cmd
        Dim ds As New DataSet
        adp.Fill(ds, "tb")
        cmd.Connection.Close()
        baglanti.Close()
        cmd.Dispose()
        adp.Dispose()

        Return ds
    End Function

    Function sqlCalistir(sql As String) As Boolean
        Dim baglanti As OleDbConnection = Baglan()
        Dim cmd = New OleDbCommand
        cmd.Connection = baglanti
        cmd.CommandText = sql
        cmd.ExecuteNonQuery()
        cmd.Connection.Close()
        baglanti.Close()
        cmd.Dispose()
        Return True
    End Function
    Function kayitSayisi(sql As String) As Integer

        Dim baglanti As OleDbConnection = Baglan()
        Dim cmd = New OleDbCommand
        cmd.Connection = baglanti
        cmd.CommandText = sql
        Dim adp = New OleDbDataAdapter
        adp.SelectCommand = cmd
        Dim dt As New DataTable
        adp.Fill(dt)
        cmd.Connection.Close()
        baglanti.Close()
        cmd.Dispose()
        adp.Dispose()
        Return dt.Rows.Count
    End Function
End Module