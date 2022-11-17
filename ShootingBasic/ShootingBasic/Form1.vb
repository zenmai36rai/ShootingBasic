Public Class Form1
    <Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function GetAsyncKeyState(
        ByVal nVirtKey As Integer) As Integer
    End Function
    Private Class Controller
        Public _d As Boolean = False
        Public _u As Boolean = False
        Public _l As Boolean = False
        Public _r As Boolean = False
        Public _s As Boolean = False
        Public _ctrl As Boolean = False
    End Class
    Private Class Fighter
        Public _img As Bitmap = New Bitmap("..\..\Resources\fighter.bmp")
        Public _x As Integer = 228
        Public _y As Integer = 340
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
            If (c._s = True) Or (c._ctrl = True) Then
                s._shoot(_x, _y)
            End If
        End Sub
    End Class
    Private Class Shot
        Public _img As Bitmap = New Bitmap("..\..\Resources\shot.bmp")
        Public ID_MAX = 5
        Public SPEED_SHOT = 8
        Private _id As Integer = 0
        Public _x() As Integer = {-100, -100, -100, -100, -100}
        Public _y() As Integer = {-100, -100, -100, -100, -100}
        Public _width As Integer = _img.Width
        Public _height As Integer = _img.Height
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
    Private Class Invader
        Public _img As Bitmap = New Bitmap("..\..\Resources\alien.bmp")
        Public ID_MAX = 20
        Public H_BUFF = 100
        Public V_BUFF = 0
        Public _x(20) As Integer
        Public _y(20) As Integer
        Public _width As Integer = _img.Width
        Public _height As Integer = _img.Height
        Public _def(20) As Integer
        Public Sub New()
            For i = 0 To (ID_MAX - 1)
                _x(i) = (i Mod 5) * 64 + H_BUFF
                _y(i) = Int(i / 5) * 64 + V_BUFF
                _def(i) = 10
            Next
        End Sub
    End Class
    Private Function CrossJudge(a As Invader, s As Shot) As Boolean
        For i = 0 To a.ID_MAX - 1
            For j = 0 To s.ID_MAX - 1
                If (a._x(i) < (s._x(j) + s._width)) And (s._x(j) < a._x(i) + a._width) Then
                    If (a._y(i) < s._y(j) + s._height) And (s._y(j) < a._y(i) + a._height) Then
                        a._def(i) = a._def(i) - 1
                        s._y(j) = -100
                        If a._def(i) = 0 Then
                            a._y(i) = 1000
                        End If
                    End If
                    End If
            Next
        Next
        Return False
    End Function
    Private canvas As Bitmap
    Private _g As Graphics
    Private _c As Controller = New Controller
    Private _f As Fighter = New Fighter
    Private _s As Shot = New Shot
    Private _a As Invader = New Invader
    Sub ControllerCheck()
        Dim ret As Integer
        ret = GetAsyncKeyState(Keys.Left)
        _c._l = ret <> 0
        ret = GetAsyncKeyState(Keys.Right)
        _c._r = ret <> 0
        ret = GetAsyncKeyState(Keys.Up)
        _c._u = ret <> 0
        ret = GetAsyncKeyState(Keys.Down)
        _c._d = ret <> 0
        ret = GetAsyncKeyState(Keys.Space)
        _c._s = ret <> 0
    End Sub
    Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Call ControllerCheck()
        CrossJudge(_a, _s)
        _f.Move(_c, _s)
        For i = 0 To (_s.ID_MAX - 1)
            _s.Move()
        Next
        _g = Graphics.FromImage(canvas)
        _g.FillRectangle(Brushes.Black, 0, 0, Me.Width, Me.Height)
        For i = 0 To (_a.ID_MAX - 1)
            _g.DrawImage(_a._img, _a._x(i), _a._y(i))
        Next
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
        If (e.Modifiers And Keys.Control) = Keys.Control Then
            _c._ctrl = True
        End If
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If (e.Modifiers And Keys.Control) <> Keys.Control Then
            _c._ctrl = False
        End If
    End Sub
End Class
