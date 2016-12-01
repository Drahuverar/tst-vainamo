Public Class Form1

    ' One control named "JobBox" (I used a picbox), multiple panels named anything you want. 

    Dim isDrag As Boolean = False
    Dim lastparent As String
    Dim lastlocation As Point

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' move JobBox into one of our panels
        ' JobBox.Parent = Panel1
        ' JobBox.Location = New Point(10, 10)
    End Sub

    ' Start Drag
    Private Sub JobBox_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles JobBox.MouseDown
        If (e.Button = MouseButtons.Left) Then
            isDrag = True
            Me.Cursor = Cursors.Hand
            ' rem current parent & location incase we need to move control back.
            lastparent = JobBox.Parent.Name
            lastlocation = JobBox.Location
            ' move control to form/front so we can drag it around.
            JobBox.Parent = Me
            JobBox.BringToFront()
        End If
    End Sub

    ' dragging
    Private Sub JobBox_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles JobBox.MouseMove
        If isDrag = True Then
            JobBox.Left = (JobBox.Left + e.X)
            JobBox.Top = (JobBox.Top + e.Y)
        End If
    End Sub

    ' dropped
    Private Sub JobBox_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles JobBox.MouseUp
        isDrag = False
        Me.Cursor = Cursors.Default
        Dim i As Integer
        Dim foundNewHome As Boolean = False
        Dim controlRectangle As Rectangle
        ' where are we now?
        Dim startPoint As Point = JobBox.PointToScreen(New Point(e.X, e.Y))
        Dim theRectangle As Rectangle = New Rectangle(startPoint.X, startPoint.Y, 0, 0)
        ' what control did we get dropped on?
        For i = 0 To Controls.Count - 1
            ' Allow drop in panels only!
            If TypeOf Controls(i) Is Panel Then ' note: 1-layer, doesn't support panels inside panels!
                controlRectangle = Controls(i).RectangleToScreen(Controls(i).ClientRectangle)
                If controlRectangle.IntersectsWith(theRectangle) Then
                    ' get drop location of the new container
                    Dim containerLocation As Point = PointToScreen(Controls(i).Location)
                    Dim dropTarget As Point = New Point(Cursor.Position.X - containerLocation.X, Cursor.Position.Y - containerLocation.Y)
                    ' move control into new container at dropped location.
                    JobBox.Parent = Controls(i)
                    JobBox.Location = dropTarget
                    JobBox.BringToFront()
                    foundNewHome = True
                    Exit For
                End If
            End If
        Next
        If foundNewHome = False Then
            ' control was dropped on form or non-panel, so move back to original container.
            'MessageBox.Show("Can't drop here!")
            JobBox.Parent = Me.Controls(lastparent)
            JobBox.Location = lastlocation
            JobBox.BringToFront()
        End If
    End Sub


End Class