# Static(靜態)關鍵字 介紹
## 前言
在了解static的用法之前，必須先對物件導向技術的相關名詞Class(類別), Object(物件), Instance(實例)有所理解，下面是它們在C#的定義:
> **A class is a data structure in C# that combines data variables and functions into a single unit. Instances of the class are known as objects.** While a class is just a blueprint, the object is an actual instantiation of the class and contains data. The different operations are performed on the object.

>Object(物件)與Instance(實例)的差異 :
一個object就是某個class的instance，可以把object和instance看作是同樣的東西。實例是該物件的唯一初始化，例如Car myCar = new Car()(使用Car類別實例一個物件，物件名稱為myCar)

## 什麼是static修飾詞關鍵字? 為什麼要用static修飾詞?
- 使用 static 修飾詞來宣告靜態成員，而靜態成員屬於類型本身，而不是特定物件(Object)，所以不需要實例(Instance)就能訪問  
- 可以將static宣告在欄位、方法、屬性、運算子、事件、建構函式和類別  

### 靜態類別
static 類別基本上與非靜態類別相同，但有一項差異︰**無法具體化靜態類別**。所以不能使用 new 運算子來建立類別型別的物件變數。 另外，可以使用靜態類別名稱本身來存取靜態類別的成員。 
在 .NET 類別庫中，靜態 System.Math 類別包含執行數學運算的方法，而不需要儲存或擷取特定類別實例 Math 特有的資料。
```
double dub = -3.14;  
Console.WriteLine(Math.Abs(dub));  
```
靜態類別的類型資訊會由 .NET 執行時間載入。 程式**無法指定確實載入類別的時間**。 但一定會載入類別、初始化其欄位，並在第一次於程式中參考類別之前呼叫其靜態建構函式。 **只會呼叫靜態建構函式一次**，而且靜態類別在程式所在應用程式定義域的存留期間**保留在記憶體中**。

在此MSDN文件提到，若要建立只有本身的一個執行實例的非靜態類別，請參考[ Implementing Singleton in C#](https://learn.microsoft.com/zh-tw/previous-versions/msp-n-p/ff650316(v=pandp.10)) 和 [筆記](https://)

### 靜態成員
非靜態類別可以包含靜態方法、欄位、屬性或事件。 即使尚未建立類別的執行個體，還是可以在類別上呼叫靜態成員。 **靜態成員一律是透過類別名稱進行存取**。 不論建立多少個類別執行個體，都**只會有一個靜態成員**
- 程式實作上，比較常使用靜態成員宣告非靜態類別，比較不會將整個類別宣告為靜態，因為如果static關鍵字套用至類別，此類別的所有成員都必須是 static
- 靜態欄位常用情境 :
    儲存必須在所有執行個體之間共用的值
    計算已具體化的物件數目
- 要注意的是: C# 不支援靜態區域變數 (在方法範圍中宣告的變數)
public static string test()
{
    ~~static~~ string name = "";
}

靜態類別的類型資訊會由 .NET 執行時間載入。 程式**無法指定確實載入類別的時間**。 但一定會載入類別、初始化其欄位，並在第一次於程式中參考類別之前呼叫其靜態建構函式。 **只會呼叫靜態建構函式一次**，而且靜態類別在程式所在應用程式定義域的存留期間**保留在記憶體中**。

在此MSDN文件提到，若要建立只有本身的一個執行實例的非靜態類別，請參考[ Implementing Singleton in C#](https://learn.microsoft.com/zh-tw/previous-versions/msp-n-p/ff650316(v=pandp.10)) 和 [筆記](https://)

## 參考資料
[Classes and Objects in C#](https://www.knowledgehut.com/tutorials/csharp/csharp-classes-objects)
[msdn-static (C# 參考)](https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/keywords/static)
[msdn-靜態類別和靜態類別成員 (C# 程式設計手冊)](https://)
