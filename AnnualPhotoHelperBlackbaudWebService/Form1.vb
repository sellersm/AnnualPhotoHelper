Imports Blackbaud.AppFx.WebAPI
Imports Blackbaud.AppFx.XmlTypes
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports System.IO
'Imports nullpointer.Metaphone
Imports NameComparisonUtility

Public Class Form1

#Region "Private definitions"
	'The files that will be validated, have a name like this:
	'IN-131 C102900 Revathi Sait 2014.JPG
	'<ProjectID> <Lookupid> <Firstname> <Lastname>

	'The Blackbaud.AppFx.WebAPI.dll is used to access the Infinity application such as 
	' Blackbaud CRM (BBEC), Altru, and Research Point (RP) and handles communication with the 
	' AppFxWebService.asmx on your behalf.  If you use this dll to access the Infinity application 
	' you do NOT need a web service reference to AppFxWebService.asmx.  
	'Declare a variable of type Blackbaud.AppFx.WebAPI.ServiceProxy.AppFxWebService to represent
	' your proxy for the communication mechanism to Infinity application.
	Private _appFx As Blackbaud.AppFx.WebAPI.ServiceProxy.AppFxWebService

	Private _childDataListId As Guid = New Guid("1a347536-d326-4f1f-8c6b-87d8320b4b18")					 'Child Data For Photo API Data List
	Private _childDataListByXmlId As Guid = New Guid("6136072e-f5b4-4367-8121-f45b9c7a07e6")			 'Get Child List For Photo API Data List
	Private _completeInteractionsRecordOpId As Guid = New Guid("18afc1ad-dd5f-4a3c-9c9b-206943857edf")	 ' this is the test record op that does nothing: "7869525e-1b2d-41e8-b68c-df9d6c0ac714"			 'Record Op ID
	Private _interactionExceptionsDataListId As Guid = New Guid("e2d58c9d-df33-4f49-835b-f2b7db3846d1")	 'Photo Interaction Exceptions Data List
	Private _photoHelperSessionId As Guid

	Private _myCred As System.Net.ICredentials
	Private _clientAppInfoHeader As Blackbaud.AppFx.WebAPI.ServiceProxy.ClientAppInfoHeader

	Private _notInCrmList As New ChildPhotoCollection
	Private _projectIdNotMatchList As New ChildPhotoCollection
	Private _nameNotMatchList As New ChildPhotoCollection
	Private _childValidatedList As New ChildPhotoCollection
	'Private _interactionExceptions As ChildPhotoCollection
	Private _alreadyCompletedList As New ChildPhotoCollection
	Private _interactionNotFoundList As New ChildPhotoCollection
	Private _unusableAlreadyCompletedList As New ChildPhotoCollection
	Private _fileNameParseErrorList As New ChildPhotoCollection

	'these threshold values for the strictness slider values used for comparing against the JW Proximity and DL Distance:
	Dim _jwProximityLow As Double = 0.75D  ' 0.7D
	Dim _jwProximityMedium As Double = 0.85D ' 0.8D
	Dim _jwProximityHigh As Double = 0.9D ' 0.875D

	Dim _dlDistanceLow As Integer = 4
	Dim _dlDistanceMedium As Integer = 3
	Dim _dlDistanceHigh As Integer = 1

	Dim _jwProximityThreshold As Double
	Dim _dlDistanceThreshold As Integer


	'****** DEBUGGING USE ONLY!  *****
	' Set from the Config file (My.Settings):
	Private _debugging As Boolean = False

	Private _interactionComment As String = String.Empty

	'The following are now set in the GetSetConfigSettingsValues() method, so what's shown here is overwritten at form load time:
	Private _notincrmfoldername As String = "Children_Not_In_CRM\"
	Private _unmatchedprojectfoldername As String = "Incorrect_Project_ID\"
	Private _unmatchednamefoldername As String = "Names_Not_Match\"
	Private _alreadycompletedfoldername As String = "Already_Completed_Exceptions\"
	Private _unusablealreadycompletedfoldername As String = "Unusable_Already_Completed_Exceptions\"
	Private _interactionnotfoundfoldername As String = "Interactions_Not_In_CRM_Exceptions\"
	Private _fileNameParseErrorFolderName As String = "Filename_Parse_Error\"
	Private _completedFolderName As String = "Children_Validated\"

	Private _recordopname As String = "Child Interaction Annual Photo Update Record Operation"	 ' "Completes Annual Photo Interactions Record Op"
	Private _photoyearfile As String = "2014.JPG"
	Private _photoyearfile_lowercase As String = "2014.jpg"
	Private _photoyearfile_withspace As String = "2014 .JPG"
	Private _securelyStoredUserName As String = "PhotoAPIUser21195D"	' "MobileServices21195p"
	Private _securelyStoredPassword As String = "P@ssword1"						'"7Fny8kbmDxr4"
	Private _dbName As String = String.Empty
	Private _appFxURL As String = String.Empty
#End Region

	Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		InitializeEverything()
		resetButton.Enabled = False
	End Sub

	Private Sub InitializeAppFxWebService()

		Try
			'Display hourglass during appfx web service calls
			Cursor.Current = Cursors.WaitCursor
			Cursor.Show()

			'ensure that we have the config values before continuing:
			If String.IsNullOrEmpty(_appFxURL) Then
				Throw New ArgumentNullException("WebServiceURL", "The WebAPI URL is empty! Unable to get value from App.Config file.")
			End If

			If String.IsNullOrEmpty(_dbName) Then
				Throw New ArgumentNullException("DBName", "The DB Name is empty! Unable to get value from App.Config file.")
			End If


			'Instantiate the proxy to the Infinity application
			_appFx = New Blackbaud.AppFx.WebAPI.ServiceProxy.AppFxWebService

			'When running local, don't need credentials!!
			'Grab the network credentials.  See GetNetworkCredentials() for details.
			Dim NetworkCredential As New System.Net.NetworkCredential(_securelyStoredUserName, _securelyStoredPassword)
			'_myCred = GetNetworkCredentials()
			_myCred = NetworkCredential

			'Set the credentials for the service proxy.
			_appFx.Credentials = _myCred

			' Testing for local credentials:
			'_appFx.Credentials = System.Net.CredentialCache.DefaultCredentials
			'_appFx.UseDefaultCredentials = True

			'Be sure to store the url to the AppFxWebService.asmx in a configuration file on your client.  Don't hard code the url.
			'AppFxWebService.asmx is the single SOAP endpoint for accessing all application features.
			'Grab the url from the Blackbaud AppFX Server HTTP Endpoint Reference (endpointhelp.html).

			_appFx.Url = _appFxURL	' "https://bbecdev03bo3.blackbaudhosting.com/21195D_c1b77079-b579-4bfc-9532-4d895d9e5891/appfxwebservice.asmx" ' DEV environment

			'_appFx.Url = "http://localhost/ocmcrm293/appfxwebservice.asmx"   'local
			'_appFx.Url = "https://bbisec04pro.blackbaudhosting.com/21195P_a4c313dc-543a-45d5-b64d-72e8f900c2df/appfxwebservice.asmx" ' PROD environment

			'Provide the ClientAppName which will be logged in the Infinity database and used for auditing purposes.
			'Use a client application name that identifies your custom client software from the web shell and any other client user interfaces.
			_clientAppInfoHeader = New Blackbaud.AppFx.WebAPI.ServiceProxy.ClientAppInfoHeader()
			_clientAppInfoHeader.ClientAppName = "CustomEventManager"
			_clientAppInfoHeader.REDatabaseToUse = _dbName	 ' "21195D"	'DEV
			'_clientAppInfoHeader.REDatabaseToUse = _dbName ' "BBInfinity"	 'local = BBInfinity  ' "21195P"		'PROD

		Catch ex As Exception
			MsgBox(ex.Message.ToString)

		Finally
			'Hide hourglass after api call
			Cursor.Current = Cursors.Default
			Cursor.Show()
		End Try
	End Sub

	Private Sub GetConstituentID(ByVal BBISUserName As String)

		'Display hourglass during appfx web service calls
		Cursor.Current = Cursors.WaitCursor
		Cursor.Show()

		Dim Req As New Blackbaud.AppFx.WebAPI.ServiceProxy.DataListLoadRequest

		Dim fvSet As New Blackbaud.AppFx.XmlTypes.DataForms.DataFormFieldValueSet

		Dim dfi As New Blackbaud.AppFx.XmlTypes.DataForms.DataFormItem
		Dim Reply As New Blackbaud.AppFx.WebAPI.ServiceProxy.DataListLoadReply

		Req.ClientAppInfo = _clientAppInfoHeader
		Req.DataListID = New System.Guid("b85106bd-b23e-4a46-be2a-f3ae98815240") '"b85106bd-b23e-4a46-be2a-f3ae98815240")		
		Req.ContextRecordID = "00000000-0000-0000-0000-000000000000" 'Needs a guid in the Context Record ID, but this is ignored in the data list

		'Pass in the BBIS Username to find
		fvSet.Add(New Blackbaud.AppFx.XmlTypes.DataForms.DataFormFieldValue With {.ID = "USERNAME", .Value = BBISUserName})

		'The DataFormItem is used to hold the set of form fields.
		dfi.Values = fvSet

		'DataFormItem is passed to the request
		Req.Parameters = dfi

		'Max rows acts as a governor to limit the amount of rows retrieved by the datalist
		Req.MaxRows = 500
		Req.IncludeMetaData = True

		Try
			Reply = _appFx.DataListLoad(Req)

			DisplayDataListReplyRowsInListView(Reply, lvResults)


		Catch exSoap As System.Web.Services.Protocols.SoapException
			'If an error occurs we attach a SOAP fault error header.
			'You can check the proxy.ResponseErrorHeaderValue to get rich
			'error information including a nicer message copared to the raw exception 			message.
			Dim wsMsg As String
			If _appFx.ResponseErrorHeaderValue IsNot Nothing Then
				wsMsg = _appFx.ResponseErrorHeaderValue.ErrorText
			Else
				wsMsg = exSoap.Message
			End If
			MsgBox(wsMsg)

		Catch ex As Exception
			MsgBox(ex.ToString)

		Finally
			'Hide hourglass after api call
			Cursor.Current = Cursors.Default
			Cursor.Show()
		End Try
	End Sub

	Private Sub DisplayDataListReplyRowsInListView(ByVal Reply As Blackbaud.AppFx.WebAPI.ServiceProxy.DataListLoadReply, ByVal ListView As System.Windows.Forms.ListView)
		Try
			'Display hourglass during appfx web service calls
			Cursor.Current = Cursors.WaitCursor
			Cursor.Show()

			With ListView
				.View = View.Details
				.FullRowSelect = True
				.Clear()
			End With

			If (Reply.Rows IsNot Nothing) Then
				For Each f As Blackbaud.AppFx.XmlTypes.DataListOutputFieldType In Reply.MetaData.OutputDefinition.OutputFields
					If f.IsHidden = True Then
						ListView.Columns.Add(f.FieldID, f.Caption, 0)
					Else
						ListView.Columns.Add(f.FieldID, f.Caption)
					End If
				Next

				For Each row As Blackbaud.AppFx.WebAPI.ServiceProxy.DataListResultRow In Reply.Rows
					ListView.Items.Add(New ListViewItem(row.Values))
				Next

				ListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
				For Each f As Blackbaud.AppFx.XmlTypes.DataListOutputFieldType In Reply.MetaData.OutputDefinition.OutputFields
					If f.IsHidden = True Then
						ListView.Columns(f.FieldID).Width = 0
					End If
				Next
			End If

		Catch ex As Exception
			MsgBox(ex.Message.ToString)

		Finally
			'Hide hourglass after api call
			Cursor.Current = Cursors.Default
			Cursor.Show()
		End Try
	End Sub

	Private Sub DisplayComparisonResultsInListView(ByVal ExceptionList As ChildPhotoCollection, ByRef ListView As System.Windows.Forms.ListView)
		Try
			'Display hourglass during appfx web service calls
			Cursor.Current = Cursors.WaitCursor
			Cursor.Show()

			With ListView
				.View = View.Details
				.FullRowSelect = True
				.Clear()
			End With

			If (ExceptionList.ChildPhotoList.Count > 0) Then
				'For Each f As ChildPhotoData In ExceptionList.ChildPhotoList
				'	ListView.Columns.Add(f.ChildLookupId., f.Caption)					
				'Next

				With ListView
					.View = View.Details
					'.CheckBoxes = True
					.GridLines = True
					.FullRowSelect = True
					.HideSelection = False
					.MultiSelect = False
					.Columns.Add("Child Id")
					.Columns.Add("Project Id")
					.Columns.Add("Child Name")
					'.Columns.Add("Last Name")
				End With

				For Each f As ChildPhotoData In ExceptionList.ChildPhotoList
					ListView.Items.Add(New ListViewItem(New String() {f.ChildLookupId, f.ChildProject, f.ChildName}))
					'ListView.Items.Add(New ListViewItem(f.ChildProject))
					'ListView.Items.Add(New ListViewItem(f.FirstName))
					'ListView.Items.Add(New ListViewItem(f.LastName))
					System.Diagnostics.Debug.WriteLine(f.ChildLookupId & ":" & f.ChildProject & ":" & f.ChildName)
				Next

				ListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
				'For Each f As Blackbaud.AppFx.XmlTypes.DataListOutputFieldType In Reply.MetaData.OutputDefinition.OutputFields
				'	If f.IsHidden = True Then
				'		ListView.Columns(f.FieldID).Width = 0
				'	End If
				'Next
			End If

		Catch ex As Exception
			MsgBox(ex.Message.ToString)

		Finally
			'Hide hourglass after api call
			Cursor.Current = Cursors.Default
			Cursor.Show()
		End Try
	End Sub


	Private Sub WriteCollectionXml(ByVal xmlWriter As XmlWriter, ByVal fieldName As String, ByVal itemArray As ChildPhotoCollection, ByVal includeDate As Boolean)
		xmlWriter.WriteProcessingInstruction("xml", "version=""1.0""")
		xmlWriter.WriteStartElement(fieldName.ToUpper())
		Dim items As ChildPhotoData = Nothing
		'If itemArray IsNot Nothing Then
		'	items = itemArray.ChildPhotoList.ToArray() 'itemArray.Items
		'End If
		'If items IsNot Nothing Then

		For Each item As ChildPhotoData In itemArray.ChildPhotoList.ToArray()
			xmlWriter.WriteStartElement("ITEM")

			'For Each fieldValue As DataFormFieldValue In item.Values
			'	Dim objectValue As Object = RuntimeHelpers.GetObjectValue(fieldValue.Value)
			'	Dim str As String = String.Empty
			'	If TypeOf objectValue Is Decimal Then
			'		str = XmlConvert.ToString(CDec(objectValue))
			'	ElseIf TypeOf objectValue Is Double Then
			'		str = XmlConvert.ToString(CDbl(objectValue))
			'	ElseIf TypeOf objectValue Is Single Then
			'		str = XmlConvert.ToString(CSng(objectValue))
			'	Else
			'		str = objectValue.ToString()
			'	End If
			xmlWriter.WriteElementString("CHILDLOOKUPID", item.ChildLookupId.ToString())
			If includeDate = True Then
				xmlWriter.WriteElementString("ACTUALDATE", DateTimePicker1.Value.ToShortDateString())
			End If
			xmlWriter.WriteElementString("COMPLETEINTERACTIONPROCESSID", _photoHelperSessionId.ToString())
			xmlWriter.WriteElementString("COMMENT", _interactionComment)
			'Next

			xmlWriter.WriteEndElement()
		Next
		'End If
		xmlWriter.WriteEndElement()

	End Sub

	Private Sub CompareDataListValues(ByVal Reply As ServiceProxy.DataListLoadReply, ByRef photoFolderChildrenList As ChildPhotoCollection)
		'This will compare the list of data from API call, to the list obtained from the photo files
		Dim childId As String = String.Empty
		Dim childProject As String = String.Empty
		Dim childName As String = String.Empty
		Dim isChildValid As Boolean = True
		Dim crmFirstName As String = String.Empty
		Dim crmLastName As String = String.Empty

		'these are used for soundex-type Name comparisons
		'Dim crmMphone As New DoubleMetaphone()
		'Dim fileMphone As New DoubleMetaphone()
		'Dim crmSoundex As New Soundex.Soundex()
		'Dim fileSoundex As New Soundex.Soundex()
		'Dim crmNameSoundex As String
		'Dim fileNameSoundex As String

		'5/30/14 new for the 2 name check algorithms:
		Dim isJWFirstNameValid As Boolean = True
		Dim isJWLastNameValid As Boolean = True
		Dim isDLFirstNameValid As Boolean = True
		Dim isDLLastNameValid As Boolean = True
		Dim isJWChildNameValid As Boolean = True
		Dim isJWPhotoNameValid As Boolean = True
		Dim isDLChildNameValid As Boolean = True
		Dim isDLPhotoNameValid As Boolean = True

		Dim isValid As Boolean = False
		Dim canCompareFirstLastName As Boolean = False
		Dim mustCheckChildName As Boolean = False

		Dim jwProximityValue As Double
		Dim dlDistanceValue As Integer

		_nameNotMatchList = New ChildPhotoCollection()
		_notInCrmList = New ChildPhotoCollection()
		_projectIdNotMatchList = New ChildPhotoCollection()
		_childValidatedList = New ChildPhotoCollection()

		photoFolderChildrenList.SortList()

		If Reply.Rows.Count > 0 Then
			'convert the datalist reply collection to a childphotocollection:
			Dim crmChildList As ChildPhotoCollection = New ChildPhotoCollection()
			crmChildList.ChildPhotoList.AddRange(CreatePhotoCollectionFromReply(Reply))

			Dim crmChild As New ChildPhotoData
			Dim photoChild As New ChildPhotoData

			'iterate through the list of kids from photo folder, checking one-by-one against the kids returned from CRM
			Dim index As Integer = 0

			'search the crm list for this child
			'if found, set the properties to variables
			'compare variables to the photo folder child we're working with
			For index = 0 To photoFolderChildrenList.ChildPhotoList.Count - 1
				'child is valid until proven otherwise
				isChildValid = True
				isValid = False
				isJWChildNameValid = False
				isDLChildNameValid = True
				canCompareFirstLastName = False
				mustCheckChildName = True
				photoChild = photoFolderChildrenList.ChildPhotoList(index)
				crmChild = crmChildList.ChildPhotoList.Where(Function(x) x.ChildLookupId.ToLower().Equals(photoChild.ChildLookupId.ToLower())).FirstOrDefault()
				'5/30/14 Memphis: we are now populating the first and last name from the CRM data list call:
				'crmFirstName = String.Empty
				'crmLastName = String.Empty

				If Not crmChild Is Nothing Then
					'if user checked the override box, then skip the Name check
					If nameValidationOverrideCheckBox.Checked = False Then
						'5/30/14 Memphis: clean out the extra characters so they don't throw off the comparisons:
						CleanoutExtraChars(photoChild.ChildName)
						CleanoutExtraChars(crmChild.ChildName)
						Dim dlDistance As DLDistance = New DLDistance()

						'Memphis 7/7: set the flags for first/lastname checking
						canCompareFirstLastName = ((photoChild.HasMiddleInitial = True) OrElse (String.IsNullOrEmpty(photoChild.FileFirstName) = False) OrElse (String.IsNullOrEmpty(photoChild.FileLastName)))

						'first check if the names match at all at a string level:
						If Not photoChild.ChildName.ToLower().Equals(crmChild.ChildName.ToLower()) Then
							'Memphis 7/8: it seems that the JWProximity may show a match when it doesn't match, so 
							'first check the individual names, if possible:
							'check the first and last names if the whole name didn't match:
							'Memphis 7/7: if we have a middle initial in the filename, or we have a first and last name,
							' then we can compare first and last names:
							If canCompareFirstLastName = True Then
								'If isJWChildNameValid = False AndAlso isDLChildNameValid = False Then
								'compare first name with JWProximity:
								CleanoutExtraChars(crmChild.CRMFirstName)
								CleanoutExtraChars(photoChild.FileFirstName)
								jwProximityValue = JaroWinklerDistance.proximity(crmChild.CRMFirstName, photoChild.FileFirstName)
								If jwProximityValue >= _jwProximityThreshold Then
									isJWFirstNameValid = True
								Else
									isJWFirstNameValid = False
								End If								

								'compare last name with JWProximity:
								CleanoutExtraChars(crmChild.CRMLastName)
								CleanoutExtraChars(photoChild.FileLastName)
								jwProximityValue = JaroWinklerDistance.proximity(crmChild.CRMLastName, photoChild.FileLastName)
								If jwProximityValue >= _jwProximityThreshold Then
									isJWLastNameValid = True
								Else
									isJWLastNameValid = False
								End If

								'Me.txtDLDistance.Text = dlDistance.DamerauLevenshteinDistance(source, target)
								'firstname check with DLDistance:
								dlDistanceValue = dlDistance.DamerauLevenshteinDistance(crmChild.CRMFirstName, photoChild.FileFirstName)
								If dlDistanceValue <= _dlDistanceThreshold Then
									isDLFirstNameValid = True
								Else
									isDLFirstNameValid = False
								End If

								'lastname compare with DLDistance:
								dlDistanceValue = dlDistance.DamerauLevenshteinDistance(crmChild.CRMLastName, photoChild.FileLastName)
								If dlDistanceValue <= _dlDistanceThreshold Then
									isDLLastNameValid = True
								Else
									isDLLastNameValid = False
								End If

								'Memphis 7/8: set the flag based on results of above
								If isJWFirstNameValid = True AndAlso isJWLastNameValid = True Then
									'do not need to check the child name if these two passed:
									mustCheckChildName = False
									isChildValid = True
									isValid = True
								Else
									'if not valid yet, check the DL Distance and consider those values:
									If isDLFirstNameValid = True AndAlso isDLLastNameValid = True Then
										'good enough, call it a match:
										mustCheckChildName = False
										isChildValid = True
										isValid = True
									Else
										mustCheckChildName = False
										isChildValid = False
										isValid = False
									End If
								End If

								'the following was copied to further down in the code:
								''calculate the final decision:
								'If isJWFirstNameValid AndAlso isJWLastNameValid Then
								'	'do not need to consider the DL Distance if these are valid:
								'	isValid = True
								'End If

								''if not valid yet, check the DL Distance and consider those values:
								'If isValid = False Then
								'	If isDLFirstNameValid AndAlso isDLLastNameValid Then
								'		'good enough, call it a match:
								'		isChildValid = True
								'	End If
								'End If
								'Else
								'	'calculate the validity of the child:
								'	isValid = ((isDLChildNameValid = True) AndAlso (isJWChildNameValid = True))
								'End If
								'Else
								'	'calculate the validity of the child, since we can't compare using first/last names:
								'	isValid = ((isDLChildNameValid = True) AndAlso (isJWChildNameValid = True))
							End If


							' CHECK CHILD NAME IF ABOVE WASN'T PERFORMED!
							'Memphis 7/8: refactored this and the above, so now check the whole child name
							'if we couldn't check the first/last names:
							If ((canCompareFirstLastName = False OrElse mustCheckChildName = True) AndAlso (isChildValid = False)) Then
								' 5/30/14 Memphis:  now using the Jaro Winkler Proximity and the DL Distance algorithms
								'   to check names for similarity:
								'5/30/14 Memphis: now using the new JWDistance and DLDistance classes for name comparisons:
								'compare childname value using JWDistance:
								jwProximityValue = JaroWinklerDistance.proximity(crmChild.ChildName, photoChild.ChildName)
								If jwProximityValue >= _jwProximityThreshold Then
									isJWChildNameValid = True
								Else
									isJWChildNameValid = False
								End If

								'if JW didn't validate, try the DLDistance:
								If isJWChildNameValid = False Then
									dlDistanceValue = dlDistance.DamerauLevenshteinDistance(crmChild.ChildName, photoChild.ChildName)
									If dlDistanceValue <= _dlDistanceThreshold Then
										isDLChildNameValid = True
									Else
										isDLChildNameValid = False
									End If
								End If

								If isJWChildNameValid = True AndAlso isDLChildNameValid = True Then
									isValid = True
									isChildValid = True
								Else
									isValid = False
									isChildValid = False
								End If
							End If

							''calculate the final decision:
							'If isJWFirstNameValid AndAlso isJWLastNameValid Then
							'	'do not need to consider the DL Distance if these are valid:
							'	isValid = True
							'End If

							''if not valid yet, check the DL Distance and consider those values:
							'If isValid = False Then
							'	If isDLFirstNameValid AndAlso isDLLastNameValid Then
							'		'good enough, call it a match:
							'		isChildValid = True
							'	End If
							'End If

							'at this point, if not valid, probably never going to be valid:
							If isValid = False Then
								isChildValid = False
							End If
							' end of new 5/30/14


							'6/3/14: Memphis  no longer using these algorithms:
							''names string values don't match, so do the metaphone check in case they're very close
							''use the DoubleMetaphone class to generate the keys & then compare the first and last names respectively
							''check the 1st names first:
							'crmMphone.computeKeys(crmFirstName.ToLower())
							'fileMphone.computeKeys(photoChild.FileFirstName.ToLower())
							'If (Not String.IsNullOrEmpty(crmMphone.PrimaryKey) AndAlso Not String.IsNullOrEmpty(fileMphone.PrimaryKey)) AndAlso Not crmMphone.PrimaryKey.Equals(fileMphone.PrimaryKey) Then
							'	'don't match at all:
							'	photoChild.CRMChildName = crmChild.CRMChildName
							'	_nameNotMatchList.ChildPhotoList.Add(photoChild)
							'	isChildValid = False
							'End If

							'If isChildValid = True Then
							'	'check the 2nd names next:
							'	crmMphone.computeKeys(crmLastName.ToLower())
							'	fileMphone.computeKeys(photoChild.FileLastName.ToLower())
							'	If (Not String.IsNullOrEmpty(crmMphone.PrimaryKey) AndAlso Not String.IsNullOrEmpty(fileMphone.PrimaryKey)) AndAlso Not crmMphone.PrimaryKey.Equals(fileMphone.PrimaryKey) Then
							'		'don't match at all:
							'		photoChild.CRMChildName = crmChild.CRMChildName
							'		_nameNotMatchList.ChildPhotoList.Add(photoChild)
							'		isChildValid = False

							'		'last resort, check the compare:
							'		'If Not String.Compare(crmChild.ChildName.ToLower(), photoChild.ChildName.ToLower()) = 0 Then
							'		'	photoChild.CRMChildName = crmChild.CRMChildName
							'		'	_nameNotMatchList.ChildPhotoList.Add(photoChild)
							'		'	isChildValid = False
							'		'End If

							'		'If (Not String.IsNullOrEmpty(crmMphone.AlternateKey) AndAlso Not String.IsNullOrEmpty(fileMphone.AlternateKey)) Then
							'		'	If Not crmMphone.AlternateKey.Equals(fileMphone.AlternateKey) Then
							'		'		'just doesn't match!
							'		'		'set the crm childname so it will be output
							'		'		photoChild.CRMChildName = crmChild.CRMChildName
							'		'		_nameNotMatchList.ChildPhotoList.Add(photoChild)
							'		'		isChildValid = False
							'		'	End If
							'		'Else
							'		'	'just doesn't match!
							'		'	'set the crm childname so it will be output
							'		'	photoChild.CRMChildName = crmChild.CRMChildName
							'		'	_nameNotMatchList.ChildPhotoList.Add(photoChild)
							'		'	isChildValid = False
							'		'End If
							'	End If
							'End If

						End If
					End If

					'check further if check above passed:
					If isChildValid = True Then
						'check project id
						If Not photoChild.ChildProject.ToLower().Equals(crmChild.ChildProject.ToLower()) Then
							'set the crm childname so it will be output
							photoChild.CRMChildName = crmChild.CRMChildName
							'set the crm project so it will be output
							photoChild.ChildProject = crmChild.ChildProject
							_projectIdNotMatchList.ChildPhotoList.Add(photoChild)
							isChildValid = False
						End If
					Else
						photoChild.CRMChildName = crmChild.CRMChildName
						_nameNotMatchList.ChildPhotoList.Add(photoChild)
					End If

				Else
					_notInCrmList.ChildPhotoList.Add(photoChild)
					isChildValid = False
				End If
				If isChildValid = True Then
					'store this child for the call to the record op
					_childValidatedList.ChildPhotoList.Add(photoChild)
				End If
			Next
			'End If

			'DisplayComparisonResultsInListView(_childValidatedList, lvResults)

		End If

	End Sub

	Public Function IsFileNameValid(ByVal strFileName As String) As Boolean

		' Determines if the name is blank.
		If strFileName = "" Then
			Return False
		End If

		' Determines if there are bad characters in the name.
		For Each badChar As Char In System.IO.Path.GetInvalidPathChars
			If InStr(strFileName, badChar) > 0 Then
				Return False
			End If
		Next

		' The name passes basic validation.
		Return True
	End Function

	Private Sub Button_GetSourceFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_GetSourceFolder.Click
		If FolderBrowser_GetSourceFolder.ShowDialog = Windows.Forms.DialogResult.OK Then
			TextBox_SourceFolder.Text = FolderBrowser_GetSourceFolder.SelectedPath
		End If
	End Sub

	Private Sub validatePhotosButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles validatePhotosButton.Click
		Dim dirinfo As DirectoryInfo
		Dim allFiles() As FileInfo
		'Dim txtFile As String

		'get the files from the folder and parse their filenames, turning them into ChildPhotoData objects
		Dim childPhotoList As New ChildPhotoCollection()
		Dim childData As New ChildPhotoData()
		'Dim fileNameItems As String()
		Dim dataCounter As Integer = 0

		validatePhotosButton.Enabled = False

		Dim isValidPhotos As Boolean = True

		Try
			'set the comment value based on which source radio button is checked:
			Dim photoSource As String = String.Empty

			'check that the completion date value is NOT in the future!
			If DateTimePicker1.Value > DateTime.Now() Then
				Throw New Exception("The photo completion date cannot be in the future! Please double-check the value.")
			End If

			'check if Other Radio button is checked and Other text is completed:
			If otherRadioButton.Checked = True Then
				If String.IsNullOrEmpty(otherSourceText.Text) Then
					Throw New ArgumentNullException("Other", "Other Source value is required if the picture source is Other.")
				Else
					photoSource = otherSourceText.Text
				End If
			End If

			If ftpRadioButton.Checked = True Then
				photoSource = "FTP"
			End If

			If emailRadioButton.Checked = True Then
				photoSource = "Email"
			End If

			_interactionComment = String.Format("Received {0} via {1} -- {2}", Me.DateTimePicker1.Value.ToShortDateString(), photoSource, Environment.UserName)

			If Not String.IsNullOrEmpty(TextBox_SourceFolder.Text) Then
				If IO.Directory.Exists(TextBox_SourceFolder.Text) Then
					dirinfo = New DirectoryInfo(TextBox_SourceFolder.Text)
					allFiles = dirinfo.GetFiles("*.jpg")

					' The datalist returns one column for name of child, CHILDNAME which is the NAME field in CRM.
					' Filenames are generally formatted: projectid childlookupid name_Annual Photo 2014.jpg 
					'
					Dim filePhotoList As List(Of ChildPhotoData) = GetPhotoFilesAsList(allFiles)

					childPhotoList.ChildPhotoList.AddRange(filePhotoList)

					'DisplayComparisonResultsInListView(childPhotoList, lvResults)

					LoadChildrenAndCompare(childPhotoList)

					'display labels that will list a count, if any, of exceptions and where to find them:
					If _notInCrmList.ChildPhotoList.Count > 0 Then
						outputNotInCRMLabel.Text = String.Format("{0} children were not found in CRM. Look here: {1}", _notInCrmList.ChildPhotoList.Count, Me.TextBox_SourceFolder.Text & "\" & _notincrmfoldername)
						outputNotInCRMLabel.Visible = True
					End If

					If _nameNotMatchList.ChildPhotoList.Count > 0 Then
						namesNotMatchOutputLabel.Text = String.Format("{0} names do not match CRM. Look here: {1}", _nameNotMatchList.ChildPhotoList.Count, Me.TextBox_SourceFolder.Text & "\" & _unmatchednamefoldername)
						namesNotMatchOutputLabel.Visible = True
					End If

					If _projectIdNotMatchList.ChildPhotoList.Count > 0 Then
						projectNotMatchOutputLabel.Text = String.Format("{0} projects do not match CRM. Look here: {1}", _projectIdNotMatchList.ChildPhotoList.Count, Me.TextBox_SourceFolder.Text & "\" & _unmatchedprojectfoldername)
						projectNotMatchOutputLabel.Visible = True
					End If

					'Memphis added 4/14/14
					If _fileNameParseErrorList.ChildPhotoList.Count > 0 Then
						lblFilenameParseErrors.Text = String.Format("{0} filename parse errors. Look here: {1}", _fileNameParseErrorList.ChildPhotoList.Count, Me.TextBox_SourceFolder.Text & "\" & _fileNameParseErrorFolderName)
						lblFilenameParseErrors.Visible = True
					End If

					If _childValidatedList.ChildPhotoList.Count > 0 Then
						CompleteInteractions()
						'Memphis added 4/14/14
						If _alreadyCompletedList.ChildPhotoList.Count > 0 Then
							lblAlreadyCompletedErrors.Text = String.Format("{0} interactions already completed. Look here: {1}", _alreadyCompletedList.ChildPhotoList.Count, Me.TextBox_SourceFolder.Text & "\" & _alreadycompletedfoldername)
							lblAlreadyCompletedErrors.Visible = True
						End If
						'Memphis added 4/14/14: update the label to display count to user
						lblEventsTitle.Text = String.Format("{0} Children Validated:", _childValidatedList.ChildPhotoList.Count)

						'Memphis 4/14/14: move the completed files to their own folder
						If _childValidatedList.ChildPhotoList.Count > 0 Then
							If DoesOutputFolderExist(Me.TextBox_SourceFolder.Text & "\" & _completedFolderName) = False Then
								CreateOutputFolder(_completedFolderName)
							End If
							WriteCollectionToOutputFolder(_childValidatedList, Me.TextBox_SourceFolder.Text & "\" & _completedFolderName, "ValidatedChildren.txt")
							MoveExceptionPhotosToOutputFolder(Me.TextBox_SourceFolder.Text & "\" & _completedFolderName, _childValidatedList)
						End If
					Else
						MsgBox("There are no valid child photos. No Interactions will be completed.")
						isValidPhotos = False
					End If

					CreateOutputOfComparison()

					If isValidPhotos = True Then
						allFiles = Nothing
						dirinfo = Nothing
						MsgBox("Photos validated and Interactions should have been completed for the valid children.")
					End If

				Else
					MsgBox("The folder entered (" & TextBox_SourceFolder.Text & ") does not exist.", MsgBoxStyle.Exclamation, "Folder Does Not Exist")
				End If
			Else
				MsgBox("The source folder cannot be blank.", MsgBoxStyle.Exclamation, "Need Source Folder!")
			End If

		Catch ex As Exception
			MsgBox("An error occured: " & ex.Message & vbCrLf & "Source: " & ex.Source)

		Finally
			resetButton.Enabled = True
		End Try

	End Sub

	Private Function CreatePhotoCollectionFromReply(ByVal Reply As ServiceProxy.DataListLoadReply) As List(Of ChildPhotoData)
		Dim returnList As New List(Of ChildPhotoData)
		' 5/30/14 Memphis: updated the DataList Spec to return first & last names:
		'rtrim(replace(sc.[NAME], ' ','')) as CHILDNAME,
		'so.LOOKUPID as CHILDID, 
		'sl.LOOKUPID as LOCATIONID,
		'sc.[NAME] as CRMNAME,
		'sc.[FIRSTNAME] as FIRSTNAME,
		'sc.[LASTNAME] as LASTNAME

		Dim childNameColumn As Integer = 0
		Dim lookupIdColumn As Integer = 1
		Dim projectIdColumn As Integer = 2
		Dim crmNameColumn As Integer = 3
		Dim firstNameColumn As Integer = 4
		Dim lastNameColumn As Integer = 5

		For Each row As Blackbaud.AppFx.WebAPI.ServiceProxy.DataListResultRow In Reply.Rows
			returnList.Add(New ChildPhotoData(row.Values(lookupIdColumn).ToString(), row.Values(projectIdColumn).ToString(), _
					  row.Values(childNameColumn).ToString().Replace(" ", ""), "", row.Values(crmNameColumn).ToString(), _
					  row.Values(firstNameColumn).ToString(), row.Values(lastNameColumn).ToString()))
		Next

		Return returnList
	End Function

	Private Sub LoadChildrenAndCompare(ByVal childList As ChildPhotoCollection)
		'Display hourglass during appfx web service calls
		Cursor.Current = Cursors.WaitCursor
		Cursor.Show()

		Dim Req As New Blackbaud.AppFx.WebAPI.ServiceProxy.DataListLoadRequest

		Dim fvSet As New Blackbaud.AppFx.XmlTypes.DataForms.DataFormFieldValueSet

		Dim dfi As New Blackbaud.AppFx.XmlTypes.DataForms.DataFormItem
		Dim Reply As New Blackbaud.AppFx.WebAPI.ServiceProxy.DataListLoadReply
		Req.ClientAppInfo = _clientAppInfoHeader

		Dim xmlString As String = GetPhotoCollectionAsXML(childList, "CHILDPHOTOCOLLECTION", False)	' output.ToString()

		'this gets the list of children by XML list of lookupid values:
		Dim xmlChildList As String = xmlString ' "<?xml version='1.0'?><CHILDPHOTOCOLLECTION><ITEM><CHILDLOOKUPID>C309254</CHILDLOOKUPID></ITEM><ITEM><CHILDLOOKUPID>C233146</CHILDLOOKUPID></ITEM><ITEM><CHILDLOOKUPID>C233100</CHILDLOOKUPID></ITEM></CHILDPHOTOCOLLECTION>"
		Req.DataListID = _childDataListByXmlId	'  "97c4f605-2bbb-4624-acbd-38f67441e630")    ' Child Search data list
		Req.ContextRecordID = xmlChildList	'child Lookup Id  "00000000-0000-0000-0000-000000000000"

		'fvSet.Add(New Blackbaud.AppFx.XmlTypes.DataForms.DataFormFieldValue With {.ID = "USERNAME", .Value = "JayciLane"})

		'The DataFormItem is used to hold the set of form fields.
		dfi.Values = fvSet

		'DataFormItem is passed to the request
		Req.Parameters = dfi

		'Max rows acts as a governor to limit the amount of rows retrieved by the datalist
		Req.MaxRows = 500
		Req.IncludeMetaData = True

		Try
			Reply = _appFx.DataListLoad(Req)

			CompareDataListValues(Reply, childList)

		Catch exSoap As System.Web.Services.Protocols.SoapException
			'If an error occurs we attach a SOAP fault error header.
			'You can check the proxy.ResponseErrorHeaderValue to get rich
			'error information including a nicer message copared to the raw exception 			message.
			Dim wsMsg As String
			If _appFx.ResponseErrorHeaderValue IsNot Nothing Then
				wsMsg = _appFx.ResponseErrorHeaderValue.ErrorText
			Else
				wsMsg = exSoap.Message
			End If
			MsgBox(wsMsg)

		Catch ex As Exception
			MsgBox(ex.ToString)
		Finally
			'Hide hourglass after api call
			Cursor.Current = Cursors.Default
			Cursor.Show()
		End Try
	End Sub

	Private Sub CreateOutputOfComparison()
		'not in crm
		If _notInCrmList.ChildPhotoList.Count > 0 Then
			If DoesOutputFolderExist(Me.TextBox_SourceFolder.Text & "\" & _notincrmfoldername) = False Then
				CreateOutputFolder(_notincrmfoldername)
			End If
			WriteCollectionToOutputFolder(_notInCrmList, Me.TextBox_SourceFolder.Text & "\" & _notincrmfoldername, "ChildrenNotInCrm.txt")
			MoveExceptionPhotosToOutputFolder(Me.TextBox_SourceFolder.Text & "\" & _notincrmfoldername, _notInCrmList)
		End If

		'names do not match
		If _nameNotMatchList.ChildPhotoList.Count > 0 Then
			If DoesOutputFolderExist(Me.TextBox_SourceFolder.Text & "\" & _unmatchednamefoldername) = False Then
				CreateOutputFolder(_unmatchednamefoldername)
			End If
			WriteCollectionToOutputFolder(_nameNotMatchList, Me.TextBox_SourceFolder.Text & "\" & _unmatchednamefoldername, "NamesDoNotMatch.txt")
			MoveExceptionPhotosToOutputFolder(Me.TextBox_SourceFolder.Text & "\" & _unmatchednamefoldername, _nameNotMatchList)
		End If

		'project id doesn't match
		If _projectIdNotMatchList.ChildPhotoList.Count > 0 Then
			If DoesOutputFolderExist(Me.TextBox_SourceFolder.Text & "\" & _unmatchedprojectfoldername) = False Then
				CreateOutputFolder(_unmatchedprojectfoldername)
			End If
			WriteCollectionToOutputFolder(_projectIdNotMatchList, Me.TextBox_SourceFolder.Text & "\" & _unmatchedprojectfoldername, "ProjectsDoNotMatch.txt")
			MoveExceptionPhotosToOutputFolder(Me.TextBox_SourceFolder.Text & "\" & _unmatchedprojectfoldername, _projectIdNotMatchList)
		End If

		'These come from the Complete Interaction record op:
		'interaction not found
		If _interactionNotFoundList.ChildPhotoList.Count > 0 Then
			If DoesOutputFolderExist(Me.TextBox_SourceFolder.Text & "\" & _interactionnotfoundfoldername) = False Then
				CreateOutputFolder(_interactionnotfoundfoldername)
			End If
			WriteCollectionToOutputFolder(_interactionNotFoundList, Me.TextBox_SourceFolder.Text & "\" & _interactionnotfoundfoldername, "InteractionNotFoundInCRM.txt")
			MoveExceptionPhotosToOutputFolder(Me.TextBox_SourceFolder.Text & "\" & _interactionnotfoundfoldername, _interactionNotFoundList)
		End If

		'annual photo type interaction already completed
		If _alreadyCompletedList.ChildPhotoList.Count > 0 Then
			If DoesOutputFolderExist(Me.TextBox_SourceFolder.Text & "\" & _alreadycompletedfoldername) = False Then
				CreateOutputFolder(_alreadycompletedfoldername)
			End If
			WriteCollectionToOutputFolder(_alreadyCompletedList, Me.TextBox_SourceFolder.Text & "\" & _alreadycompletedfoldername, "InteractionAlreadyCompleted.txt")
			MoveExceptionPhotosToOutputFolder(Me.TextBox_SourceFolder.Text & "\" & _alreadycompletedfoldername, _alreadyCompletedList)
		End If

		'unusable photo type interaction already completed
		If _unusableAlreadyCompletedList.ChildPhotoList.Count > 0 Then
			If DoesOutputFolderExist(Me.TextBox_SourceFolder.Text & "\" & _unusablealreadycompletedfoldername) = False Then
				CreateOutputFolder(_unusablealreadycompletedfoldername)
			End If
			WriteCollectionToOutputFolder(_interactionNotFoundList, Me.TextBox_SourceFolder.Text & "\" & _unusablealreadycompletedfoldername, "UnusableInteractionAlreadyCompleted.txt")
			MoveExceptionPhotosToOutputFolder(Me.TextBox_SourceFolder.Text & "\" & _unusablealreadycompletedfoldername, _unusableAlreadyCompletedList)
		End If

		'filenames that couldn't be parsed correctly:
		If _fileNameParseErrorList.ChildPhotoList.Count > 0 Then
			If DoesOutputFolderExist(Me.TextBox_SourceFolder.Text & "\" & _fileNameParseErrorFolderName) = False Then
				CreateOutputFolder(_fileNameParseErrorFolderName)
			End If
			WriteCollectionToOutputFolder(_fileNameParseErrorList, Me.TextBox_SourceFolder.Text & "\" & _fileNameParseErrorFolderName, "UnableToParseFilename.txt")
			MoveExceptionPhotosToOutputFolder(Me.TextBox_SourceFolder.Text & "\" & _fileNameParseErrorFolderName, _fileNameParseErrorList)
		End If


		If _debugging = True Then
			'write out the photo files into a file
			WriteCollectionToOutputFolder(_childValidatedList, ".", "PhotoTesting.txt")
		End If

	End Sub

	Private Function DoesOutputFolderExist(ByVal folderName As String) As Boolean
		Return IO.Directory.Exists(folderName)
	End Function

	Private Sub WriteCollectionToOutputFolder(ByVal childList As ChildPhotoCollection, ByVal folderName As String, ByVal fileName As String)
		Using outfile As New StreamWriter(folderName & fileName, False)
			For Each child As ChildPhotoData In childList.ChildPhotoList
				'outfile.WriteLine(child.ChildLookupId & ":" & child.ChildName & ":" & child.ChildProject & ":" & child.PhotoFile)
				' for the mismatched names, output the CRM Name data as well, filename then CRM Name, like this:  
				' IN-131 C117947 Joel John Philip 2014.jpg          Joel J. Philip          
				' IN-131 C315156 Troy Christy Moses 2014.JPG          Troy C. Moses        

				If fileName.ToLower().Equals("namesdonotmatch.txt") Then
					outfile.WriteLine(String.Format("{0}          {1}", child.PhotoFile, child.CRMChildName))
				ElseIf fileName.ToLower().Equals("interactionalreadycompleted.txt") Then
					'output the completion date that came back from CRM
					outfile.WriteLine(String.Format("{0}          {1}", child.PhotoFile, child.CompletionDate))
				ElseIf fileName.ToLower().Equals("projectsdonotmatch.txt") Then
					outfile.WriteLine(String.Format("{0}          {1}          {2}          {3}", child.PhotoFile, child.ChildProject, child.ChildLookupId, child.CRMChildName))
				Else
					outfile.WriteLine(child.PhotoFile)
				End If

			Next
			outfile.Close()
		End Using
	End Sub

	Private Sub CreateOutputFolder(ByVal folderName As String)
		IO.Directory.CreateDirectory(Me.TextBox_SourceFolder.Text & "\" & folderName)
	End Sub

	Private Sub MoveExceptionPhotosToOutputFolder(ByVal outputFolder As String, ByVal photoList As ChildPhotoCollection)
		'this will move the exception photos to the respective output folder
		For Each child As ChildPhotoData In photoList.ChildPhotoList
			'we assume the folder exists already
			Dim fileName As String = child.PhotoFile
			My.Computer.FileSystem.MoveFile(Me.TextBox_SourceFolder.Text & "\" & fileName, outputFolder & fileName)
		Next
	End Sub

	Private Sub CompleteInteractions()
		'MsgBox("This will call the record op to complete interactions!")

		'Display hourglass during appfx web service calls
		Cursor.Current = Cursors.WaitCursor
		Cursor.Show()

		Dim Req As New Blackbaud.AppFx.WebAPI.ServiceProxy.RecordOperationPerformRequest
		Dim Reply As Blackbaud.AppFx.WebAPI.ServiceProxy.RecordOperationPerformReply
		Dim fvSet As New Blackbaud.AppFx.XmlTypes.DataForms.DataFormFieldValueSet
		Dim dfi As New Blackbaud.AppFx.XmlTypes.DataForms.DataFormItem

		Req.ClientAppInfo = _clientAppInfoHeader
		'Req.ID = _completeInteractionsRecordOpId
		Req.RecordOperationName = _recordopname	'"Event Expense: Delete"

		'turn the validated child photo list to XML:
		Dim xmlString As String = GetPhotoCollectionAsXML(_childValidatedList, "PHOTOINTERACTION", True)

		'debugging only, to gather the XML to validate it:
		If _debugging = True Then
			WriteXMLToFile(xmlString, Me.TextBox_SourceFolder.Text & "\", "XMLstring.xml")
		End If

		'these are used for the parameters, which is the XML and ChangeAgentID which will be NULL value:
		fvSet.Add(New Blackbaud.AppFx.XmlTypes.DataForms.DataFormFieldValue With {.ID = "ID", .Value = xmlString})
		fvSet.Add(New Blackbaud.AppFx.XmlTypes.DataForms.DataFormFieldValue With {.ID = "CHANGEAGENTID", .Value = Nothing})

		'The DataFormItem is used to hold the set of form fields.
		dfi.Values = fvSet

		'DataFormItem is passed to the request
		Req.Parameters = dfi

		'Record OP ID is the context or ID parameter passed into record op.  It's required in the SPEC, we're sending the XML collection:
		Req.ID = xmlString
		Req.RecordOperationID = _completeInteractionsRecordOpId

		Try
			Reply = _appFx.RecordOperationPerform(Req)

			'Now check if there were any exceptions
			ProcessInteractionExceptions()

			DisplayComparisonResultsInListView(_childValidatedList, lvResults)

		Catch exSoap As System.Web.Services.Protocols.SoapException
			'If an error occurs we attach a SOAP fault error header.
			'You can check the proxy.ResponseErrorHeaderValue to get rich
			'error information including a nicer message copared to the raw exception message.
			Dim wsMsg As String
			If _appFx.ResponseErrorHeaderValue IsNot Nothing Then
				wsMsg = _appFx.ResponseErrorHeaderValue.ErrorText
			Else
				wsMsg = exSoap.Message
			End If
			MsgBox(wsMsg)

		Catch ex As Exception
			MsgBox(ex.ToString)
		Finally
			'Hide hourglass after api call
			Cursor.Current = Cursors.Default
			Cursor.Show()
		End Try
	End Sub

	Private Function GetPhotoCollectionAsXML(ByVal childList As ChildPhotoCollection, ByVal rootElement As String, ByVal includeDate As Boolean) As String
		'turns the collection into XML and returns that XML as a string
		Dim output As New StringBuilder()
		Dim xmlString As String
		Dim xmlWriter__1 As XmlWriter = XmlWriter.Create(output)
		WriteCollectionXml(xmlWriter__1, rootElement, childList, includeDate)
		xmlWriter__1.Close()
		xmlString = output.ToString()

		' DEBUGGING ONLY:
		If _debugging = True Then
			WriteXMLToFile(xmlString, Me.TextBox_SourceFolder.Text & "\", "SampleXMLOutput.xml")
		End If

		Return output.ToString()
	End Function

	Private Sub WriteXMLToFile(ByVal xmlString As String, ByVal folderName As String, ByVal fileName As String)
		Using outfile As New StreamWriter(folderName & fileName, False)
			outfile.WriteLine(xmlString)
			outfile.Close()
		End Using
	End Sub

	Private Sub ProcessInteractionExceptions()
		'Display hourglass during appfx web service calls
		Cursor.Current = Cursors.WaitCursor
		Cursor.Show()

		Dim Req As New Blackbaud.AppFx.WebAPI.ServiceProxy.DataListLoadRequest
		Dim fvSet As New Blackbaud.AppFx.XmlTypes.DataForms.DataFormFieldValueSet
		Dim dfi As New Blackbaud.AppFx.XmlTypes.DataForms.DataFormItem
		Dim Reply As New Blackbaud.AppFx.WebAPI.ServiceProxy.DataListLoadReply
		Req.ClientAppInfo = _clientAppInfoHeader

		'this gets the list of children by XML list of lookupid values:
		Req.DataListID = _interactionExceptionsDataListId	'  "97c4f605-2bbb-4624-acbd-38f67441e630")    ' Child Search data list
		Req.ContextRecordID = _photoHelperSessionId.ToString()	'child Lookup Id  "00000000-0000-0000-0000-000000000000"

		'MsgBox(_photoHelperSessionId.ToString())

		fvSet.Add(New Blackbaud.AppFx.XmlTypes.DataForms.DataFormFieldValue With {.ID = "SESSIONID", .Value = _photoHelperSessionId.ToString()})

		'The DataFormItem is used to hold the set of form fields.
		dfi.Values = fvSet

		'DataFormItem is passed to the request
		Req.Parameters = dfi

		'Max rows acts as a governor to limit the amount of rows retrieved by the datalist
		Req.MaxRows = 500
		Req.IncludeMetaData = True

		Try
			Reply = _appFx.DataListLoad(Req)

			HandleInteractionExceptionsReply(Reply)

		Catch exSoap As System.Web.Services.Protocols.SoapException
			'If an error occurs we attach a SOAP fault error header.
			'You can check the proxy.ResponseErrorHeaderValue to get rich
			'error information including a nicer message copared to the raw exception 			message.
			Dim wsMsg As String
			If _appFx.ResponseErrorHeaderValue IsNot Nothing Then
				wsMsg = _appFx.ResponseErrorHeaderValue.ErrorText
			Else
				wsMsg = exSoap.Message
			End If
			MsgBox(wsMsg)

		Catch ex As Exception
			MsgBox(ex.ToString)
		Finally
			'Hide hourglass after api call
			Cursor.Current = Cursors.Default
			Cursor.Show()
		End Try

	End Sub

	Private Sub HandleInteractionExceptionsReply(ByVal Reply As ServiceProxy.DataListLoadReply)
		'This will handle the reply from the datalist to get any interaction exceptions that happened during the record operation process (create interactions)
		'If there are any exceptions, need to create an output folder for each type of exception:
		'  - unusable photo already completed exception
		'  - annual photo interaction already completed exception
		'  - interaction not found exception
		_alreadyCompletedList = New ChildPhotoCollection()
		_interactionNotFoundList = New ChildPhotoCollection()
		_unusableAlreadyCompletedList = New ChildPhotoCollection()
		Dim isCompleted As Boolean = False
		Dim isNotFound As Boolean = False

		'select CHILDLOOKUPID,
		'       EXCEPTION,
		'		COMPLETEDDATE
		Dim childIdColumn As Integer = 0
		Dim exceptionColumn As Integer = 1
		Dim completionDateColumn As Integer = 2

		'this collection, _childValidatedList, has the children passed to the interactions record op
		'if we couldn't complete the interactions, then the childphoto needs to be moved to the appropriate output folder

		Dim exceptionChild As ChildPhotoData
		'do we have any exceptions at all?
		If Reply.Rows.Count > 0 Then
			exceptionChild = New ChildPhotoData
			isCompleted = False
			isNotFound = False
			For Each row As Blackbaud.AppFx.WebAPI.ServiceProxy.DataListResultRow In Reply.Rows
				'get the offending child photo object
				exceptionChild = _childValidatedList.ChildPhotoList.Where(Function(x) x.ChildLookupId.Equals(row.Values(childIdColumn))).FirstOrDefault()
				'check the exception message and put that row in the appropriate collection
				If row.Values(exceptionColumn).ToString().Equals("Annual photo already completed") Then
					'put it in the already completed collection
					_alreadyCompletedList.ChildPhotoList.Add(exceptionChild)
					_childValidatedList.ChildPhotoList.Remove(exceptionChild)
					'populate the completed date
					exceptionChild.CompletionDate = CDate(row.Values(completionDateColumn))
					isCompleted = True
				End If
				If row.Values(exceptionColumn).ToString().Equals("Interaction Not Found") Then
					'put it in the interaction not found collection
					_interactionNotFoundList.ChildPhotoList.Add(exceptionChild)
					_childValidatedList.ChildPhotoList.Remove(exceptionChild)
					isNotFound = True
				End If
				If row.Values(exceptionColumn).ToString().Equals("Unusable photo already completed") Then
					'put it in the unusable already completed collection if not already there:
					If Not isCompleted Then
						_unusableAlreadyCompletedList.ChildPhotoList.Add(exceptionChild)
						_childValidatedList.ChildPhotoList.Remove(exceptionChild)
						'populate the completed date
						exceptionChild.CompletionDate = CDate(row.Values(completionDateColumn))
					End If
				End If
			Next

		End If

	End Sub

	Private Function GetPhotoFilesAsList(ByVal allFiles As FileInfo()) As List(Of ChildPhotoData)
		' The datalist returns one column for name of child, CHILDNAME which is the NAME field in CRM.
		' Filenames are generally formatted: projectid childlookupid name_Annual Photo 2014.jpg 
		'
		Dim childPhotoList As New List(Of ChildPhotoData)
		Dim childData As New ChildPhotoData
		Dim fileNameItems As String()
		Dim dataCounter As Integer = 0
		Dim fileNameTemp As String = String.Empty
		Dim isValidToParse As Boolean = True
		Dim hasMiddleInitial As Boolean = False

		Dim periodPosition As Integer
		Dim firstNameString As String
		Dim firstNameItems As String()
		Dim firstNameCounter As Integer
		Dim firstNameBuilder As StringBuilder = New StringBuilder()
		Dim lastNameString As String
		Dim lastNameItems As String()
		Dim lastNameCounter As Integer
		Dim lastNameBuilder As StringBuilder = New StringBuilder()
		Dim nameBuilder As StringBuilder = New StringBuilder()

		For Each fl As FileInfo In allFiles
			'Memphis 6/30/14 added:
			hasMiddleInitial = False	'default to False, force to true if we know there is one

			'Memphis 5/13/14: ensure that the filename doesn't have multiple dashes in it
			'                 should only be 1 in the project ID, here's a bad example: 
			'                 DO-105-C228353-Vanessa Martinez.JPG
			'Memphis 5/14/14: need to account for dashes in the name as well, either first or last
			'	HT-004 C232587 Walnise Saint-Fleur.JPG
			'	HT-004 C318492 Roovelt Jean-Baptiste.JPG
			'	HT-004 C318493 Richelson Jean-Baptiste.JPG
			'	HT-004 C318501 Rose-Andrelle Gabrielle.JPG
			'	HT-004 C318738 Phendy Bien-Aime.JPG
			'	HT-004 C319230 Louse-Kenia Fonrose.JPG
			' to accomplish this, just check the first 16 characters, which should encompass the project id, childid and first letter of first name:
			'	HT-004 C319230 L

			'Memphis 5/14/14:  in order for this to work, I must verify that there's a dash in the Project ID value
			'                  so check for a dash in the first 5 characters, if not, throw it out:
			If CountCharacter(fl.ToString().Substring(0, 5), "-") < 1 Then
				'no good, can't continue because all the dash and file field parse counts will be off:
				isValidToParse = False
			Else
				'need to reset this flag since we're in a loop and it could stay false incorrectly
				isValidToParse = True
			End If

			If isValidToParse = True AndAlso CountCharacter(fl.ToString().Substring(0, 16), "-") = 1 Then
				'populate collection of ChildPhotoData objects from filenames
				fileNameTemp = fl.Name.Replace(_photoyearfile, "").Trim()

				'check for the lowercase extension
				If fileNameTemp.Contains(_photoyearfile_lowercase) Then
					fileNameTemp = fileNameTemp.Replace(_photoyearfile_lowercase, "").Trim()
				End If

				'check for the case where a space is after the photo year:  "2014 .JPG"
				If fileNameTemp.Contains(_photoyearfile_withspace) Then
					fileNameTemp = fileNameTemp.Replace(_photoyearfile_withspace, "").Trim()
				End If

				'check for the lowercase ".jpg" presence in the filename
				If fileNameTemp.Contains(".jpg") Then
					fileNameTemp = fileNameTemp.Replace(".jpg", "").Trim()
				End If

				'check for the uppercase ".JPG" presence in the filename
				If fileNameTemp.Contains(".JPG") Then
					fileNameTemp = fileNameTemp.Replace(".JPG", "").Trim()
				End If

				'6/5/14: Memphis make sure there's a space after middle initial and last name, before splitting:
				'If fileNameTemp.Contains(".") Then
				'	fileNameTemp = AddSpacesToSentence(fileNameTemp, True)
				'End If

				'6/30/14:  Memphis

				'If the filename has a period in it, assume that's the middle initial
				If fileNameTemp.Contains(".") Then
					'If there are more than one period characters in the filename, we don't know which is the middle initial,
					'  so we have to check the count, if greater than 1 we assume we can't get the middle initial:
					If CountCharacter(fileNameTemp, ".") = 1 Then
						'  If the filename has a period in it, assume that's the middle initial
						'    -Any words with spaces before the middle initial are the 1st name
						'      - Split the substring from starting to IndexOf the period "."
						'set the Property to indicate this photofile has a middle initial.
						hasMiddleInitial = True
					Else
						'is there a way to strip out the extra period character?
						'	'probably a trailing period, so remove that one if it is:
						'Check if this is a trailing period:
						Dim lastPeriodIndex As Integer = fileNameTemp.LastIndexOf(".")
						'compare length of string to the position of the last period, if close, 
						'then call it a trailing period in the filename:
						If fileNameTemp.Length - lastPeriodIndex <= 2 Then
							'this is probably a trailing period, so get rid of it:
							fileNameTemp = fileNameTemp.Substring(0, lastPeriodIndex)
							hasMiddleInitial = True
						Else
							'	Dim tempString = fileNameTemp.Substring(0, fileNameTemp.LastIndexOf("."))
							'	fileNameItems = AddSpacesToSentence(tempString.Substring(14).Trim(), True).Split(" ")
							'	dataCounter = fileNameItems.Count - 1
							hasMiddleInitial = False
						End If
					End If
				Else
					' If the filename does NOT have a period in it (middle name), then all we can do is:
					'   strip and compress the string and compare it to the stripped and compressed CRM:
					'   - compare against the entire CRM name
					'   - if not match, compare against the CRM FirstName and LastName
					'set the Property to indicate this photofile does NOT have a middle initial.
					'MsgBox("There is no middle name, so unable to parse into names, can only compress and strip and compare...")
					hasMiddleInitial = False
				End If
				' END OF 6/30/14 add

				If hasMiddleInitial = True Then
					periodPosition = fileNameTemp.IndexOf(".")
					firstNameString = fileNameTemp.Substring(0, periodPosition)
					firstNameItems = firstNameString.Split(" ")
					firstNameCounter = firstNameItems.Count - 1
					firstNameBuilder = New StringBuilder()
					'child project is first
					'MsgBox(String.Format("Project: {0}", firstNameItems(0)))
					'child id is 2nd
					'MsgBox(String.Format("ChildId: {0}", firstNameItems(1)))

					'Memphis: 7/7/14: we do NOT want to include the middle name/initial in the firstname
					' so don't append the last item in the split array:
					For counter = 2 To firstNameCounter - 1
						'MsgBox(String.Format("Counter: {0} {1}", CStr(counter), firstNameItems(counter)))
						firstNameBuilder.Append(firstNameItems(counter))
					Next
					'MsgBox(String.Format("firstNameBuilder: {0}", firstNameBuilder.ToString()))

					'Parse last names:
					'    -Any words after the middle initial are the last name
					'      - Split the substring from the IndexOf the period "." to the end of the string
					lastNameString = fileNameTemp.Substring(periodPosition + 1).Trim()
					lastNameItems = lastNameString.Split(" ")
					lastNameCounter = lastNameItems.Count - 1
					lastNameBuilder = New StringBuilder()
					For counter = 0 To lastNameCounter
						'MsgBox(String.Format("Counter: {0} {1}", CStr(counter), lastNameItems(counter)))
						lastNameBuilder.Append(lastNameItems(counter))
					Next
					'MsgBox(String.Format("lastNameBuilder: {0}", lastNameBuilder.ToString()))

					'populate the childData object:
					childData = New ChildPhotoData()  'lookupid, projectid, name
					childData.ChildProject = firstNameItems(0)
					childData.ChildLookupId = firstNameItems(1)
					childData.PhotoFile = fl.Name
					childData.FileFirstName = firstNameBuilder.ToString()
					childData.FileLastName = lastNameBuilder.ToString()
					childData.HasMiddleInitial = hasMiddleInitial
					childData.ChildName = childData.FileFirstName.Replace(" ", "").Trim() + childData.FileLastName.Replace(" ", "").Trim()
				Else
					'populate the childData object:
					fileNameItems = fileNameTemp.Split(" ")
					dataCounter = fileNameItems.Count - 1
					childData = New ChildPhotoData()  'lookupid, projectid, name
					childData.ChildProject = fileNameItems(0)
					childData.ChildLookupId = fileNameItems(1)
					childData.PhotoFile = fl.Name
					childData.HasMiddleInitial = hasMiddleInitial
					'concatenate the names values
					nameBuilder.Clear()
					For fileCounter = 2 To dataCounter
						If Not fileNameItems(fileCounter).ToLower().Contains("copy") Then
							nameBuilder.Append(fileNameItems(fileCounter))
						End If
						'Test the string to see if there are only two values in it, if so, assume 1st is FirstName, 2nd is LastName
						' if there are, then dataCounter will be 3, and items 2 and 3 will be 1st & last name respectively:
						If dataCounter = 3 Then
							If fileCounter = 2 Then
								'populate firstname:
								childData.FileFirstName = fileNameItems(fileCounter)
							End If
							If fileCounter = 3 Then
								'populate lastname:
								childData.FileLastName = fileNameItems(fileCounter)
							End If
						End If
					Next
					childData.ChildName = nameBuilder.ToString().Replace(" ", "").Trim()
				End If

				'MsgBox(String.Format("Project: {0}", childData.ChildProject))
				'MsgBox(String.Format("ChildId: {0}", childData.ChildLookupId))
				'MsgBox(String.Format("PhotoFile: {0}", childData.PhotoFile))
				'MsgBox(String.Format("FileFirstName: {0}", childData.FileFirstName))
				'MsgBox(String.Format("FileLastName: {0}", childData.FileLastName))
				'MsgBox(String.Format("ChildName: {0}", childData.ChildName))

				'Memphis 7/7 commented out, replaced with code above:
				'fileNameItems = fileNameTemp.Split(" ")
				'dataCounter = fileNameItems.Count - 1
				'childData = New ChildPhotoData()  'lookupid, projectid, name
				'childData.ChildProject = fileNameItems(0)
				'childData.ChildLookupId = fileNameItems(1)
				'childData.PhotoFile = fl.Name
				'childData.FileFirstName = fileNameItems(2)
				'childData.HasMiddleInitial = hasMiddleInitial
				'Memphis 7/7 end of comment out:

				'AddSpacesToSentence(fileNameTemp.Substring(14).Trim(), True)
				'text.Split(".").Length -1
				'Memphis 7/7: took this out, no longer necessary:
				'Dim periodCount As Integer = fileNameTemp.Split(".").Length - 1
				'If periodCount = 1 Then
				'	fileNameItems = AddSpacesToSentence(fileNameTemp.Substring(14).Trim(), True).Split(" ")
				'	dataCounter = fileNameItems.Count - 1
				'ElseIf periodCount > 1 Then
				'	'probably a trailing period, so remove that one:
				'	Dim tempString = fileNameTemp.Substring(0, fileNameTemp.LastIndexOf("."))
				'	fileNameItems = AddSpacesToSentence(tempString.Substring(14).Trim(), True).Split(" ")
				'	dataCounter = fileNameItems.Count - 1
				'End If
				'end 7/7


				'Memphis 7/7 moved up above:
				'concatenate the names values
				'Dim nameBuilder As StringBuilder = New StringBuilder()
				'For fileCounter = 2 To dataCounter
				'	If Not fileNameItems(fileCounter).ToLower().Contains("copy") Then
				'		nameBuilder.Append(fileNameItems(fileCounter))
				'	End If
				'	If fileCounter = 3 Then
				'		childData.FileLastName = fileNameItems(fileCounter)
				'	End If
				'Next
				'childData.ChildName = nameBuilder.ToString().Replace(" ", "").Trim()
				'end of move

				'if there is not a C in the childlookupid then we can't use this one:
				If childData.ChildLookupId.ToLower().StartsWith("c") Then
					'make sure the ChildLookup ID value starts with uppercase "C":
					childData.ChildLookupId = childData.ChildLookupId.ToUpper()
					childPhotoList.Add(childData)
				Else
					_fileNameParseErrorList.ChildPhotoList.Add(childData)
				End If
			Else
				'file is no good, throw it out...
				childData = New ChildPhotoData()  'lookupid, projectid, name
				childData.PhotoFile = fl.Name
				_fileNameParseErrorList.ChildPhotoList.Add(childData)
			End If

		Next

		Return childPhotoList

	End Function

	''' <summary>
	''' This will count the number of characters in the given string.
	''' </summary>
	''' <param name="value"></param>
	''' <param name="ch"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
		Return value.Count(Function(c As Char) c = ch)
		'value.Count(Function(c) c = "$"c)
	End Function


	Private Sub GetSetConfigSettingsValues()
		_notincrmfoldername = My.Settings.NOTINCRMFOLDERNAME  ' "Children_Not_In_CRM\"
		_unmatchedprojectfoldername = My.Settings.UNMATCHEDPROJECTFOLDERNAME  ' "Incorrect_Project_ID\"
		_unmatchednamefoldername = My.Settings.UNMATCHEDNAMEFOLDERNAME	 '"Names_Not_Match\"
		_alreadycompletedfoldername = My.Settings.ALREADYCOMPLETEDFOLDERNAME ' "Already_Completed_Exceptions\"
		_unusablealreadycompletedfoldername = My.Settings.UNUSABLEALREADYCOMPLETEDFOLDERNAME ' "Unusable_Already_Completed_Exceptions\"
		_interactionnotfoundfoldername = My.Settings.INTERACTIONNOTFOUNDFOLDERNAME ' "Interactions_Not_In_CRM_Exceptions\"
		_fileNameParseErrorFolderName = My.Settings.FILENAMEPARSEERRORFOLDERNAME   ' "Filename_Parse_Error\"
		_completedFolderName = My.Settings.VALIDATEDCHILDRENFOLDERNAME
		_recordopname = My.Settings.RECORDOPNAME ' "Child Interaction Annual Photo Update Record Operation"	 ' "Completes Annual Photo Interactions Record Op"
		_photoyearfile = My.Settings.PHOTOYEARFILE ' "2014.JPG"
		_photoyearfile_lowercase = My.Settings.PHOTOYEARFILE_LOWERCASE	'"2014.jpg"
		_photoyearfile_withspace = My.Settings.PHOTOYEARFILE_WITHSPACE	'"2014 .JPG"
		_debugging = My.Settings.DEBUGGING



		'these settings are environment specific:
		If My.Settings.TARGET_ENVIRONMENT.ToLower().Equals("dev") Then
			_securelyStoredUserName = My.Settings.DEVUN	' "PhotoAPIUser21195D"	' "MobileServices21195p"
			_securelyStoredPassword = My.Settings.DEVPW	 ' "P@ssword1"						'"7Fny8kbmDxr4"
			_appFxURL = My.Settings.DEVURL
			_dbName = My.Settings.DEV_DB
		Else
			_securelyStoredUserName = My.Settings.PRODUN	' "PhotoAPIUser21195D"	' "MobileServices21195p"
			_securelyStoredPassword = My.Settings.PRODPW	 ' "P@ssword1"						'"7Fny8kbmDxr4"
			_appFxURL = My.Settings.PRODURL
			_dbName = My.Settings.PROD_DB
		End If

		If _debugging = True Then
			MsgBox(My.Settings.TARGET_ENVIRONMENT)
			MsgBox(_appFxURL)
			MsgBox(_dbName)
			MsgBox(_securelyStoredPassword)
			MsgBox(_securelyStoredUserName)
		End If

		'set the display label value:
		Me.environmentLabel.Text = My.Settings.TARGET_ENVIRONMENT
		Me.environmentLabel.Visible = True

	End Sub

	Private Sub nameValidationOverrideCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nameValidationOverrideCheckBox.CheckedChanged
		If nameValidationOverrideCheckBox.Checked = True Then
			'prompt user to confirm:
			If MessageBox.Show("This will prevent any name checking!  Are you sure?", "Annual Photo Helper", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.No Then
				nameValidationOverrideCheckBox.Checked = False
			End If
		End If
		Me.TrackBar1.Enabled = Not nameValidationOverrideCheckBox.Checked
	End Sub

	Private Sub InitializeEverything()
		GetSetConfigSettingsValues()

		'5/30/14
		' set to medium strict (middle)
		'2/12/15 per Laura, set to most strict (high)
		Me.TrackBar1.Value = 2

		'probably make these config values??:
		'by default, when loading or reset, set to medium strictness:
		_jwProximityThreshold = _jwProximityMedium
		_dlDistanceThreshold = _dlDistanceMedium

		'this resets/restarts stuff, called by Form Load and by Reset button
		InitializeAppFxWebService()

		'turn off reset button
		resetButton.Enabled = False

		'turn on the validation button
		validatePhotosButton.Enabled = True

		outputNotInCRMLabel.Visible = False
		namesNotMatchOutputLabel.Visible = False
		projectNotMatchOutputLabel.Visible = False
		lblAlreadyCompletedErrors.Visible = False
		lblFilenameParseErrors.Visible = False

		ftpRadioButton.Checked = True
		otherLabel.Visible = False
		otherSourceText.Visible = False
		nameValidationOverrideCheckBox.Checked = False

		'this is passed into record operation and used for the insert into any photo interaction exceptions
		If _debugging = True Then
			_photoHelperSessionId = New Guid("BD1357AE-6FC8-4078-A49E-E45F05E3C3EA")
		Else
			_photoHelperSessionId = Guid.NewGuid()
		End If

		'empty out all the collections
		If Not _childValidatedList.ChildPhotoList Is Nothing Then
			_childValidatedList.ChildPhotoList.Clear()
		End If

		If Not _notInCrmList.ChildPhotoList Is Nothing Then
			_notInCrmList.ChildPhotoList.Clear()
		End If

		If Not _projectIdNotMatchList.ChildPhotoList Is Nothing Then
			_projectIdNotMatchList.ChildPhotoList.Clear()
		End If

		If Not _nameNotMatchList.ChildPhotoList Is Nothing Then
			_nameNotMatchList.ChildPhotoList.Clear()
		End If

		'If Not _interactionExceptions.ChildPhotoList Is Nothing Then
		'	_interactionExceptions.ChildPhotoList.Clear()
		'End If

		If Not _alreadyCompletedList.ChildPhotoList Is Nothing Then
			_alreadyCompletedList.ChildPhotoList.Clear()
		End If

		If Not _interactionNotFoundList.ChildPhotoList Is Nothing Then
			_interactionNotFoundList.ChildPhotoList.Clear()
		End If

		If Not _unusableAlreadyCompletedList.ChildPhotoList Is Nothing Then
			_unusableAlreadyCompletedList.ChildPhotoList.Clear()
		End If

		If Not _fileNameParseErrorList.ChildPhotoList Is Nothing Then
			_fileNameParseErrorList.ChildPhotoList.Clear()
		End If

		'clear out the source folder textbox
		Me.TextBox_SourceFolder.Text = String.Empty
		Me.otherSourceText.Text = String.Empty

		'clear out the listview
		lvResults.Clear()
		lblEventsTitle.Text = String.Format("{0} Children Validated:", _childValidatedList.ChildPhotoList.Count)

	End Sub

	Private Sub resetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles resetButton.Click
		InitializeEverything()
		resetButton.Enabled = False
	End Sub


	Private Sub otherRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles otherRadioButton.CheckedChanged
		otherLabel.Visible = otherRadioButton.Checked
		otherSourceText.Visible = otherRadioButton.Checked
	End Sub


	Private Sub CleanoutExtraChars(ByRef stringValue As String)
		If stringValue.Contains(".") Then
			stringValue = stringValue.Replace(".", "")
		End If
		If stringValue.Contains("-") Then
			stringValue = stringValue.Replace("-", "")
		End If
		If stringValue.Contains("_") Then
			stringValue = stringValue.Replace("_", "")
		End If

	End Sub

	Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
		'user can select between 0-2: 0 is least strict, 2 is most stric
		If Me.TrackBar1.Value = 0 Then
			_jwProximityThreshold = _jwProximityLow
			_dlDistanceThreshold = _dlDistanceLow
		End If

		If Me.TrackBar1.Value = 1 Then
			_jwProximityThreshold = _jwProximityMedium
			_dlDistanceThreshold = _dlDistanceMedium
		End If

		If Me.TrackBar1.Value > 1 Then
			_jwProximityThreshold = _jwProximityHigh
			_dlDistanceThreshold = _dlDistanceHigh
		End If
	End Sub

	Private Function AddSpacesToSentence(ByVal text As String, ByVal preserveAcronyms As Boolean) As String
		If String.IsNullOrWhiteSpace(text) Then
			Return String.Empty
		End If
		Dim newText As New StringBuilder(text.Length * 2)
		newText.Append(text(0))
		For i As Integer = 1 To text.Length - 1
			If Char.IsUpper(text(i)) Then
				If (text(i - 1) <> " "c AndAlso Not Char.IsUpper(text(i - 1))) OrElse (preserveAcronyms AndAlso Char.IsUpper(text(i - 1)) AndAlso i < text.Length - 1 AndAlso Not Char.IsUpper(text(i + 1))) Then
					newText.Append(" "c)
				End If
			End If
			newText.Append(text(i))
		Next
		Return newText.ToString()
	End Function

End Class
