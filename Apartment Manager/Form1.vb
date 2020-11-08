Public Class Form1
    Sub daireGetir()
        dgvDaire.Rows.Clear()
        Dim dtDaire As DataTable = dtGetir("SELECT * FROM daire")
        For i = 0 To dtDaire.Rows.Count - 1
            dgvDaire.Rows.Add(dtDaire.Rows(i)("daire_id").ToString(),
                              dtDaire.Rows(i)("daire_no").ToString(),
                              dtDaire.Rows(i)("daire_oturan").ToString()
)
        Next
    End Sub

    Sub gelirGetir()
        dgvGeT.Rows.Clear()
        Dim dtGT As DataTable = dtGetir("SELECT * FROM gelirTip")
        For i = 0 To dtGT.Rows.Count - 1
            dgvGeT.Rows.Add(dtGT.Rows(i)("gelirTip_id").ToString(),
                              dtGT.Rows(i)("gelirTip_tip").ToString()
                              )
            cbGeT.Items.Add(dtGT.Rows(i)("gelirTip_tip").ToString())

        Next
    End Sub
    Sub giderGetir()
        dgvGiT.Rows.Clear()
        Dim dtGiT As DataTable = dtGetir("SELECT * FROM giderTip")
        For i = 0 To dtGiT.Rows.Count - 1
            dgvGiT.Rows.Add(dtGiT.Rows(i)("giderTip_id").ToString(),
                              dtGiT.Rows(i)("giderTip_tip").ToString()
                              )
            cbGiT.Items.Add(dtGiT.Rows(i)("giderTip_tip").ToString())
        Next

    End Sub
    Sub Bank()
        Dim gelir As DataTable = dtGetir("SELECT SUM(gelir_tutar) AS toplam FROM gelir")
        Dim gider As DataTable = dtGetir("SELECT SUM(gider_tutar) AS toplam FROM gider")
        lblBank.Text = gelir.Rows(0)("toplam") - gider.Rows(0)("toplam")
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        daireGetir()
        gelirGetir()
        giderGetir()
        Bank()
    End Sub

    Private Sub dgwDaire_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDaire.CellClick
        Dim daireId As Integer = dgvDaire.CurrentRow.Cells(0).Value
        Dim dtAidat As DataTable = dtGetir("SELECT * FROM aidat,daire WHERE daire.daire_id=aidat.daire_id AND aidat.daire_id=" & daireId)
        dgvAidat.Rows.Clear()
        For i = 0 To dtAidat.Rows.Count - 1
            dgvAidat.Rows.Add(dtAidat.Rows(i)("aidat_id").ToString,
           dtAidat.Rows(i)("aidat_donem").ToString,
           dtAidat.Rows(i)("aidat_tutar").ToString)
        Next

        txtDaireID.Text = dgvDaire.CurrentRow.Cells(0).Value.ToString()
        txtDaireNo.Text = dgvDaire.CurrentRow.Cells(1).Value.ToString()
        txtDaireOturan.Text = dgvDaire.CurrentRow.Cells(2).Value.ToString()
    End Sub

    Private Sub dgvGT_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvGeT.CellClick
        Dim gelirId As Integer = dgvGeT.CurrentRow.Cells(0).Value
        Dim dtGelir As DataTable = dtGetir("SELECT * FROM gelir,gelirTip WHERE gelirTip.gelirTip_id=gelir.gelirTip_id AND gelir.gelirTip_id=" & gelirId)
        dgvGelir.Rows.Clear()
        For i = 0 To dtGelir.Rows.Count - 1


            dgvGelir.Rows.Add(dtGelir.Rows(i)("gelir_id").ToString,
                              dtGelir.Rows(i)("gelir_tarih").ToString,
                              dtGelir.Rows(i)("gelir_tutar").ToString,
                              dtGelir.Rows(i)("gelir_aciklama").ToString
)
        Next

        txtGeTID.Text = dgvGeT.CurrentRow.Cells(0).Value.ToString()
        txtGelirTip.Text = dgvGeT.CurrentRow.Cells(1).Value.ToString()
        cbGeT.Text = dgvGeT.CurrentRow.Cells(1).Value.ToString()
    End Sub

    Private Sub btnDaireKyt_Click(sender As Object, e As EventArgs) Handles btnDaireKyt.Click
        Dim no = txtDaireNo.Text
        Dim oturan = txtDaireOturan.Text

        sqlCalistir("INSERT INTO daire (daire_no,daire_oturan) VALUES ('" & no & "','" & oturan & "')")
        MsgBox("Daire Eklendi")
        daireGetir()
    End Sub

    Private Sub btnDaireDuz_Click(sender As Object, e As EventArgs) Handles btnDaireDuz.Click

        If txtDaireID.Text = "" Then
            MsgBox("Lütfen Düzenlenecek Daireyi Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If

        If MsgBox("Daire Düzenlenecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If

        sqlCalistir("UPDATE daire SET daire_no='" & txtDaireNo.Text & "',daire_oturan='" & txtDaireOturan.Text & "' WHERE daire_id=" & Convert.ToInt32(txtDaireID.Text))
        MsgBox("Daire Düzenlendi")
        daireGetir()
    End Sub

    Private Sub btnDaireSil_Click(sender As Object, e As EventArgs) Handles btnDaireSil.Click
        If txtDaireID.Text = "" Then
            MsgBox("Lütfen Silinecek Daireyi Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If
        If MsgBox("Daire Silinecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If
        sqlCalistir("DELETE FROM daire WHERE daire_id=" & txtDaireID.Text)
        MsgBox("Daire Silindi")
        daireGetir()
    End Sub

    Private Sub btnSOI_Click(sender As Object, e As EventArgs) Handles btnSOI.Click
        If (txtAidatID.Text = "") Then
            MsgBox("Lütfen Kaydı Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If
        Dim id = txtAidatID.Text
        Dim daire = txtDaireNo.Text
        Dim oturan = txtDaireOturan.Text
        Dim tarih = dgvAidat.CurrentRow.Cells(1).Value.ToString()
        Dim gelir = txtAidatTutar.Text
        Dim aciklama = daire & " nolu dairede oturan " & oturan & " adlı kişinin " & tarih & " tarihli aidat ücreti."
        If MsgBox(txtDaireNo.Text & " nolu daire oturan " & oturan & " kişiye ₺" & gelir & " tutarında Aidat gelir kaydı oluşturulacaktır. Onaylıyor musunuz?", vbYesNo, "Kayıt Onayı") = MsgBoxResult.No Then
            Return
        End If
        sqlCalistir("INSERT INTO gelir (gelir_tarih,gelir_tutar,gelir_aciklama,gelirTip_id) VALUES ('" & tarih & "'," & gelir & ",'" & aciklama & "',1)")
        sqlCalistir("DELETE FROM aidat WHERE aidat_id=" & Convert.ToInt32(txtAidatID.Text))
        txtDaireNo.Text = ""
        gelirGetir()
        daireGetir()
        Bank()
    End Sub

    Private Sub dgvAidat_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAidat.CellClick

        txtAidatID.Text = dgvAidat.CurrentRow.Cells(0).Value.ToString()
        txtAidatTutar.Text = dgvAidat.CurrentRow.Cells(2).Value.ToString()

    End Sub

    Private Sub btnAidatOlustur_Click(sender As Object, e As EventArgs) Handles btnAidatOlustur.Click
        If (cbAy.Text = "" Or cbYil.Text = "" Or txtAidatTutar.Text = "") Then
            MsgBox("Lütfen tüm istenen bilgileri giriniz", MsgBoxStyle.Critical)
            Return
        End If


        Dim ay = cbAy.SelectedIndex + 1
        Dim yil = cbYil.Text
        Dim tarih = "1-" & ay & "-" & yil
        Dim ucret = txtAidatTutar.Text
        Dim daireler As DataTable = dtGetir("SELECT * FROM daire")
        For i = 0 To daireler.Rows.Count - 1
            sqlCalistir("INSERT INTO aidat (aidat_donem,aidat_tutar,daire_id) VALUES ('" & tarih & "'," & ucret & "," & daireler.Rows(i)("daire_id").ToString() & ")")
        Next
        MsgBox("Dönem Oluşturuludu")
    End Sub

    Private Sub btnGelirEkle_Click(sender As Object, e As EventArgs) Handles btnGelirEkle.Click
        Dim tarih = dtpGelir.Value.Day & "-" & dtpGelir.Value.Month & "-" & dtpGelir.Value.Year
        Dim tip = cbGeT.SelectedIndex + 1
        Dim gelir = txtGeTutar.Text
        Dim aciklama = txtGeAciklama.Text
        If MsgBox("₺" & gelir & " tutarında bir gelir kaydı oluşturmak üzeresiniz. Onaylıyor musunuz?", vbYesNo, "Kayıt Onayı") = MsgBoxResult.No Then
            Return
        End If
        sqlCalistir("INSERT INTO gelir (gelir_tarih,gelir_tutar,gelir_aciklama,gelirTip_id) VALUES ('" & tarih & "'," & gelir & ",'" & aciklama & "'," & tip & ")")
        Bank()
        gelirGetir()
    End Sub

    Private Sub btnGelirDuz_Click(sender As Object, e As EventArgs) Handles btnGelirDuz.Click
        Dim tarih = dtpGelir.Value.Day & "-" & dtpGelir.Value.Month & "-" & dtpGelir.Value.Year
        Dim tip = cbGeT.SelectedIndex + 1
        Dim gelir = txtGeTutar.Text
        Dim aciklama = txtGeAciklama.Text
        If txtGelirID.Text = "" Then
            MsgBox("Lütfen Düzenlenecek Kaydı Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If

        If MsgBox("Kayıt Düzenlenecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If


        sqlCalistir("UPDATE gelir SET gelir_tarih='" & tarih & "',gelir_tutar='" & gelir & "',gelir_aciklama='" & aciklama & "',gelirTip_id=" & tip & " WHERE gelir_id=" & Convert.ToInt32(txtGelirID.Text))
        gelirGetir()
        Bank()
    End Sub

    Private Sub dgvGelir_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvGelir.CellClick
        txtGelirID.Text = dgvGelir.CurrentRow.Cells(0).Value.ToString()
        dtpGelir.Value = dgvGelir.CurrentRow.Cells(1).Value.ToString()
        txtGeTutar.Text = dgvGelir.CurrentRow.Cells(2).Value.ToString()
        txtGeAciklama.Text = dgvGelir.CurrentRow.Cells(3).Value.ToString()

    End Sub

    Private Sub btnGelirSil_Click(sender As Object, e As EventArgs) Handles btnGelirSil.Click
        If txtGelirID.Text = "" Then
            MsgBox("Lütfen Silinecek Kaydı Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If
        If MsgBox("Kayıt Silinecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If
        sqlCalistir("DELETE FROM gelir WHERE gelir_id=" & txtGelirID.Text)
        gelirGetir()
        Bank()
    End Sub

    Private Sub btnGeTEkle_Click(sender As Object, e As EventArgs) Handles btnGeTEkle.Click
        sqlCalistir("INSERT INTO gelirTip (gelirTip_tip) VALUES ('" & txtGelirTip.Text & "')")
        gelirGetir()
    End Sub

    Private Sub btnGeTDuz_Click(sender As Object, e As EventArgs) Handles btnGeTDuz.Click
        If txtGeTID.Text = "" Then
            MsgBox("Lütfen Düzenlenecek Kaydı Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If

        If MsgBox("Kayıt Düzenlenecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If
        gelirGetir()
        sqlCalistir("UPDATE gelirTip SET gelirTip_tip='" & txtGelirTip.Text & "' WHERE gelirTip_id=" & Convert.ToInt32(txtGeTID.Text))

    End Sub

    Private Sub btnGeTSil_Click(sender As Object, e As EventArgs) Handles btnGeTSil.Click
        If txtGelirID.Text = "" Then
            MsgBox("Lütfen Silinecek Kaydı Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If
        If MsgBox("Kayıt Silinecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If
        sqlCalistir("DELETE FROM gelirTip WHERE gelirTip_id=" & txtGeTID.Text)
        gelirGetir()
        daireGetir()
    End Sub

    Private Sub btnGiEk_Click(sender As Object, e As EventArgs) Handles btnGiEk.Click
        Dim tarih = dtpGider.Value.Day & "-" & dtpGider.Value.Month & "-" & dtpGider.Value.Year
        Dim tip = cbGiT.SelectedIndex + 1
        Dim gider = txtGiTut.Text
        Dim aciklama = txtGiAciklama.Text
        If MsgBox("₺" & gider & " tutarında bir gider kaydı oluşturmak üzeresiniz. Onaylıyor musunuz?", vbYesNo, "Kayıt Onayı") = MsgBoxResult.No Then
            Return
        End If
        sqlCalistir("INSERT INTO gider (gider_tarih,gider_tutar,gider_aciklama,giderTip_id) VALUES ('" & tarih & "'," & gider & ",'" & aciklama & "'," & tip & ")")
        giderGetir()
        Bank()
    End Sub

    Private Sub btnGiDuz_Click(sender As Object, e As EventArgs) Handles btnGiDuz.Click
        Dim tarih = dtpGider.Value.Day & "-" & dtpGider.Value.Month & "-" & dtpGider.Value.Year
        Dim tip = cbGiT.SelectedIndex + 1
        Dim tutar = txtGiTut.Text
        Dim aciklama = txtGiAciklama.Text
        If txtGiID.Text = "" Then
            MsgBox("Lütfen Düzenlenecek Kaydı Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If

        If MsgBox("Kayıt Düzenlenecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If


        sqlCalistir("UPDATE gider SET gider_tarih='" & tarih & "',gider_tutar='" & tutar & "',gider_aciklama='" & aciklama & "',giderTip_id=" & tip & " WHERE gider_id=" & Convert.ToInt32(txtGiID.Text))
        giderGetir()
        Bank()
    End Sub

    Private Sub btnGiSil_Click(sender As Object, e As EventArgs) Handles btnGiSil.Click
        If txtGiID.Text = "" Then
            MsgBox("Lütfen Silinecek Kaydı Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If
        If MsgBox("Kayıt Silinecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If
        sqlCalistir("DELETE FROM gider WHERE gider_id=" & txtGiID.Text)
        giderGetir()
        Bank()
    End Sub

    Private Sub btnGiTEk_Click(sender As Object, e As EventArgs) Handles btnGiTEk.Click
        sqlCalistir("INSERT INTO giderTip (giderTip_tip) VALUES ('" & txtGiTip.Text & "')")
        giderGetir()
    End Sub

    Private Sub btnGiTDuz_Click(sender As Object, e As EventArgs) Handles btnGiTDuz.Click
        If txtGiTID.Text = "" Then
            MsgBox("Lütfen Düzenlenecek Kaydı Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If

        If MsgBox("Kayıt Düzenlenecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If

        sqlCalistir("UPDATE giderTip SET giderTip_tip='" & txtGiTip.Text & "' WHERE giderrTip_id=" & Convert.ToInt32(txtGiTID.Text))
        giderGetir()
    End Sub

    Private Sub btnGiTSil_Click(sender As Object, e As EventArgs) Handles btnGiTSil.Click
        If txtGiTID.Text = "" Then
            MsgBox("Lütfen Silinecek Kaydı Seçiniz!", MsgBoxStyle.Critical, "Hata")
            Return
        End If
        If MsgBox("Kayıt Silinecek Onaylıyor musunuz?", MsgBoxStyle.YesNo, "İşlem Onayı") = MsgBoxResult.No Then
            Return
        End If
        sqlCalistir("DELETE FROM giderTip WHERE giderTip_id=" & txtGiTID.Text)
        giderGetir()
    End Sub

    Private Sub dgvGiT_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvGiT.CellClick
        Dim giderId As Integer = dgvGiT.CurrentRow.Cells(0).Value
        Dim dtGider As DataTable = dtGetir("SELECT * FROM gider,giderTip WHERE giderTip.giderTip_id=gider.giderTip_id AND gider.giderTip_id=" & giderId)
        dgvGider.Rows.Clear()
        For i = 0 To dtGider.Rows.Count - 1
            dgvGider.Rows.Add(dtGider.Rows(i)("gider_id").ToString,
                              dtGider.Rows(i)("gider_tarih").ToString,
                              dtGider.Rows(i)("gider_tutar").ToString,
                              dtGider.Rows(i)("gider_aciklama").ToString
)
        Next
        txtGiTID.Text = dgvGiT.CurrentRow.Cells(0).Value.ToString()
        cbGiT.Text = dgvGiT.CurrentRow.Cells(1).Value.ToString()
        txtGiTip.Text = dgvGiT.CurrentRow.Cells(1).Value.ToString()
    End Sub

    Private Sub dgvGider_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvGider.CellClick
        txtGiID.Text = dgvGider.CurrentRow.Cells(0).Value.ToString()
        dtpGider.Value = dgvGider.CurrentRow.Cells(1).Value.ToString()
        txtGiTut.Text = dgvGider.CurrentRow.Cells(2).Value.ToString()
        txtGiAciklama.Text = dgvGider.CurrentRow.Cells(3).Value.ToString()

    End Sub

    Private Sub btnGiTemizle_Click(sender As Object, e As EventArgs) Handles btnGiTemizle.Click
        txtGiID.Text = ""
        txtGiAciklama.Text = ""
        txtGiTID.Text = ""
        txtGiTip.Text = ""
        txtGiTut.Text = ""
        cbGiT.Text = ""
    End Sub

    Private Sub btnGeTemizle_Click(sender As Object, e As EventArgs) Handles btnGeTemizle.Click
        txtGelirID.Text = ""
        txtGeAciklama.Text = ""
        txtGeTID.Text = ""
        txtGelirTip.Text = ""
        txtGeTutar.Text = ""
        cbGeT.Text = ""
    End Sub
End Class
