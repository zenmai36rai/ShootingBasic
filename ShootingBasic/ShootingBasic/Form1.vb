Public Class Form1
    Private Class Controller
        Public _d As Boolean = False
        Public _u As Boolean = False
        Public _l As Boolean = False
        Public _r As Boolean = False
        Public _s As Boolean = False
    End Class
    Private Class Fighter
        Public _img As Bitmap = New Bitmap("..\..\Resources\fighter.bmp")
        Public _x As Integer = 180
        Public _y As Integer = 270
        Public SPEED_FIGHTER = 4
        Public Sub Move(c As Controller, s As Shot)
            If c._l = True Then
                _x = _x - SPEED_FIGHTER
            ElseIf c._r = True Then
                _x = _x + SPEED_FIGHTER
            End If
            If c._u = True Then
                _y = _y - SPEED_FIGHTER
            ElseIf c._d = True Then
                _y = _y + SPEED_FIGHTER
            End If
            If c._s = True Then
                s._shoot(_x, _y)
            End If
        End Sub
    End Class
    Private Class Shot
        Public _img As Bitmap = New Bitmap("..\..\Resources\Shot.bmp")
        Public ID_MAX = 5
        Public SPEED_SHOT = 8
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
                _y(i) = _y(i) - SPEED_SHOT
            Next
        End Sub
    End Class
    Private canvas As Bitmap
    Private _g As Graphics
    Private _c As Controller = New Controller
    Private _f As Fighter = New Fighter
    Private _s As Shot = New Shot
    Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        _f.Move(_c, _s)
        For i = 0 To (_s.ID_MAX - 1)
            _s.Move()
        Next
        _g = Graphics.FromImage(canvas)
        _g.FillRectangle(Brushes.Black, 0, 0, Me.Width, Me.Height)
        For i = 0 To 2
            _g.DrawImage(_s._img, _s._x(i), _s._y(i))
        Next
        _g.DrawImage(_f._img, _f._x, _f._y)
        _g.Dispose()
        PictureBox1.Image = canvas
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PictureBox1.Width = Me.Width
        PictureBox1.Height = Me.Height
        canvas = New Bitmap(Me.Width, Me.Height)
        Timer1.Start()
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Left
                _c._l = True
            Case Keys.Right
                _c._r = True
            Case Keys.Up
                _c._u = True
            Case Keys.Down
                _c._d = True
            Case Keys.Space
                _c._s = True
            Case Keys.Z
                _c._s = True
            Case Keys.X
                _c._s = True
        End Select
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        Select Case e.KeyCode
            Case Keys.Left
                _c._l = False
            Case Keys.Right
                _c._r = False
            Case Keys.Up
                _c._u = False
            Case Keys.Down
                _c._d = False
            Case Keys.Space
                _c._s = False
            Case Keys.Z
                _c._s = False
            Case Keys.X
                _c._s = False
        End Select
    End Sub
End Class
