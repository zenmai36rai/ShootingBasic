' SPACE INVADER LIKE SHOOTING GAME
' EDIT BY KYOSUKE MIYAZAWA
' COPYRIGHT SINCE 2023 ALL RIGHTS RESERVED
Public Class Form1
    <Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function GetAsyncKeyState(
        ByVal nVirtKey As Integer) As Integer
    End Function
    Private Score As Integer = 0
    Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Long, ByVal hwndCallback As Long) As Long
    Private Class Controller
        Public _d As Boolean = False
        Public _u As Boolean = False
        Public _l As Boolean = False
        Public _r As Boolean = False
        Public _s As Boolean = False
        Public _ctrl As Boolean = False
    End Class
    Const SCREEN_WIDTH As Integer = 600
    Const SCREEN_HIGHT As Integer = 704
    Private Class Fighter
        Public _img As Bitmap = New Bitmap("..\..\Resources\fighter.png")
        Public _x As Integer = SCREEN_WIDTH / 2 - 48 / 2
        Public _y As Integer = SCREEN_HIGHT - 48
        Public SPEED_FIGHTER = 4
        Public Sub Move(c As Controller, s As Shot)
            If c._l = True Then
                _x = _x - SPEED_FIGHTER
            ElseIf c._r = True Then
                _x = _x + SPEED_FIGHTER
            End If
            If False Then
                If c._u = True Then
                    _y = _y - SPEED_FIGHTER
                ElseIf c._d = True Then
                    _y = _y + SPEED_FIGHTER
                End If
            End If
            If (c._s = True) Or (c._ctrl = True) Then
                s._shoot(_x, _y, _img.Width)
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
        Public Sub _shoot(ByVal x As Integer, ByVal y As Integer, ByVal w As Integer)
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

            _x(_id) = x + (w / 2) - (_width / 2)
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
    Private Class Chip
        Public _x As Double = 0
        Public _y As Double = 0
        Public _vx As Double = 0
        Public _vy As Double = 0
        Public _life As Integer = 0
        Public Sub New()

        End Sub
        Public Sub Hit(ByVal x As Integer, ByVal y As Integer)
            _x = x
            _y = y
            _life = 10
            Dim angle As Double = Rnd() * 3.14 * 2
            _vx = Math.Sin(angle) * 8
            _vy = Math.Cos(angle) * 8
        End Sub
        Public Sub Move()
            If _life = 0 Then
                Return
            End If
            _x = _x + _vx
            _y = _y + _vy
            _life = _life - 1
        End Sub
    End Class
    Private Class Chips
        Public ID_MAX As Integer = 5
        Public _c(ID_MAX) As Chip
        Public Sub New()
            For i = 0 To ID_MAX - 1
                _c(i) = New Chip
            Next
        End Sub
    End Class

    Private Class UFO
        Public _img As Bitmap = New Bitmap("..\..\Resources\ufo.png")
        Public _x As Integer
        Public _y As Integer
        Public _width As Integer = _img.Width
        Public _height As Integer = _img.Height
        Public _a As Integer = 1
        Public Sub New()
            _x = -_width
            _y = 0
            _a = 1
        End Sub
        Public Sub Appere()
            _x = -_width
            _y = 0
            _a = 1
        End Sub
        Public Sub Move()
            _x = _x + 4
            If _x > 600 Then
                _a = 0
            End If
        End Sub
    End Class
    Private Class Invader
        Public _img As Bitmap
        Public _img_green As Bitmap = New Bitmap("..\..\Resources\alien.png")
        Public _img_red As Bitmap = New Bitmap("..\..\Resources\alien_red.png")
        Public _img_hit As Bitmap = New Bitmap("..\..\Resources\alien_hit.png")
        Public ENEMY_COLOM = 7
        Public ENEMY_LOW = 5
        Public ID_MAX = ENEMY_COLOM * ENEMY_LOW
        Public H_BUFF = 64
        Public V_BUFF = 0
        Public _t As Double = 0
        Public _x(ID_MAX) As Double
        Public _y(ID_MAX) As Double
        Public _h(ID_MAX) As Integer
        Public _width As Integer = _img_green.Width
        Public _height As Integer = _img_green.Height
        Public _def(ID_MAX) As Integer
        Const MOV_LEFT As Integer = 0
        Const MOV_DOWN_L As Integer = 1
        Const MOV_RIGHT As Integer = 2
        Const MOV_DOWN_R As Integer = 3
        Public _downposition As Double = 0
        Public _moveflag As Integer = MOV_LEFT
        Public _movespeed As Double = 0.05
        Public _downspeed As Double = 0.05
        Public Sub New()
            _img = _img_green
            For i = 0 To (ID_MAX - 1)
                _x(i) = (i Mod ENEMY_COLOM) * 80 + H_BUFF
                _y(i) = Int(i / ENEMY_COLOM) * 64 + V_BUFF + 36
                _def(i) = 4
                _h(i) = 0
            Next
        End Sub
        Public Function CountAlien() As Integer
            Dim count = 0
            For i = 0 To (ID_MAX - 1)
                If _y(i) <= 999 Then
                    count = count + 1
                    _h(i) = 0
                End If
            Next
            If count <= 3 Then
                _img = _img_red
                _movespeed = 0.2
                _downspeed = 0.2
            Else
                _img = _img_green
                _movespeed = 0.05
                _downspeed = 0.05
            End If
            Return count
        End Function

        Public Function GetImage(ByVal count As Integer) As Bitmap
            If _h(count) = 0 Then
                Return _img
            Else
                Return _img_hit
            End If
        End Function
        Public Sub Hit(ByVal count As Integer)
            _h(count) = 1
        End Sub
        Public Sub Move(ByRef e As EnemyShot)
            CountAlien()
            Dim b As Boolean = False
            If _moveflag = MOV_LEFT Then
                For i = 0 To (ID_MAX - 1)
                    _x(i) = _x(i) - _movespeed
                    If _x(i) < 1 And _y(i) < 1000 Then
                        b = True
                    End If
                Next
                If b = True Then
                    _moveflag = MOV_DOWN_L
                    _downposition = (32 + V_BUFF) * 25
                End If
            ElseIf _moveflag = MOV_RIGHT Then
                For i = 0 To (ID_MAX - 1)
                    _x(i) = _x(i) + _movespeed
                    If _x(i) > SCREEN_WIDTH - 64 And _y(i) < 1000 Then
                        b = True
                    End If
                Next
                If b = True Then
                    _moveflag = MOV_DOWN_R
                    _downposition = (32 + V_BUFF) * 25
                End If
            ElseIf _moveflag = MOV_DOWN_L Then
                For i = 0 To (ID_MAX - 1)
                    _y(i) = _y(i) + _downspeed
                Next
                _downposition = _downposition - _downspeed * 20
                If _downposition <= 0 Then
                    _moveflag = MOV_RIGHT
                End If
            ElseIf _moveflag = MOV_DOWN_R Then
                For i = 0 To (ID_MAX - 1)
                    _y(i) = _y(i) + _downspeed
                Next
                _downposition = _downposition - _downspeed * 20
                If _downposition <= 0 Then
                    _moveflag = MOV_LEFT
                End If
            End If
            Dim r As Integer = Rnd() * 2560
            If r < ID_MAX Then
                e._shoot(_x(r), _y(r), _img.Width)
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
        Public Sub _shoot(ByVal x As Integer, ByVal y As Integer, ByVal _w As Integer)
            If _y(_id) < 1000 Then
                Exit Sub
            End If
            _x(_id) = x + (_w / 2) - (_img.Width / 2)
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
    Private HIT_WAV_01 As String = "..\..\Resources\hit01.wav"
    Private HIT_WAV_02 As String = "..\..\Resources\hit02.wav"
    Private HIT_WAV_03 As String = "..\..\Resources\hit03.wav"
    Private HIT_WAV_04 As String = "..\..\Resources\hit04.wav"
    Private HIT_WAV_05 As String = "..\..\Resources\hit05.wav"
    Private BOMB_WAV_01 As String = "..\..\Resources\bomb01.wav"
    Private BOMB_WAV_02 As String = "..\..\Resources\bomb02.wav"
    Private BOMB_WAV_03 As String = "..\..\Resources\bomb03.wav"
    Private BOMB_WAV_04 As String = "..\..\Resources\bomb04.wav"
    Private BOMB_WAV_05 As String = "..\..\Resources\bomb05.wav"
    Private UFO_BONUS_WAV As String = "..\..\Resources\ufo_bonus_01.wav"
    Private FANFARE_WAV As String = "..\..\Resources\fanfare.wav"
    Private _bomb_count As Integer = 0
    Private _hit_count As Integer = 0
    Const BOMB_COUNT_MAX As Integer = 5
    Private Function CrossJudge(a As Invader, s As Shot) As Boolean
        For i = 0 To a.ID_MAX - 1
            For j = 0 To s.ID_MAX - 1
                If (a._x(i) < (s._x(j) + s._width)) And (s._x(j) < a._x(i) + a._width) Then
                    If (a._y(i) < s._y(j) + s._height) And (s._y(j) < a._y(i) + a._height) Then
                        a._def(i) = a._def(i) - 1
                        a.Hit(i)
                        HitShot(_hit_count)
                        _ch._c(j).Hit(s._x(j), s._y(j))
                        s._y(j) = -100
                        If a._def(i) = 0 Then
                            Score = Score + 20
                            Bomb(_bomb_count)
                            a._y(i) = 1000
                        Else
                            Score = Score + 10
                        End If
                    End If
                End If
            Next
        Next
        Return False
    End Function
    Private Function UFOJudge(u As UFO, s As Shot) As Boolean
        For j = 0 To s.ID_MAX - 1
            If (u._x < (s._x(j) + s._width)) And (s._x(j) < u._x + u._width) Then
                If (u._y < s._y(j) + s._height) And (s._y(j) < u._y + u._height) Then
                    Score = Score + 300
                    u._a = 0
                    s._y(j) = -100
                    mciSendString("play """ & UFO_BONUS_WAV & """", "", 0, 0)
                    Bomb(_bomb_count)
                    u._x = 1000
                End If
            End If
        Next
        Return False
    End Function
    Private Function GameOverJudge(a As Invader, es As EnemyShot, f As Fighter) As Boolean
        For i = 0 To a.ID_MAX - 1
            If (a._x(i) < (f._x + f._img.Width)) And (f._x < a._x(i) + a._width) Then
                If (a._y(i) < f._y + f._img.Height) And (f._y < a._y(i) + a._height) Then
                    Return True
                End If
            End If
        Next

        For i = 0 To es.ID_MAX - 1
            If (es._x(i) < (f._x + f._img.Width)) And (f._x < es._x(i) + es._width) Then
                If (es._y(i) < f._y + f._img.Height) And (f._y < es._y(i) + es._height) Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function
    Private Function Bomb(ByRef _bomb_count As Integer)
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
    End Function
    Public Sub HitShot(ByRef _hit_count)
        If _hit_count = 0 Then
            mciSendString("play """ & HIT_WAV_01 & """", "", 0, 0)
            _hit_count = _hit_count + 1
        ElseIf _hit_count = 1 Then
            mciSendString("play """ & HIT_WAV_02 & """", "", 0, 0)
            _hit_count = _hit_count + 1
        ElseIf _hit_count = 2 Then
            mciSendString("play """ & HIT_WAV_03 & """", "", 0, 0)
            _hit_count = _hit_count + 1
        ElseIf _bomb_count = 3 Then
            mciSendString("play """ & HIT_WAV_04 & """", "", 0, 0)
            _hit_count = _hit_count + 1
        Else
            mciSendString("play """ & HIT_WAV_05 & """", "", 0, 0)
            _hit_count = 0
        End If
    End Sub
    Private canvas As Bitmap
    Private _g As Graphics
    Private _c As Controller = New Controller
    Private _f As Fighter = New Fighter
    Private _s As Shot = New Shot
    Private _a As Invader = New Invader
    Private _e As EnemyShot = New EnemyShot
    Private _u As UFO = New UFO
    Private _ch As Chips = New Chips
    Private _gxy As Bitmap = New Bitmap("..\..\Resources\galaxy_l.png")
    Private _gxy_title As Bitmap = New Bitmap("..\..\Resources\galaxy_title.png")
    Private _gxy_red As Bitmap = New Bitmap("..\..\Resources\galaxy_red.png")
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

    Sub InitGame()
        _f = New Fighter
        _s = New Shot
        _a = New Invader
        _e = New EnemyShot
        _u = New UFO
        _ch = New Chips
    End Sub
    Sub GameLoop()
        If Rnd() * 128 < 1 And _u._a = 0 Then
            _u.Appere()
        End If
        _f.Move(_c, _s)
        For i = 0 To (_s.ID_MAX - 1)
            _s.Move()
            _ch._c(i).Move()
        Next
        For i = 0 To (_a.ID_MAX - 1)
            _a.Move(_e)
        Next
        For i = 0 To (_e.ID_MAX - 1)
            _e.Move()
        Next
        _u.Move()
        CrossJudge(_a, _s)
        UFOJudge(_u, _s)
        _g = Graphics.FromImage(canvas)
        '_g.FillRectangle(Brushes.Black, 0, 0, Me.Width, Me.Height)
        If GameOverJudge(_a, _e, _f) = False Then
            _g.DrawImage(_gxy, 0, 0)
        Else
            _g.DrawImage(_gxy_red, 0, 0)
        End If
        Dim fnt As New Font("MS UI Gothic", 20)
        _g.DrawString("score : " + Score.ToString, fnt, Brushes.White, 0, 0)
        For i = 0 To (_a.ID_MAX - 1)
            Dim x As Integer = _a._x(i)
            Dim y As Integer = _a._y(i)
            _g.DrawImage(_a.GetImage(i), x, y)
        Next
        For i = 0 To 2
            _g.DrawImage(_s._img, _s._x(i), _s._y(i))
        Next
        _g.DrawImage(_f._img, _f._x, _f._y)
        For i = 0 To (_e.ID_MAX - 1)
            _g.DrawImage(_e._img, _e._x(i), _e._y(i))
        Next
        _g.DrawImage(_u._img, _u._x, _u._y)
        _g.Dispose()
        For i = 0 To (_ch.ID_MAX - 1)
            Dim x = _ch._c(i)._x
            Dim y = _ch._c(i)._y
            Dim l = _ch._c(i)._life
            If x >= 0 And x < canvas.Width And y >= 0 And y < canvas.Height And l > 0 Then
                canvas.SetPixel(_ch._c(i)._x, _ch._c(i)._y, Color.White)
            End If
        Next
        PictureBox1.Image = canvas
        If _a.CountAlien = 0 Then
            mciSendString("play """ & FANFARE_WAV & """", "", 0, 0)
            SCENE_STATE = CLEAR_SCENE
            _wav.Stop()
        End If
    End Sub
    Private CtrlFlag = 0
    Sub Title()
        _g = Graphics.FromImage(canvas)
        _g.DrawImage(_gxy_title, 0, 0)
        Dim fnt As New Font("MS UI Gothic", 20)
        _g.DrawString("score : " + Score.ToString, fnt, Brushes.White, 0, 0)
        PictureBox1.Image = canvas
        If _c._ctrl Then
            If CtrlFlag = 0 Then
                CtrlFlag = 1
                InitGame()
                SCENE_STATE = GAME_SCENE
                _wav.PlayLooping()
            End If
        Else
            CtrlFlag = 0
        End If
    End Sub
    Const TITLE_SCENE = 0
    Const GAME_SCENE = 1
    Const CLEAR_SCENE = 2
    Private SCENE_STATE As Integer = TITLE_SCENE
    Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Call ControllerCheck()
        If SCENE_STATE = GAME_SCENE Then
            Call GameLoop()
        Else
            Call Title()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Randomize()
        PictureBox1.Width = Me.Width
        PictureBox1.Height = Me.Height
        canvas = New Bitmap(Me.Width, Me.Height)
        _wav = New System.Media.SoundPlayer("..\..\Resources\wpm001.wav")
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
