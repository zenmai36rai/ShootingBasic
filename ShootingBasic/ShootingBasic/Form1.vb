Public Class Form1
    Private Class Fighter
        Public _img As Bitmap = New Bitmap("..\..\Resources\fighter.png")
        Public _x As Integer = 150
        Public _y As Integer = 220
    End Class
    Private Class Shot
        Public _img As Bitmap = New Bitmap("..\..\Resources\Shot.png")
        Private ID_MAX = 3
        Private _id As Integer = 0
        Public _x() As Integer = {1000, 1000, 1000}
        Public _y() As Integer = {1000, 1000, 1000}
        Public Sub _shoot(ByVal x As Integer, ByVal y As Integer)
            _x(_id) = x
            _y(_id) = y
            _id = _id + 1
            If _id = ID_MAX Then
                _id = 0
            End If
        End Sub
        Public Sub Move()
            For i = 0 To 2
                _y(i) = _y(i) - 8
            Next
        End Sub
    End Class
    Private _f As Fighter = New Fighter
    Private _s As Shot = New Shot
    Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For i = 0 To 2
            _s.Move()
        Next
        Dim g As Graphics = PictureBox1.CreateGraphics()
        g.FillRectangle(Brushes.Black, 0, 0, PictureBox1.Width, PictureBox1.Height)
        For i = 0 To 2
            g.DrawImage(_s._img, _s._x(i), _s._y(i))
        Next
        g.DrawImage(_f._img, _f._x, _f._y)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        _f._x = _f._x - 4
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        _s._shoot(_f._x, _f._y)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        _f._x = _f._x + 4
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()
    End Sub

    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        Dim c As Char = e.KeyChar
        Console.Write(c)
        If c = " " Then
            _s._shoot(_f._x, _f._y)
        End If
    End Sub

    Private Sub Form1_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles MyBase.PreviewKeyDown
        Select Case e.KeyCode
            Case Keys.Left
                _f._x = _f._x - 4
            Case Keys.Right
                _f._x = _f._x + 4
        End Select
    End Sub
End Class
