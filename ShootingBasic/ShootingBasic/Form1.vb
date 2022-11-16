Public Class Form1
    Private Class Controller
        Public _l As Boolean = False
        Public _r As Boolean = False
        Public _s As Boolean = False
    End Class
    Private Class Fighter
        Public _img As Bitmap = New Bitmap("..\..\Resources\fighter.png")
        Public _x As Integer = 150
        Public _y As Integer = 220
    End Class
    Private Class Shot
        Public _img As Bitmap = New Bitmap("..\..\Resources\Shot.png")
        Public ID_MAX = 5
        Private _id As Integer = 0
        Public _x() As Integer = {-100, -100, -100, -100, -100}
        Public _y() As Integer = {-100, -100, -100, -100, -100}
        Public Sub _shoot(ByVal x As Integer, ByVal y As Integer)
            If _y(_id) > -100 Then
                Exit Sub
            End If
            _x(_id) = x
            _y(_id) = y
            _id = _id + 1
            If _id = ID_MAX Then
                _id = 0
            End If
        End Sub
        Public Sub Move()
            For i = 0 To (ID_MAX - 1)
                _y(i) = _y(i) - 8
            Next
        End Sub
    End Class
    Private _c As Controller = New Controller
    Private _f As Fighter = New Fighter
    Private _s As Shot = New Shot
    Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If _c._l = True Then
            _f._x = _f._x - 4
        ElseIf _c._r = True Then
            _f._x = _f._x + 4
        End If
        If _c._s = True Then
            _s._shoot(_f._x, _f._y)
        End If
        For i = 0 To (_s.ID_MAX - 1)
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

    Private Sub Form1_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles MyBase.PreviewKeyDown
        Select Case e.KeyCode
            Case Keys.Left
                _c._l = True
            Case Keys.Right
                _c._r = True
        End Select
        If e.KeyCode = Keys.Space Then
            _c._s = True
        End If
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        Select Case e.KeyCode
            Case Keys.Left
                _c._l = False
            Case Keys.Right
                _c._r = False
        End Select
        If e.KeyCode = Keys.Space Then
            _c._s = False
        End If
    End Sub
End Class
