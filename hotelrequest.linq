<Query Kind="VBProgram">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.EnterpriseServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.RegularExpressions.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Design.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.ApplicationServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.Protocols.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceProcess.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Services.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Utilities.v4.0.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Caching.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Framework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Tasks.v4.0.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Web</Namespace>
</Query>

Sub Main
	
	Dim ggXMlRequest As New GoGlobalHotelXMLRequest
	
	ggXMlRequest.PostRequest(226, 75, #2018-11-18#, 3)
	
	Console.WriteLine(ggXMlRequest.xmlErrorText)
	Console.ReadLine()
	
End Sub

' Define other methods and classes here

' Define other methods and classes here
Public Class GoGlobalHotelXMLRequest
	
	Public  xmlErrorText As String = ""
	

	Public  Function IsErrorResponse(Byval xmlResponse As String) As Boolean
		Dim searchResult As XmlNodeList = FindElement(xmlResponse, "Error")

		If searchResult.Count > 0 Then
			xmlErrorText = searchResult(0).InnerText
			Return True
		End If			
		Return False
	End Function
	
	Public  Function FindElement(Byval xmlResponse As String, Byval element As String) As XmlNodeList
		Dim mXMLResponse As New XmlDocument
		mXMLResponse.LoadXml(xmlResponse)
		Dim lstSearchResult As XmlNodeList = mXMLResponse.GetElementsByTagName(element)

		Return lstSearchResult
	End Function	

	Public  Sub PostRequest(Byval hotelID, Byval cityID, Byval arrivalDate, Byval noNights)
		Dim uri As New Uri("http://xml.qa.goglobal.travel/XMLWebService.asmx")

		Dim request As HttpWebRequest = HttpWebRequest.Create(uri)
		Dim xml As String = CreateRequestString(hotelID, cityID, arrivalDate, noNights)
		
		Console.WriteLine(xml)
		request.Method = WebRequestMethods.Http.Post
		request.ContentLength = xml.Length
		request.ContentType = "text/xml"

		Dim writer As New StreamWriter(request.GetRequestStream)
		writer.Write(xml)
		writer.Close()

		Dim oResponse As HttpWebResponse = request.GetResponse()
		Dim reader As New StreamReader(oResponse.GetResponseStream())
		Dim strResponse As String = HttpUtility.HtmlDecode(reader.ReadToEnd())		
		oResponse.Close()
		
		If Not IsErrorResponse(strResponse) Then
			
		End If	
	End Sub	
	
	Public  Function CreateRequestString(Byval hotelID As Integer, cityID As Integer, _
		arrivalDate As Date, noNights As Integer)

		Dim requestString As String = getXML()
		Dim sbHotelRequest As New StringBuilder

		If Not String.IsNullOrEmpty(requestString)
			With sbHotelRequest
				.Append(requestString)

				If hotelID > 0 Then
					.Replace("##HotelID##", hotelID)
				Else
					.Replace("##HotelID##", "")
				End If

				If cityID > 0 Then
					.Replace("##CityCode##", cityID)
				Else
					.Replace("##CityCode##", "")
				End If
				
				.Replace("##ArrivalDate##", arrivalDate.ToString("yyyy-MM-dd"))
				.Replace("##Nights##", noNights)
				requestString = .ToString()
			End With
		End If

		Return requestString
	End Function

	Public  Function getXML()
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