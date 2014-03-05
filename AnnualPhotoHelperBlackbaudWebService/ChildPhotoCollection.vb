Imports System.Xml.Serialization

<Serializable()> Public Class ChildPhotoCollection

	Private _childPhotoList As List(Of ChildPhotoData)


	<XmlElement(ElementName:="CHILDPHOTOCOLLECTION")>
	Public Property ChildPhotoList() As List(Of ChildPhotoData)
		Get
			Return _childPhotoList
		End Get
		Set(ByVal value As List(Of ChildPhotoData))
			_childPhotoList = value
		End Set
	End Property

	Public Sub SortList()
		_childPhotoList.Sort(Function(p1, p2) p1.ChildLookupId.CompareTo(p2.ChildLookupId))
	End Sub

	Public Sub New()
		_childPhotoList = New List(Of ChildPhotoData)
	End Sub

End Class
