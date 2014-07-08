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
		Me.environmentLabel = New System.Windows.Forms.Label()
		Me.ftpRadioButton = New System.Windows.Forms.RadioButton()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.otherRadioButton = New System.Windows.Forms.RadioButton()
		Me.emailRadioButton = New System.Windows.Forms.RadioButton()
		Me.otherSourceText = New System.Windows.Forms.TextBox()
		Me.otherLabel = New System.Windows.Forms.Label()
		Me.lblFilenameParseErrors = New System.Windows.Forms.Label()
		Me.lblAlreadyCompletedErrors = New System.Windows.Forms.Label()
		Me.TrackBar1 = New System.Windows.Forms.TrackBar()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.GroupBox1.SuspendLayout()
		CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'lblEventsTitle
		'
		Me.lblEventsTitle.AutoSize = True
		Me.lblEventsTitle.Location = New System.Drawing.Point(4, 324)
		Me.lblEventsTitle.Name = "lblEventsTitle"
		Me.lblEventsTitle.Size = New System.Drawing.Size(105, 13)
		Me.lblEventsTitle.TabIndex = 0
		Me.lblEventsTitle.Text = "# Children Validated:"
		'
		'lvResults
		'
		Me.lvResults.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.lvResults.Location = New System.Drawing.Point(0, 342)
		Me.lvResults.Name = "lvResults"
		Me.lvResults.Size = New System.Drawing.Size(775, 350)
		Me.lvResults.TabIndex = 1
		Me.lvResults.UseCompatibleStateImageBehavior = False
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(4, 90)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(119, 13)
		Me.Label1.TabIndex = 8
		Me.Label1.Text = "Enter source folder:"
		'
		'Button_GetSourceFolder
		'
		Me.Button_GetSourceFolder.Location = New System.Drawing.Point(440, 106)
		Me.Button_GetSourceFolder.Name = "Button_GetSourceFolder"
		Me.Button_GetSourceFolder.Size = New System.Drawing.Size(28, 20)
		Me.Button_GetSourceFolder.TabIndex = 7
		Me.Button_GetSourceFolder.Text = "..."
		Me.Button_GetSourceFolder.UseVisualStyleBackColor = True
		'
		'TextBox_SourceFolder
		'
		Me.TextBox_SourceFolder.Location = New System.Drawing.Point(5, 107)
		Me.TextBox_SourceFolder.Name = "TextBox_SourceFolder"
		Me.TextBox_SourceFolder.Size = New System.Drawing.Size(429, 20)
		Me.TextBox_SourceFolder.TabIndex = 6
		'
		'validatePhotosButton
		'
		Me.validatePhotosButton.Location = New System.Drawing.Point(6, 133)
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
		Me.outputNotInCRMLabel.Location = New System.Drawing.Point(4, 194)
		Me.outputNotInCRMLabel.Name = "outputNotInCRMLabel"
		Me.outputNotInCRMLabel.Size = New System.Drawing.Size(120, 13)
		Me.outputNotInCRMLabel.TabIndex = 12
		Me.outputNotInCRMLabel.Text = "Not in CRM output label"
		'
		'namesNotMatchOutputLabel
		'
		Me.namesNotMatchOutputLabel.AutoSize = True
		Me.namesNotMatchOutputLabel.Location = New System.Drawing.Point(4, 213)
		Me.namesNotMatchOutputLabel.Name = "namesNotMatchOutputLabel"
		Me.namesNotMatchOutputLabel.Size = New System.Drawing.Size(148, 13)
		Me.namesNotMatchOutputLabel.TabIndex = 13
		Me.namesNotMatchOutputLabel.Text = "Names not match output label"
		'
		'projectNotMatchOutputLabel
		'
		Me.projectNotMatchOutputLabel.AutoSize = True
		Me.projectNotMatchOutputLabel.Location = New System.Drawing.Point(4, 232)
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
		Me.resetButton.Location = New System.Drawing.Point(297, 133)
		Me.resetButton.Name = "resetButton"
		Me.resetButton.Size = New System.Drawing.Size(137, 23)
		Me.resetButton.TabIndex = 16
		Me.resetButton.Text = "Reset"
		Me.resetButton.UseVisualStyleBackColor = True
		'
		'environmentLabel
		'
		Me.environmentLabel.AutoSize = True
		Me.environmentLabel.Location = New System.Drawing.Point(314, 90)
		Me.environmentLabel.Name = "environmentLabel"
		Me.environmentLabel.Size = New System.Drawing.Size(130, 13)
		Me.environmentLabel.TabIndex = 17
		Me.environmentLabel.Text = "display target enviornment"
		Me.environmentLabel.Visible = False
		'
		'ftpRadioButton
		'
		Me.ftpRadioButton.AutoSize = True
		Me.ftpRadioButton.Location = New System.Drawing.Point(6, 21)
		Me.ftpRadioButton.Name = "ftpRadioButton"
		Me.ftpRadioButton.Size = New System.Drawing.Size(48, 17)
		Me.ftpRadioButton.TabIndex = 18
		Me.ftpRadioButton.TabStop = True
		Me.ftpRadioButton.Text = "FTP"
		Me.ftpRadioButton.UseVisualStyleBackColor = True
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.otherRadioButton)
		Me.GroupBox1.Controls.Add(Me.emailRadioButton)
		Me.GroupBox1.Controls.Add(Me.ftpRadioButton)
		Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.GroupBox1.Location = New System.Drawing.Point(6, 36)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(298, 51)
		Me.GroupBox1.TabIndex = 19
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Picture Source:"
		'
		'otherRadioButton
		'
		Me.otherRadioButton.AutoSize = True
		Me.otherRadioButton.Location = New System.Drawing.Point(203, 21)
		Me.otherRadioButton.Name = "otherRadioButton"
		Me.otherRadioButton.Size = New System.Drawing.Size(56, 17)
		Me.otherRadioButton.TabIndex = 20
		Me.otherRadioButton.TabStop = True
		Me.otherRadioButton.Text = "Other"
		Me.otherRadioButton.UseVisualStyleBackColor = True
		'
		'emailRadioButton
		'
		Me.emailRadioButton.AutoSize = True
		Me.emailRadioButton.Location = New System.Drawing.Point(102, 21)
		Me.emailRadioButton.Name = "emailRadioButton"
		Me.emailRadioButton.Size = New System.Drawing.Size(55, 17)
		Me.emailRadioButton.TabIndex = 19
		Me.emailRadioButton.TabStop = True
		Me.emailRadioButton.Text = "Email"
		Me.emailRadioButton.UseVisualStyleBackColor = True
		'
		'otherSourceText
		'
		Me.otherSourceText.Location = New System.Drawing.Point(310, 59)
		Me.otherSourceText.Name = "otherSourceText"
		Me.otherSourceText.Size = New System.Drawing.Size(151, 20)
		Me.otherSourceText.TabIndex = 20
		Me.otherSourceText.Visible = False
		'
		'otherLabel
		'
		Me.otherLabel.AutoSize = True
		Me.otherLabel.Location = New System.Drawing.Point(310, 39)
		Me.otherLabel.Name = "otherLabel"
		Me.otherLabel.Size = New System.Drawing.Size(36, 13)
		Me.otherLabel.TabIndex = 21
		Me.otherLabel.Text = "Other:"
		Me.otherLabel.Visible = False
		'
		'lblFilenameParseErrors
		'
		Me.lblFilenameParseErrors.AutoSize = True
		Me.lblFilenameParseErrors.Location = New System.Drawing.Point(4, 251)
		Me.lblFilenameParseErrors.Name = "lblFilenameParseErrors"
		Me.lblFilenameParseErrors.Size = New System.Drawing.Size(132, 13)
		Me.lblFilenameParseErrors.TabIndex = 22
		Me.lblFilenameParseErrors.Text = "Filename parse errors label"
		'
		'lblAlreadyCompletedErrors
		'
		Me.lblAlreadyCompletedErrors.AutoSize = True
		Me.lblAlreadyCompletedErrors.Location = New System.Drawing.Point(4, 270)
		Me.lblAlreadyCompletedErrors.Name = "lblAlreadyCompletedErrors"
		Me.lblAlreadyCompletedErrors.Size = New System.Drawing.Size(148, 13)
		Me.lblAlreadyCompletedErrors.TabIndex = 23
		Me.lblAlreadyCompletedErrors.Text = "Already completed errors label"
		'
		'TrackBar1
		'
		Me.TrackBar1.LargeChange = 1
		Me.TrackBar1.Location = New System.Drawing.Point(532, 7)
		Me.TrackBar1.Maximum = 2
		Me.TrackBar1.Name = "TrackBar1"
		Me.TrackBar1.Size = New System.Drawing.Size(104, 45)
		Me.TrackBar1.TabIndex = 24
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(474, 16)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(56, 13)
		Me.Label3.TabIndex = 25
		Me.Label3.Text = "Less Strict"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(642, 16)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(58, 13)
		Me.Label4.TabIndex = 26
		Me.Label4.Text = "More Strict"
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(775, 692)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.TrackBar1)
		Me.Controls.Add(Me.lblAlreadyCompletedErrors)
		Me.Controls.Add(Me.lblFilenameParseErrors)
		Me.Controls.Add(Me.otherLabel)
		Me.Controls.Add(Me.otherSourceText)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.environmentLabel)
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
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
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
	Friend WithEvents environmentLabel As System.Windows.Forms.Label
	Friend WithEvents ftpRadioButton As System.Windows.Forms.RadioButton
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents otherRadioButton As System.Windows.Forms.RadioButton
	Friend WithEvents emailRadioButton As System.Windows.Forms.RadioButton
	Friend WithEvents otherSourceText As System.Windows.Forms.TextBox
	Friend WithEvents otherLabel As System.Windows.Forms.Label
	Friend WithEvents lblFilenameParseErrors As System.Windows.Forms.Label
	Friend WithEvents lblAlreadyCompletedErrors As System.Windows.Forms.Label
	Friend WithEvents TrackBar1 As System.Windows.Forms.TrackBar
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents Label4 As System.Windows.Forms.Label

End Class
