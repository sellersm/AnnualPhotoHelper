Imports System.Xml.Serialization

<Serializable()> Public Class ChildPhotoData
	Implements IComparable

	Private _crmName As String
	Private _childLookupId As String
	Private _childProject As String
	Private _childName As String
	Private _fileName As String
	'Private _lastName As String
	Private _completionDate As Date

	<XmlElement(ElementName:="CHILDLOOKUPID")>
	Public Property ChildLookupId() As String
		Get
			Return _childLookupId
		End Get
		Set(ByVal value As String)
			_childLookupId = value
		End Set
	End Property

	Public Property ChildName() As String
		Get
			Return _childName
		End Get
		Set(ByVal value As String)
			_childName = value
		End Set
	End Property

	'Public Property LastName() As String
	'	Get
	'		Return _lastName
	'	End Get
	'	Set(ByVal value As String)
	'		_lastName = value
	'	End Set
	'End Property

	Public Property ChildProject() As String
		Get
			Return _childProject
		End Get
		Set(ByVal value As String)
			_childProject = value
		End Set
	End Property

	Public Property PhotoFile As String
		Get
			Return _fileName
		End Get
		Set(ByVal value As String)
			_fileName = value
		End Set
	End Property

	Public Property CRMChildName() As String
		Get
			Return _crmName
		End Get
		Set(ByVal value As String)
			_crmName = value
		End Set
	End Property

	Public Property CompletionDate() As Date
		Get
			Return _completionDate
		End Get
		Set(ByVal value As Date)
			_completionDate = value
		End Set
	End Property


	Public Sub New(ByVal lookupid As String, ByVal project As String, ByVal childName As String)
		_childLookupId = lookupid
		_childProject = project
		_childName = childName
	End Sub

	Public Sub New(ByVal lookupid As String, ByVal project As String, ByVal childName As String, ByVal fileName As String)
		_childLookupId = lookupid
		_childProject = project
		_childName = childName
		_fileName = fileName
	End Sub

	Public Sub New(ByVal lookupid As String, ByVal project As String, ByVal childName As String, ByVal fileName As String, ByVal crmChildName As String)
		_childLookupId = lookupid
		_childProject = project
		_childName = childName
		_fileName = fileName
		_crmName = crmChildName
	End Sub


	Public Sub New()
		_childLookupId = String.Empty
		_childProject = String.Empty
		_childName = String.Empty
		_fileName = String.Empty
		_crmName = String.Empty
		_completionDate = Nothing
	End Sub

	Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
		If obj Is Nothing Then Return 1

		Dim otherPhotoData As ChildPhotoData = TryCast(obj, ChildPhotoData)
		If otherPhotoData IsNot Nothing Then
			Return Me.ChildLookupId.CompareTo(otherPhotoData.ChildLookupId) AndAlso Me.ChildProject.CompareTo(otherPhotoData.ChildProject) AndAlso Me.ChildName.CompareTo(otherPhotoData.ChildName)
		Else
			Throw New ArgumentException("Object is not a ChildPhotoData")
		End If
	End Function
End Class
