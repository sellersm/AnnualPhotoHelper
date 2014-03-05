Public Class ChildPhotoComparer
	Implements IEqualityComparer(Of ChildPhotoData)

	Public Overloads Function Equals(ByVal x As ChildPhotoData, ByVal y As ChildPhotoData) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of ChildPhotoData).Equals
		'Check whether the objects are the same object.  
		If [Object].ReferenceEquals(x, y) Then
			Return True
		End If

		'Check whether the products' properties are equal.  
		Return x IsNot Nothing AndAlso y IsNot Nothing AndAlso x.ChildLookupId.Equals(y.ChildLookupId) _
		 AndAlso x.ChildProject.Equals(y.ChildProject) AndAlso x.ChildName.Equals(y.ChildName)
	End Function

	Public Overloads Function GetHashCode(ByVal obj As ChildPhotoData) As Integer Implements System.Collections.Generic.IEqualityComparer(Of ChildPhotoData).GetHashCode
		'Get hash code for the LookupId field if it is not null.  
		Dim hashLookupId As Integer = If(obj.ChildLookupId Is Nothing, 0, obj.ChildLookupId.GetHashCode())

		'Get hash code for the ProjectId field.  
		Dim hashProjectId As Integer = obj.ChildProject.GetHashCode()

		'Get hash code for the ChildName field.  
		Dim hashChildName As Integer = obj.ChildName.GetHashCode()

		'Calculate the hash code for the product.  
		Return hashLookupId Xor hashProjectId Xor hashChildName
	End Function
End Class