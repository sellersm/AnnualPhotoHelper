<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.lblEventsTitle = New System.Windows.Forms.Label()
		Me.lvResults = New System.Windows.Forms.ListView()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Button_GetSourceFolder = New System.Windows.Forms.Button()
		Me.TextBox_SourceFolder = New System.Windows.Forms.TextBox()
		Me.FolderBrowser_GetSourceFolder = New System.Windows.Forms.FolderBrowserDialog()
		Me.validatePhotosButton = New System.Windows.Forms.Button()
		Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.outputNotInCRMLabel = New System.Windows.Forms.Label()
		Me.namesNotMatchOutputLabel = New System.Windows.Forms.Label()
		Me.projectNotMatchOutputLabel = New System.Windows.Forms.Label()
		Me.nameValidationOverrideCheckBox = New System.Windows.Forms.CheckBox()
		Me.resetButton = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'lblEventsTitle
		'
		Me.lblEventsTitle.AutoSize = True
		Me.lblEventsTitle.Location = New System.Drawing.Point(4, 197)
		Me.lblEventsTitle.Name = "lblEventsTitle"
		Me.lblEventsTitle.Size = New System.Drawing.Size(97, 13)
		Me.lblEventsTitle.TabIndex = 0
		Me.lblEventsTitle.Text = "Validated Chid List:"
		'
		'lvResults
		'
		Me.lvResults.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.lvResults.Location = New System.Drawing.Point(0, 214)
		Me.lvResults.Name = "lvResults"
		Me.lvResults.Size = New System.Drawing.Size(694, 212)
		Me.lvResults.TabIndex = 1
		Me.lvResults.UseCompatibleStateImageBehavior = False
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(4, 49)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(119, 13)
		Me.Label1.TabIndex = 8
		Me.Label1.Text = "Enter source folder:"
		'
		'Button_GetSourceFolder
		'
		Me.Button_GetSourceFolder.Location = New System.Drawing.Point(498, 66)
		Me.Button_GetSourceFolder.Name = "Button_GetSourceFolder"
		Me.Button_GetSourceFolder.Size = New System.Drawing.Size(28, 20)
		Me.Button_GetSourceFolder.TabIndex = 7
		Me.Button_GetSourceFolder.Text = "..."
		Me.Button_GetSourceFolder.UseVisualStyleBackColor = True
		'
		'TextBox_SourceFolder
		'
		Me.TextBox_SourceFolder.Location = New System.Drawing.Point(5, 66)
		Me.TextBox_SourceFolder.Name = "TextBox_SourceFolder"
		Me.TextBox_SourceFolder.Size = New System.Drawing.Size(487, 20)
		Me.TextBox_SourceFolder.TabIndex = 6
		'
		'validatePhotosButton
		'
		Me.validatePhotosButton.Location = New System.Drawing.Point(6, 92)
		Me.validatePhotosButton.Name = "validatePhotosButton"
		Me.validatePhotosButton.Size = New System.Drawing.Size(212, 23)
		Me.validatePhotosButton.TabIndex = 9
		Me.validatePhotosButton.Text = "Validate && Complete Photo Interactions"
		Me.validatePhotosButton.UseVisualStyleBackColor = True
		'
		'DateTimePicker1
		'
		Me.DateTimePicker1.Location = New System.Drawing.Point(104, 12)
		Me.DateTimePicker1.Name = "DateTimePicker1"
		Me.DateTimePicker1.Size = New System.Drawing.Size(200, 20)
		Me.DateTimePicker1.TabIndex = 10
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(4, 18)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(94, 13)
		Me.Label2.TabIndex = 11
		Me.Label2.Text = "Complete Date:"
		'
		'outputNotInCRMLabel
		'
		Me.outputNotInCRMLabel.AutoSize = True
		Me.outputNotInCRMLabel.Location = New System.Drawing.Point(4, 130)
		Me.outputNotInCRMLabel.Name = "outputNotInCRMLabel"
		Me.outputNotInCRMLabel.Size = New System.Drawing.Size(120, 13)
		Me.outputNotInCRMLabel.TabIndex = 12
		Me.outputNotInCRMLabel.Text = "Not in CRM output label"
		'
		'namesNotMatchOutputLabel
		'
		Me.namesNotMatchOutputLabel.AutoSize = True
		Me.namesNotMatchOutputLabel.Location = New System.Drawing.Point(4, 148)
		Me.namesNotMatchOutputLabel.Name = "namesNotMatchOutputLabel"
		Me.namesNotMatchOutputLabel.Size = New System.Drawing.Size(148, 13)
		Me.namesNotMatchOutputLabel.TabIndex = 13
		Me.namesNotMatchOutputLabel.Text = "Names not match output label"
		'
		'projectNotMatchOutputLabel
		'
		Me.projectNotMatchOutputLabel.AutoSize = True
		Me.projectNotMatchOutputLabel.Location = New System.Drawing.Point(4, 166)
		Me.projectNotMatchOutputLabel.Name = "projectNotMatchOutputLabel"
		Me.projectNotMatchOutputLabel.Size = New System.Drawing.Size(148, 13)
		Me.projectNotMatchOutputLabel.TabIndex = 14
		Me.projectNotMatchOutputLabel.Text = "Project not match output label"
		'
		'nameValidationOverrideCheckBox
		'
		Me.nameValidationOverrideCheckBox.AutoSize = True
		Me.nameValidationOverrideCheckBox.Location = New System.Drawing.Point(317, 15)
		Me.nameValidationOverrideCheckBox.Name = "nameValidationOverrideCheckBox"
		Me.nameValidationOverrideCheckBox.Size = New System.Drawing.Size(151, 17)
		Me.nameValidationOverrideCheckBox.TabIndex = 15
		Me.nameValidationOverrideCheckBox.Text = "Override Name validation?"
		Me.nameValidationOverrideCheckBox.UseVisualStyleBackColor = True
		'
		'resetButton
		'
		Me.resetButton.Location = New System.Drawing.Point(224, 92)
		Me.resetButton.Name = "resetButton"
		Me.resetButton.Size = New System.Drawing.Size(137, 23)
		Me.resetButton.TabIndex = 16
		Me.resetButton.Text = "Reset"
		Me.resetButton.UseVisualStyleBackColor = True
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(694, 426)
		Me.Controls.Add(Me.resetButton)
		Me.Controls.Add(Me.nameValidationOverrideCheckBox)
		Me.Controls.Add(Me.projectNotMatchOutputLabel)
		Me.Controls.Add(Me.namesNotMatchOutputLabel)
		Me.Controls.Add(Me.outputNotInCRMLabel)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.DateTimePicker1)
		Me.Controls.Add(Me.validatePhotosButton)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.Button_GetSourceFolder)
		Me.Controls.Add(Me.TextBox_SourceFolder)
		Me.Controls.Add(Me.lvResults)
		Me.Controls.Add(Me.lblEventsTitle)
		Me.Name = "Form1"
		Me.Text = "Annual Photo Helper"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents lblEventsTitle As System.Windows.Forms.Label
    Friend WithEvents lvResults As System.Windows.Forms.ListView
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents Button_GetSourceFolder As System.Windows.Forms.Button
	Friend WithEvents TextBox_SourceFolder As System.Windows.Forms.TextBox
	Friend WithEvents FolderBrowser_GetSourceFolder As System.Windows.Forms.FolderBrowserDialog
	Friend WithEvents validatePhotosButton As System.Windows.Forms.Button
	Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents outputNotInCRMLabel As System.Windows.Forms.Label
	Friend WithEvents namesNotMatchOutputLabel As System.Windows.Forms.Label
	Friend WithEvents projectNotMatchOutputLabel As System.Windows.Forms.Label
	Friend WithEvents nameValidationOverrideCheckBox As System.Windows.Forms.CheckBox
	Friend WithEvents resetButton As System.Windows.Forms.Button

End Class
