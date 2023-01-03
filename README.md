# XmlPeek

XMLドキュメントの一部分を操作するライブラリです。  
XElementのヘルパークラスのような感じです。

### 使い方

#### Element

```xml
<Root>
  <Element>
    <Child1>Text1</Child1>
    <Child2>Text2</Child2>
  </Element>
  <OtherElement>
    <Child3>Text3</Child3>
    <Child4>Text4</Child4>
  </OtherElement>
</Root>
```

エレメント ```<Element>``` を抽出する場合、以下のようにインスタンスを生成します。

```csharp
var xml = XElement.Load('sample.xml');
var element = new Element(xml, "Element");
```

```element```を生成した後、```xml```は以下のように```<Element>```が空になります。

```xml
<Root>
  <Element/>
  <OtherElement>
    <Child3>Text3</Child3>
    <Child4>Text4</Child4>
  </OtherElement>
</Root>
```

抽出されたエレメントは```Element.XElement```に移動しているので自由に操作できます。

操作後、元に戻す場合は```Poke```メソッドを使います。

```csharp
element.Poke(xml);
```

#### 想定される使い方

```Element.XElement```を直接操作するのはあまり意味がないので、```Element```を派生したクラスでの操作を想定しています。

```csharp
class TestElement : Element {
  public TestElement(XElement? parent) : base(parent, "Element") {}

  public string? Child1 {
    get => GetContent<string>();
    set => SetContent(value);
  }
}
```

```xml
<Root>
  <Element>
    <Child1>Text1</Child1>
    <Child2>Text2</Child2>
  </Element>
  <OtherElement>
    <Child3>Text3</Child3>
    <Child4>Text4</Child4>
  </OtherElement>
</Root>
```

```csharp
var xml = XElement.Load('sample.xml');
var element = new TestElement(xml);

Console.WriteLine(element.Child1); // >> Text1
element.Child1 = "Foo";

element.Poke(xml);
//<Root>
//  <Element>
//    <Child1>Foo</Child1>
//    <Child2>Text2</Child2>
//  </Element>
//  <OtherElement>
//    <Child3>Text3</Child3>
//    <Child4>Text4</Child4>
//  </OtherElement>
//</Root>
```

#### ElementList

```Element```型のリストです。```IList```を実装しています。

```xml
<Root>
  <ElementList>
    <Element>
      <Child>0</Child>
    </Element>
    <Element>
      <Child>1</Child>
    </Element>
    <Element>
      <Child>2</Child>
    </Element>
  </ElementList>
</Root>
```

```csharp
class TestElement : Element {
  public TestElement(XElement? parent) : base(parent, "Element") { }

  public string? Child {
      get => GetContent<string>();
      set => SetContent(value);
  }
}

var xml = XElement.Load('sample.xml');
var element = new ElementList<TestElement>(xml, "ElementList");

Console.WriteLine(element[0].Child); // >> 0
Console.WriteLine(element[1].Child); // >> 1
Console.WriteLine(element[2].Child); // >> 2

element[2].Child = "Foo";
element.Add(new TestElement(null) { Child = "Bar" });

element.Poke(xml);
//<Root>
//  <ElementList>
//    <Element>
//      <Child>0</Child>
//    </Element>
//    <Element>
//      <Child>1</Child>
//    </Element>
//    <Element>
//      <Child>Foo</Child>
//    </Element>
//    <Element>
//      <Child>Bar</Child>
//    </Element>
//  </ElementList>
//</Root>
```

#### ValueList

単純な配列を操作するクラスです。

```xml
<Root>
  <ValueList>
    <Item>0</Item>
    <Item>1</Item>
    <Item>2</Item>
    <Item>3</Item>
    <Item>4</Item>
  </ValueList>
</Root>
```

```csharp
var xml = XElement.Load('sample.xml');
var element = new ValueList<int>(xml, "Item", "ValueList");

Console.WriteLine(element.Count);  // >> 5
Console.WriteLine(element[0]);     // >> 0
Console.WriteLine(element[1]);     // >> 1
Console.WriteLine(element[2]);     // >> 2
Console.WriteLine(element[3]);     // >> 3
Console.WriteLine(element[4]);     // >> 4
```

#### エレメントがない場合

エレメントがない場合、```Element.XElement```は```null```がセットされます。

```xml
<Root/>
```

```csharp
class TestElement : Element {
  public TestElement(XElement? parent) : base(parent, "Element") {}

  public string? Child1 {
    get => GetContent<string>();
    set => SetContent(value);
  }
}

var xml = XElement.Load('sample.xml');
var element = new TestElement(xml);

if (element.XElement == null) {
  // true
}
if (element.Child1 == null) {
  // true
}

element.Poke(xml);
//<Root/>

// プロパティを操作した場合はXElementが生成されます
element.Child1 = "Foo";
element.Poke(xml);
//<Root>
//  <Element>
//    <Child1>Foo</Child1>
//  </Element>
//</Root>
```
