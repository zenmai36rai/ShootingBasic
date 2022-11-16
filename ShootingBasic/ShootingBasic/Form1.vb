Public Class Form1
    Public Class Fighter
        Public img As Bitmap = New Bitmap("..\..\Resources\fighter.png")
        Public x As Integer = 150
        Public y As Integer = 150
    End Class
    Private f As Fighter = New Fighter
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim g As Graphics = PictureBox1.CreateGraphics()
        g.DrawImage(f.img, f.x, f.y)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        f.x = f.x - 4
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Timer1.Start()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        f.x = f.x + 4
    End Sub
End Class
