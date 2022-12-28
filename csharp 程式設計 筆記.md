# c# 程式設計 筆記
###### tags: `c# 程式觀念`
> 紀錄一些C#程式設計觀念

## 部分類別 (Partial Class)
C# 2.0 開始支援部份類別 (Partial Class) 的機制，在同一個Class底下也可以分別寫在不同的來源檔案(source code file)，而當編譯應用程式時，由編譯器將所有區段結合為單一類別  
- 使用時機
處理大型專案時，將類別分散到不同的檔案可讓多位程式設計人員同時處理  
處理自動產生的來源時，程式碼可以加入類別，不必重新建立來源檔案(例如:WinForm的Form1.cs, Form1.designer.cs即是以partial方式撰寫)  
注意:部分類別需要是相同的命名空間(namespace)
![](https://i.imgur.com/Vc9rFsO.png)
- 優點
多個開發者可以同步開發一個Class  
避免一個類別的程式碼過於冗長  

## 覆寫 ToString 方法
C# 中的每個類別或結構都會隱含地繼承 Object 類別。 因此，C# 中的每個物件都會取得 ToString 方法，以傳回該物件的字串表示  
- 使用原因
當自訂類別呼叫 ToString 方法時，系統會直接叫用Systrm.Object類別的ToString方法。返回值多半是類別名稱等較不具識別性的字串  
所以自訂類別應該覆寫 ToString 方法，將自訂類別的資訊提供給開發人員
```csharp=
//自訂Person類別
class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    //覆寫ToString 方法一
    public override string ToString()
    {
        return "Person: " + Name + " " + Age;
    }
    //覆寫ToString 方法二
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Name:");
        sb.Append(Name);
        sb.Append(" ");
        sb.Append("Age:");
        sb.Append(Age);
        return sb.ToString();
    }
}
```

## 列舉(enum)型別
列舉型別是由基礎整數數值類型的一組具名常數所定義。若要定義列舉型別，使用 enum 關鍵字並指定列舉成員的名稱  
下面範例使用列舉型別(Parameters)指定列舉成員BHNO、CSEQ
```csharp=
public class ToolKit
{
    public enum Parameters
    {
        BHNO,
        CSEQ
    }
    /// <summary>
    /// 檢核必填欄位是否為空
    /// </summary>
    /// <param name="value"></param>
    /// <param name="parameter"></param>
    public static void CheckQueryParameter(string value, Parameters parameter)
    {
        if (parameter == Parameters.BHNO && value == "")
        {
            Console.WriteLine("Error");
        }
    }
}
```

## HttpWebRequest, HttpWebResponse類別
專案執行上需要在程式邏輯中呼叫另一個URL的網址並取得回傳資料，可以使用 HttpWebRequest 類別，下面介紹在HTTP中最常用的POST與GET方法   
### GET
- GET 是直接將要傳送的資料以 Query String（一種Key/Vaule的編碼方式）加在傳送的URL地址後面   
範例：http://10.10.10.10:8080/Quote/Stock.jsp?stock=0050,0056,6214
上述範例的Query String為0050,0056,6214
- 瀏覽器的網址列可以看見傳送的資料，若是要傳送密碼就會產生安全性的問題
- ==此方法要注意傳送的URL長度== [這裡是一些URL長度的討論(Maximum length of HTTP GET request)](https://dotblogs.com.tw/joysdw12/2012/12/04/85380)   [( URL 長度有沒有一定的限制)](https://blog.miniasp.com/post/2022/07/19/Maximum-length-of-URL-in-browsers-and-servers)   
255 bytes is the safest length to assume that the entire URL will come in
```csharp=
string targetUrl = "(要查詢的URL)";
HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
request.Method = "GET";
request.ContentType = "application/x-www-form-urlencoded";
request.Timeout = 30000;
string result = "";
// 取得回應資料(result)
using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
{
    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
    {
        result = sr.ReadToEnd();
    }
}
```
### POST
- POST 不會將傳送的資料加在URL地址後面，而是另外寫在message-body
```csharp=
string targetUrl = "(要查詢的URL)";
string parame = "p=Arvin";
byte[] postData = Encoding.UTF8.GetBytes(parame);
HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
request.Method = "POST";
request.ContentType = "application/x-www-form-urlencoded";
request.Timeout = 30000;
request.ContentLength = postData.Length;
// 寫入 Post Body Message 資料流
using (Stream st = request.GetRequestStream())
{
    st.Write(postData, 0, postData.Length);
}
string result = "";
// 取得回應資料
using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
{
    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
    {
        result = sr.ReadToEnd();
    }
}
```

## 參考資料
[MSDN 部分類別和方法 (C# 程式設計手冊)](https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods)  
[[.NET C#] Partial Classes　部份類別](https://dotblogs.com.tw/jackeir/2015/06/16/151580)  
[MSDN 如何覆寫 ToString 方法](https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/classes-and-structs/how-to-override-the-tostring-method)  
[使用 HttpWebRequest POST/GET 方法](https://dotblogs.com.tw/joysdw12/2012/12/04/85380)  


