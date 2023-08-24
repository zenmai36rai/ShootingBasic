Public Class Form1
    <Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function GetAsyncKeyState(
        ByVal nVirtKey As Integer) As Integer
    End Function
    Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Long, ByVal hwndCallback As Long) As Long
    Private Class Controller
        Public _d As Boolean = False
        Public _u As Boolean = False
        Public _l As Boolean = False
        Public _r As Boolean = False
        Public _s As Boolean = False
        Public _ctrl As Boolean = False
    End Class
    Const SCREEN_WIDTH As Integer = 1377
    Const SCREEN_HIGHT As Integer = 768
    Private Class Fighter
        Public _img As Bitmap = New Bitmap("..\..\Resources\fighter.png")
        Public _x As Integer = SCREEN_WIDTH / 2 - 48 / 2
        Public _y As Integer = SCREEN_HIGHT - 48 * 4
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
        Public _img As Bitmap = New Bitmap("..\..\Resources\shot.png")
        Private SHOT_WAV_01 As String = "..\..\Resources\shot001.wav"
        Private SHOT_WAV_02 As String = "..\..\Resources\shot02.wav"
        Private SHOT_WAV_03 As String = "..\..\Resources\shot03.wav"
        Private SHOT_WAV_04 As String = "..\..\Resources\shot04.wav"
        Private SHOT_WAV_05 As String = "..\..\Resources\shot05.wav"
        Private BUFF_WAV As Integer = 16
        Private _buff As Integer = BUFF_WAV
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

            If _id = 0 Then
                mciSendString("play """ & SHOT_WAV_01 & """", "", 0, 0)
            ElseIf _id = 1 Then
                mciSendString("play """ & SHOT_WAV_02 & """", "", 0, 0)
            ElseIf _id = 2 Then
                mciSendString("play """ & SHOT_WAV_03 & """", "", 0, 0)
            ElseIf _id = 3 Then
                mciSendString("play """ & SHOT_WAV_04 & """", "", 0, 0)
            ElseIf _id = 4 Then
                mciSendString("play """ & SHOT_WAV_05 & """", "", 0, 0)
            End If

            _x(_id) = x
            _y(_id) = y
            _id = _id + 1
            If _id = ID_MAX Then
                _id = 0
            End If
        End Sub
        Public Sub Move()
            _buff = _buff - 1
            If _buff = 0 Then
                _buff = BUFF_WAV
            End If
            For i = 0 To (ID_MAX - 1)
                _y(i) = _y(i) - SPEED_SHOT
            Next
        End Sub
    End Class
    Private Class Invader
        Public _img As Bitmap = New Bitmap("..\..\Resources\alien.png")
        Public ID_MAX = 144
        Public H_BUFF = 100
        Public V_BUFF = 0
        Public _t As Double = 0
        Public _x(ID_MAX) As Double
        Public _y(ID_MAX) As Double
        Public _width As Integer = _img.Width
        Public _height As Integer = _img.Height
        Public _def(ID_MAX) As Integer
        Const MOV_LEFT As Integer = 0
        Const MOV_RIGHT As Integer = 1
        Const MOV_DOWN As Integer = 2
        Public _downposition As Integer = 0
        Public _moveflag As Integer = MOV_LEFT
        Public _movespeed As Double = 0.02
        Public Sub New()
            For i = 0 To (ID_MAX - 1)
                _x(i) = (i Mod 18) * 64 + H_BUFF
                _y(i) = Int(i / 18) * 64 + V_BUFF
                _def(i) = 3
            Next
        End Sub
        Public Sub Move(ByRef e As EnemyShot)
            Dim b As Boolean = False
            If _moveflag = MOV_LEFT Then
                For i = 0 To (ID_MAX - 1)
                    _x(i) = _x(i) - _movespeed
                    If _x(i) < 1 And _y(i) < 1000 Then
                        b = True
                    End If
                Next
                If b Then
                    _moveflag = MOV_RIGHT
                End If
            ElseIf _moveflag = MOV_RIGHT Then
                For i = 0 To (ID_MAX - 1)
                    _x(i) = _x(i) + _movespeed
                    If _x(i) > SCREEN_WIDTH - 64 And _y(i) < 1000 Then
                        b = True
                    End If
                Next
                If b = True Then
                    _moveflag = MOV_DOWN
                    _downposition = 64 + H_BUFF
                End If
            ElseIf _moveflag = MOV_DOWN Then
                For i = 0 To (ID_MAX - 1)
                    _y(i) = _y(i) + _movespeed
                Next
                _downposition = _downposition - 1
                If _downposition = 0 Then
                    _moveflag = MOV_LEFT
                End If
            End If
            Dim r As Integer = Rnd() * 2048
            If r < ID_MAX Then
                e._shoot(_x(r), _y(r))
            End If
        End Sub
    End Class
    Private Class EnemyShot
        Public _img As Bitmap = New Bitmap("..\..\Resources\e_shot.png")
        Public SPEED_SHOT = 4
        Public ID_MAX = 5
        Private _id As Integer = 0
        Public _x() As Integer = {-100, -100, -100, -100, -100}
        Public _y() As Integer = {1000, 1000, 1000, 1000, 1000}
        Public _width As Integer = _img.Width
        Public _height As Integer = _img.Height
        Public Sub _shoot(ByVal x As Integer, ByVal y As Integer)
            If _y(_id) < 1000 Then
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
                _y(i) = _y(i) + SPEED_SHOT
            Next
        End Sub
    End Class
    Private BOMB_WAV_01 As String = "..\..\Resources\bomb01.wav"
    Private BOMB_WAV_02 As String = "..\..\Resources\bomb02.wav"
    Private BOMB_WAV_03 As String = "..\..\Resources\bomb03.wav"
    Private BOMB_WAV_04 As String = "..\..\Resources\bomb04.wav"
    Private BOMB_WAV_05 As String = "..\..\Resources\bomb05.wav"
    Private _bomb_count As Integer = 0
    Const BOMB_COUNT_MAX As Integer = 5
    Private Function CrossJudge(a As Invader, s As Shot) As Boolean
        For i = 0 To a.ID_MAX - 1
            For j = 0 To s.ID_MAX - 1
                If (a._x(i) < (s._x(j) + s._width)) And (s._x(j) < a._x(i) + a._width) Then
                    If (a._y(i) < s._y(j) + s._height) And (s._y(j) < a._y(i) + a._height) Then
                        a._def(i) = a._def(i) - 1
                        s._y(j) = -100
                        If a._def(i) = 0 Then
                            If _bomb_count = 0 Then
                                mciSendString("play """ & BOMB_WAV_01 & """", "", 0, 0)
                                _bomb_count = _bomb_count + 1
                            ElseIf _bomb_count = 1 Then
                                mciSendString("play """ & BOMB_WAV_02 & """", "", 0, 0)
                                _bomb_count = _bomb_count + 1
                            ElseIf _bomb_count = 2 Then
                                mciSendString("play """ & BOMB_WAV_03 & """", "", 0, 0)
                                _bomb_count = _bomb_count + 1
                            ElseIf _bomb_count = 3 Then
                                mciSendString("play """ & BOMB_WAV_04 & """", "", 0, 0)
                                _bomb_count = _bomb_count + 1
                            Else
                                mciSendString("play """ & BOMB_WAV_05 & """", "", 0, 0)
                                _bomb_count = 0
                            End If
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
    Private _e As EnemyShot = New EnemyShot
    Private _gxy As Bitmap = New Bitmap("..\..\Resources\galaxy_l.png")
    Private _wav As System.Media.SoundPlayer = Nothing
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
        For i = 0 To (_a.ID_MAX - 1)
            _a.Move(_e)
        Next
        For i = 0 To (_e.ID_MAX - 1)
            _e.Move()
        Next
        _g = Graphics.FromImage(canvas)
        '_g.FillRectangle(Brushes.Black, 0, 0, Me.Width, Me.Height)
        _g.DrawImage(_gxy, 0, 0)
        For i = 0 To (_a.ID_MAX - 1)
            Dim x As Integer = _a._x(i)
            Dim y As Integer = _a._y(i)
            _g.DrawImage(_a._img, x, y)
        Next
        For i = 0 To 2
            _g.DrawImage(_s._img, _s._x(i), _s._y(i))
        Next
        _g.DrawImage(_f._img, _f._x, _f._y)
        For i = 0 To (_e.ID_MAX - 1)
            _g.DrawImage(_e._img, _e._x(i), _e._y(i))
        Next
        _g.Dispose()
        PictureBox1.Image = canvas
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PictureBox1.Width = Me.Width
        PictureBox1.Height = Me.Height
        canvas = New Bitmap(Me.Width, Me.Height)
        _wav = New System.Media.SoundPlayer("..\..\Resources\wpm001.wav")
        _wav.PlayLooping()
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
