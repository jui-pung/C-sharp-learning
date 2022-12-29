# XSD介紹
###### tags: `c# 程式觀念`
## 什麼是XSD?為什麼要用XSD?
如果從外部取得資料，希望來源符合我們自己的規則，提供元素與屬性的資料型態
### Ways to Validate
- Well Formed(w3c)
相信作者根據一些規則所寫的XML型式是正確的，不一定要做驗證，規則包括必須有唯一的根元素，所有元素必須有開始結束標籤，屬性值必須在雙引號中，類似於HTML型式
- DTD(Document type definition)
Elements and attributes that may appear in the document
Number and order of children
它會規範文件的元素與屬性以及子元素的數量和順序，但是沒有規範資料型態
- XSD
Elements and attributes that may appear in the document
Number and order of children
Data types for elements and attributes
Defaults and fixed values for elements and attributes
```
<h2> XSD額外可以知道在h2標籤裡的內容與資料型態 </h2>
```
### Why Schema Rules? 為什麼要有XML描述的規則?
XML常作為通信傳送的一種方式
Ensure valid data before importing
下圖是使用XSD規範描述的XML文件與XSD文件

![](https://i.imgur.com/fhxBagv.jpg)

XSD文件也是像XML一樣的樹狀結構，根元素 稱為Schema Element
XML Root Element several items:
- xmlns default namespace
- xmlns:xsi http://www.w3.org/2001/XMLSchema
- xsi:schemaLocation Where your XSD file is

![](https://i.imgur.com/jMoO85K.jpg)

### Simple Types Available
在XML文件中沒有屬性、沒有子元素的元素，例如上方圖片的firstname,lastname等標籤   
Types available:   
> xs:string
> xs:decimal
> xs:integer
> xs:boolean
> xs:date
> xs:time

## 實作XSD並連結到XML文件(使用Notepad++)
- XSD內容-細節註解於程式碼中
```xml=
<?xml version="1.0" encoding="UTF-8"?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
	<xs:element name="root">
	<!-- XSD的驗證規則將會從xs:element tag 進入 -->
		<xs:complexType>
			<xs:sequence>
			<!-- 以下的element會依照順序排列 -->
				<xs:element name="qtype" type="xs:string"/>
				<xs:element name="bhno" type="xs:string"/>
				<xs:element name="cseq" type="xs:string"/>
				<!-- minOccurs=0代表不是必須的element -->
				<xs:element name="notRequired" type="xs:string" minOccurs="0"/>
			</xs:sequence>
		</xs:complexType>
	</xs:element>
</xs:schema>
```
- 若輸入的XML文件格式如下，會出現錯誤訊息，因為'root'元素內容不全(使用Notepad++中的外掛XML Tools驗證 "XML Tools -> Validate Now") 
必須再加上<bhno>592S</bhno>與<cseq>0000527</cseq>標籤內容
```xml=
<?xml version="1.0" encoding="UTF-8"?>
<root xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="root.xsd">
	<qtype>0001</qtype>
</root>	
```
## Database Table產生XSD實作方式
### 1.將Database Table名稱與Table欄位名稱存為.xml檔
```csharp=
string dbName = "ESMP";
string connstr = $"Server=localhost;Integrated security=SSPI;database={dbName}";
string cmdstr = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
List<string> tableNames = new List<string>();
using (SqlConnection conn = new SqlConnection(connstr))
{
	using (SqlCommand cmd = new SqlCommand(cmdstr, conn))
	{
		try
		{
			conn.Open();
			SqlDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				tableNames.Add((string)reader["TABLE_NAME"]);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
		finally
		{
			conn.Close();
		}
	}
}
cmdstr = "";
foreach (var item in tableNames)
{
	cmdstr = String.Concat(cmdstr, $"SELECT Top 0 * FROM dbo.{item}; ");
}
DataSet dbESMP = new DataSet();
dbESMP.DataSetName = "dbESMP";
using (SqlConnection conn = new SqlConnection(connstr))
{
	using (SqlCommand cmd = new SqlCommand(cmdstr, conn))
	{
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			adapter.SelectCommand = cmd;

			conn.Open();
			adapter.Fill(dbESMP);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
		finally
		{
			conn.Close();
		}
	}
}
for (int i = 0; i < dbESMP.Tables.Count; i++)
{
	dbESMP.Tables[i].TableName = tableNames[i];
}
dbESMP.WriteXml(@"C:\temp\dbESMP.xml", XmlWriteMode.WriteSchema);

```
### 2.XML轉換至XSD與C#類別
我們需要有 (XML Schema Definition) XSD.exe 才能將 xsd 轉換為類，如果已經安裝 Visual Studio，它將自動安裝並作為 Windows 軟件開發工具包的一部分提供。  
- XSD.exe檔案路徑
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools"(依安裝.net版本)
- 將上述產生的.xml檔放置於與XSD.exe相同資料夾下
- 以系統管理員開啟命令指示字元 並cd到XSD.exe檔案路徑 依序執行
```
xsd.exe YourFileName.xml
```
```
xsd /c YourFileName.xsd
```
xsd檔案與cs檔案將會生成於資料夾下

## 參考資料
[Generate Class from XSD in C#](https://qawithexperts.com/article/c-sharp/generate-class-from-xsd-in-c-using-cmd-or-visual-studio/277)   
[how to create c# class from Xsd](https://stackoverflow.com/questions/61959993/how-to-create-c-sharp-class-from-xsd)   
[產生內嵌 XSD 架構 MSDN](https://learn.microsoft.com/zh-tw/sql/relational-databases/xml/generate-an-inline-xsd-schema?view=sql-server-ver16)