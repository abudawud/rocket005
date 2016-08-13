Imports OpenTK
Imports OpenTK.Graphics
Imports OpenTK.Graphics.OpenGL
Imports OpenTK.Input
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.IO.Ports
Imports System.Threading


Public Class formViewer
    Dim i As Integer = 0
    Dim getI As Integer
    Dim receivedData As String = ""
    Dim gyroxLog(100) As Integer
    Dim gyroyLog(100) As Integer
    Dim gyrozLog(100) As Integer
    Dim accexLog(100) As Integer
    Dim acceyLog(100) As Integer
    Dim accezLog(100) As Integer
    Dim compxLog(100) As Integer
    Dim compyLog(100) As Integer
    Dim compzLog(100) As Integer
    Dim comphLog(100) As Integer
    Dim tempLog(100) As Integer
    Dim presLog(100) As Integer
    Dim atmLog(100) As Integer
    Dim altLog(100) As Integer
    ' Dim buffer(30) As Byte
    Delegate Sub SetTextCallback(ByVal [packet] As String)

    Private Sub modelViewer_Load(sender As Object, e As EventArgs) Handles modelViewer.Load
        GL.ClearColor(Color.White)
        GL.Viewport(0, 0, modelViewer.Width, modelViewer.Height)
        GL.MatrixMode(MatrixMode.Projection)
        GL.LoadIdentity()
        GL.Ortho(-modelViewer.Width, modelViewer.Width, -modelViewer.Height, modelViewer.Height, -1, 1)
        GL.MatrixMode(MatrixMode.Modelview)
        GL.LoadIdentity()

        CheckBox1.Checked = True
        CheckBox2.Checked = True


        tbXMin.Text = -modelViewer.Width
        tbXMax.Text = modelViewer.Width
        tbYMin.Text = -modelViewer.Height
        tbYMax.Text = modelViewer.Height
        rectangleDraw()


    End Sub

    Private Sub modelViewer_Paint(sender As Object, e As PaintEventArgs) Handles modelViewer.Paint
        GL.Clear(ClearBufferMask.ColorBufferBit)
        GL.Clear(ClearBufferMask.DepthBufferBit)

    End Sub

    Private Sub rectangleDraw()
        If CheckBox2.Checked Then
            GL.Color3(0, 0, 0)
            GL.Begin(BeginMode.LineLoop)
            GL.Vertex2(-450, -450)
            GL.Vertex2(-250, -450)
            GL.Vertex2(-250, -150)
            GL.Vertex2(-450, -150)
            GL.End()
            GL.Begin(BeginMode.LineLoop)
            GL.Vertex2(50, 50)
            GL.Vertex2(350, 50)
            GL.Vertex2(350, 350)
            GL.Vertex2(50, 350)
            GL.End()
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        modelViewer.Invalidate()

    End Sub

    Private Sub updateChart(ByVal gyrox As Integer, ByVal gyroy As Integer, ByVal gyroz As Integer, ByVal accex As Integer, ByVal accey As Integer, ByVal accez As Integer, ByVal compx As Integer, ByVal compy As Integer, ByVal compz As Integer, ByVal comph As Integer, ByVal temp As Integer, ByVal pres As Integer, ByVal atm As Integer, ByVal alt As Integer)
        For i As Integer = 0 To 99
            gyroxLog(i) = gyroxLog(i + 1)
            gyroyLog(i) = gyroyLog(i + 1)
            gyrozLog(i) = gyrozLog(i + 1)
            accexLog(i) = accexLog(i + 1)
            acceyLog(i) = acceyLog(i + 1)
            accezLog(i) = accezLog(i + 1)
            compxLog(i) = compxLog(i + 1)
            compyLog(i) = compyLog(i + 1)
            compzLog(i) = compzLog(i + 1)
            comphLog(i) = comphLog(i + 1)
            tempLog(i) = tempLog(i + 1)
            presLog(i) = presLog(i + 1)
            atmLog(i) = atmLog(i + 1)
            altLog(i) = altLog(i + 1)
        Next

        gyroxLog(100) = gyrox
        gyroyLog(100) = gyroy
        gyrozLog(100) = gyroz
        accexLog(100) = accex
        acceyLog(100) = accey
        accezLog(100) = accez
        compxLog(100) = compx
        compyLog(100) = compy
        compzLog(100) = compz
        comphLog(100) = comph
        tempLog(100) = temp
        presLog(100) = pres
        atmLog(100) = atm
        altLog(100) = alt

        chGyro.Series("GyroX").Points.Clear()
        chGyro.Series("GyroY").Points.Clear()
        chGyro.Series("GyroZ").Points.Clear()
        chAcce.Series("AcceX").Points.Clear()
        chAcce.Series("AcceY").Points.Clear()
        chAcce.Series("AcceZ").Points.Clear()
        chComp.Series("CompX").Points.Clear()
        chComp.Series("CompY").Points.Clear()
        chComp.Series("CompZ").Points.Clear()
        chHead.Series("Heading").Points.Clear()
        chTemp.Series("Temperature").Points.Clear()
        chPres.Series("Pressure").Points.Clear()
        chAtm.Series("ATM").Points.Clear()
        chAlt.Series("Altitude").Points.Clear()

        For i As Integer = 0 To 100

            chGyro.Series("GyroX").Points.AddXY(100 - i, gyroxLog(i))
            chGyro.Series("GyroY").Points.AddXY(100 - i, gyroyLog(i))
            chGyro.Series("GyroZ").Points.AddXY(100 - i, gyrozLog(i))
            chAcce.Series("AcceX").Points.AddXY(100 - i, accexLog(i))
            chAcce.Series("AcceY").Points.AddXY(100 - i, acceyLog(i))
            chAcce.Series("AcceZ").Points.AddXY(100 - i, accezLog(i))
            chComp.Series("CompX").Points.AddXY(100 - i, compxLog(i))
            chComp.Series("CompY").Points.AddXY(100 - i, compyLog(i))
            chComp.Series("CompZ").Points.AddXY(100 - i, compzLog(i))
            chHead.Series("Heading").Points.AddXY(100 - i, comphLog(i))
            chTemp.Series("Temperature").Points.AddXY(100 - i, tempLog(i))
            chPres.Series("Pressure").Points.AddXY(100 - i, presLog(i))
            chAtm.Series("ATM").Points.AddXY(100 - i, atmLog(i))
            chAlt.Series("Altitude").Points.AddXY(100 - i, altLog(i))
        Next
    End Sub

    'Private Sub updateChart2(ByVal gyrox As Integer, ByVal gyroy As Integer, ByVal gyroz As Integer, ByVal accex As Integer, ByVal accey As Integer, ByVal accez As Integer, ByVal compx As Integer, ByVal compy As Integer, ByVal compz As Integer, ByVal comph As Integer, ByVal temp As Integer, ByVal pres As Integer, ByVal atm As Integer, ByVal alt As Integer)
    Private Sub updateChart2(ByVal gyrox As Integer, ByVal gyroy As Integer, ByVal gyroz As Integer, ByVal accex As Integer, ByVal accey As Integer, ByVal accez As Integer)

        For i As Integer = 0 To 99
            gyroxLog(i) = gyroxLog(i + 1)
            gyroyLog(i) = gyroyLog(i + 1)
            gyrozLog(i) = gyrozLog(i + 1)
            accexLog(i) = accexLog(i + 1)
            acceyLog(i) = acceyLog(i + 1)
            accezLog(i) = accezLog(i + 1)
            'compxLog(i) = compxLog(i + 1)
            'compyLog(i) = compyLog(i + 1)
            'compzLog(i) = compzLog(i + 1)
            'comphLog(i) = comphLog(i + 1)
            'tempLog(i) = tempLog(i + 1)
            'presLog(i) = presLog(i + 1)
            'atmLog(i) = atmLog(i + 1)
            'altLog(i) = altLog(i + 1)
        Next
        gyroxLog(100) = gyrox
        gyroyLog(100) = gyroy
        gyrozLog(100) = gyroz
        accexLog(100) = accex
        acceyLog(100) = accey
        accezLog(100) = accez
        'compxLog(100) = compx
        'compyLog(100) = compy
        'compzLog(100) = compz
        'comphLog(100) = comph
        'tempLog(100) = temp
        'presLog(100) = pres
        'atmLog(100) = atm
        'altLog(100) = alt

        chGyro.Series("GyroXa").Points.Clear()
        chGyro.Series("GyroXv").Points.Clear()
        chGyro.Series("GyroXp").Points.Clear()
        chAcce.Series("AcceX").Points.Clear()
        chAcce.Series("AcceY").Points.Clear()
        chAcce.Series("AcceZ").Points.Clear()
        'chComp.Series("CompX").Points.Clear()
        'chComp.Series("CompY").Points.Clear()
        'chComp.Series("CompZ").Points.Clear()
        'chHead.Series("Heading").Points.Clear()
        'chTemp.Series("Temperature").Points.Clear()
        'chPres.Series("Pressure").Points.Clear()
        'chAtm.Series("ATM").Points.Clear()
        'chAlt.Series("Altitude").Points.Clear()

        For i As Integer = 0 To 100
            chGyro.Series("GyroXa").Points.AddXY(100 - i, gyroxLog(i))
            chGyro.Series("GyroXv").Points.AddXY(100 - i, gyroyLog(i))
            chGyro.Series("GyroXp").Points.AddXY(100 - i, gyrozLog(i))
            chAcce.Series("AcceX").Points.AddXY(100 - i, accexLog(i))
            chAcce.Series("AcceY").Points.AddXY(100 - i, acceyLog(i))
            chAcce.Series("AcceZ").Points.AddXY(100 - i, accezLog(i))
            'chComp.Series("CompX").Points.AddXY(100 - i, compxLog(i))
            'chComp.Series("CompY").Points.AddXY(100 - i, compyLog(i))
            'chComp.Series("CompZ").Points.AddXY(100 - i, compzLog(i))
            'chHead.Series("Heading").Points.AddXY(100 - i, comphLog(i))
            'chTemp.Series("Temperature").Points.AddXY(100 - i, tempLog(i))
            'chPres.Series("Pressure").Points.AddXY(100 - i, presLog(i))
            'chAtm.Series("ATM").Points.AddXY(100 - i, atmLog(i))
            'chAlt.Series("Altitude").Points.AddXY(100 - i, altLog(i))
        Next
    End Sub

    Private Sub receivedText(ByVal [packet] As String)
        Dim Head,
            ID,
            gx, gy, gz,
            ax, ay, az,
            comp,
        i As Integer
        Dim data As String

        'compares the ID of the creating Thread to the ID of the calling Thread
        Dim buffer() As String = packet.Split(" ")
        If Me.tbReceivedLog.InvokeRequired Then
            Dim x As New SetTextCallback(AddressOf receivedText)
            Invoke(x, New Object() {[packet]})
        Else
            Try
                'code
                ' tbInbox.Text = ""
                'tbInbox.Text = "" & ""
                'For i = 0 To 24
                'tbInbox.Text &= toSigned(buffer(i))

                'Next
                'Head = Asc(buffer(0))
                'ID = Asc(buffer(1)) * 100 + Asc(buffer(2)) * 10 + Asc(buffer(3))

                'ax = toSigned(Asc(buffer(4))) * 100 + toSigned(Asc(buffer(5))) * 10 + toSigned(Asc(buffer(6)))
                'ay = toSigned(Asc(buffer(7))) * 100 + toSigned(Asc(buffer(8))) * 10 + toSigned(Asc(buffer(9)))
                'az = toSigned(Asc(buffer(10))) * 100 + toSigned(Asc(buffer(11))) * 10 + toSigned(Asc(buffer(12)))

                'gx = toSigned(Asc(buffer(13))) * 100 + toSigned(Asc(buffer(14))) * 10 + toSigned(Asc(buffer(15)))
                'gy = toSigned(Asc(buffer(16))) * 100 + toSigned(Asc(buffer(17))) * 10 + toSigned(Asc(buffer(18)))
                'gz = toSigned(Asc(buffer(19))) * 100 + toSigned(Asc(buffer(20))) * 10 + toSigned(Asc(buffer(21)))

                'comp = toSigned(Asc(buffer(22))) * 100 + toSigned(Asc(buffer(23))) * 10 + toSigned(Asc(buffer(24)))

                'tbHeading.Text = Head
                'tbkode.Text = ID
                tbGyroX.Text = buffer(4)
                tbGyroY.Text = buffer(5)
                tbGyroZ.Text = buffer(6)
                tbAcceX.Text = buffer(1)
                tbAcceY.Text = buffer(2)
                tbAcceZ.Text = buffer(3)
                tbCompX.Text = buffer(7)

                tbReceivedLog.AppendText(buffer(1))
                'tbReceivedLog.AppendText(vbCrLf)


                Int32.TryParse(buffer(1), ax)
                Int32.TryParse(buffer(2), ay)
                Int32.TryParse(buffer(3), az)
                Int32.TryParse(buffer(4), gx)
                Int32.TryParse(buffer(5), gy)
                Int32.TryParse(buffer(6), gz)

                updateChart2(gx, gy, gz, ax, ay, az)
            Catch ex As Exception

            End Try
        End If

    End Sub
    Private Sub getPorts()
        Dim ports As String() = SerialPort.GetPortNames()
        Dim port As String
        cbPorts.Items.Clear()
        For Each port In ports
            cbPorts.Items.Add(port)
        Next

        cbPorts.SelectedIndex = 0
        cbBaudRates.SelectedIndex = 0
    End Sub

    Private Sub formViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        spInput.PortName = "COM6"
        spInput.BaudRate = "115200"
        spInput.Parity = Parity.None
        spInput.StopBits = StopBits.One
        spInput.DataBits = 8
        spInput.ReadTimeout = 10000
        'Input.Open()
    End Sub

    Private Sub spInput_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles spInput.DataReceived
        'receivedText(spInput.ReadExisting())
        'showdata(spInput.Read(buffer, 0, 25))
        'receivedText(spInput.Read(buffer, 0, 30))
        receivedText(spInput.ReadLine())
    End Sub

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click

        Try
            If btnConnect.Text = "Connect" Then

                If Not spInput.IsOpen Then

                    spInput.PortName = cbPorts.Text
                    spInput.BaudRate = cbBaudRates.Text
                    cbPorts.Enabled = False
                    cbBaudRates.Enabled = False
                    spInput.Parity = Parity.None
                    spInput.StopBits = StopBits.One
                    spInput.DataBits = 8
                    spInput.ReadTimeout = 10000000
                    spInput.Open()
                    btnConnect.Text = "Disconnect"
                Else
                    MsgBox("kkk")
                End If
            Else

                spInput.Close()
                cbPorts.Enabled = True
                cbBaudRates.Enabled = True
                btnConnect.Text = "Connect"
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnRefreshPort_Click(sender As Object, e As EventArgs) Handles btnRefreshPort.Click
        getPorts()
    End Sub

    Private Sub tbkode_TextChanged(sender As Object, e As EventArgs) Handles tbkode.TextChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        Dim num As Integer = 10
        tbHeading.Text = num


    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click
        'tbATM.Text = buffer(0)
        'tbAltitude.Text = buffer(1)
        'tbAltitude.Text = spInput.ReadBufferSize()

    End Sub
    Private Sub showdata(ByVal a As Integer)
        ' tbHeading.Text = buffer(0)
    End Sub
    Function toSigned(ByVal src As Int16) As Integer
        If (src > 128) Then
            Return (src - 256)
        ElseIf (src < 128 And src <> 0) Then
            Return src
        Else
            Return 0
        End If

    End Function
End Class
