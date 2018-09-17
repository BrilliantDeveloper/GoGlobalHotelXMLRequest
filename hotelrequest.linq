<Query Kind="VBProgram" />

Sub Main
	Dim xml = GoGlobalHotelXMLRequest.CreateRequestString(1111, 2222, #2018-09-27#, 5)
	Console.WriteLine(xml)
	Console.ReadLine()
End Sub

' Define other methods and classes here

' Define other methods and classes here
Public Class GoGlobalHotelXMLRequest



	Public Shared Function CreateRequestString(Byval hotelID As Integer, cityID As Integer, _
		arrivalDate As Date, noNights As Integer)

		Dim requestString As String = getXML()
		Dim sbHotelRequest As New StringBuilder

		If Not String.IsNullOrEmpty(requestString)
			With sbHotelRequest
				.Append(requestString)

				If hotelID > 0 Then
					.Replace("##HotelID##", hotelID)
				End If

				If cityID > 0 Then
					.Replace("##CityCode##", cityID)
				End If
				
				.Replace("##ArrivalDate##", arrivalDate)
				.Replace("##Nights##", noNights)
				requestString = .ToString()
			End With
		End If

		Return requestString
	End Function

	Public Shared Function getXML()
		Dim xmlRequest As String
		xmlRequest = My.Computer.FileSystem.ReadAllText("C:\Users\swise\Repos\BTI\goglobal_classes\GoGlobalHotelXMLRequest\request.xml")
		Return xmlRequest
	End Function
End Class

Public Class Assertions
	Public Shared Sub AssertEqual(Byval actual, expected, testName)

		If actual = expected Then
			Console.WriteLine("Passed.")
			Console.ReadLine()
		Else
			Console.WriteLine("Failed [" + testName + "] Expected " + expected + " but got " + actual)
			Console.ReadLine()

		End If
	End Sub

End Class