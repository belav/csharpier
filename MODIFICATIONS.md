# CSharpier-Flex 扩展记录

本文档记录对 CSharpier 原版所做的所有定制化修改。

---

## 配置方式

所有扩展行为统一由 `formattingStyle` 配置项控制：

```json
// .csharpierrc
{
  "formattingStyle": "resharper",
  "printWidth": 130
}
```

- `"default"`（默认，不写此项时）：使用原版 CSharpier 格式化行为
- `"resharper"`：启用下列所有修改

---

## 修改列表

<!-- 按时间倒序排列，最新的在最上面 -->

### 3. 方法调用尾部 Lambda 参数格式化

**日期**：2026-05-22

**目的**：当方法调用的最后一个参数是带有代码块（block body）的 lambda 表达式时，非 lambda 参数保持在方法调用的同一行，仅 lambda 体换行。避免所有参数被打散到各自的新行。

**修改前**（所有参数各占一行）：
```csharp
BindButton(
    _buttonChallenge,
    () =>
    {
        var playBtn = _buttonChallenge.GetComponent<PlayButton>();
        bool allowDiamond = playBtn != null && playBtn.AllowDiamond;
        Presenter.HandleChallengeClicked(allowDiamond, CloseCT);
    }
);
```

**修改后**（非 lambda 参数保持内联，`{` 与方法调用同级，`});` 紧跟）：
```csharp
BindButton(_buttonChallenge, () =>
{
    var playBtn = _buttonChallenge.GetComponent<PlayButton>();
    bool allowDiamond = playBtn != null && playBtn.AllowDiamond;
    Presenter.HandleChallengeClicked(allowDiamond, CloseCT);
});
```

**修改文件**：
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/ArgumentListLikeSyntax.cs` — 在默认多参数分支前插入新分支，非 lambda 参数内联打印，lambda 的 Body 直接跟在 Head 后

---

### 2. 二元表达式操作符换行位置 + 括号风格

**日期**：2026-05-22

**目的**：将二元表达式（`||`、`&&`、`+`、`==`、`??` 等）换行时操作符的位置从"下一行开头"改为"当前行末尾"；同时统一 `if` / `while` / `do-while` / `switch` 的条件括号风格，使条件紧跟 `(`，`)` 紧跟最后一个条件。

**修改前**（操作符在下一行开头）：
```csharp
if (
    GameManager.Instance.gameMode == GameManager.GameMode.Classic
    || GameManager.Instance.gameMode == GameManager.GameMode.GroupChallenge
) { }
```

**修改后**（操作符在当前行末尾）：
```csharp
if (GameManager.Instance.gameMode == GameManager.GameMode.Classic ||
    GameManager.Instance.gameMode == GameManager.GameMode.GroupChallenge)
{
```

**修改文件**：
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/BinaryExpression.cs` — 条件化操作符与换行符的 Doc 顺序
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/IfStatement.cs` — 条件化括号风格
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/WhileStatement.cs` — 同上
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/DoStatement.cs` — 同上
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/SwitchStatement.cs` — 同上

---

### 1. 字段属性同行

**日期**：2026-05-22

**目的**：允许字段声明的属性（Attribute）与字段保持在同一行，而不是强制换行。适用于 Unity 项目中常见的 `[SerializeField] private Button _buttonQuit;` 风格。

**行为**（`formattingStyle: "resharper"` 时生效）：
```csharp
// 1-2 个属性 → 全部同行
[SerializeField] private Button _buttonQuit;
[SerializeField] [HideInInspector] private Text _label;
// 3 个属性 → 属性同行，字段换行
[SerializeField] [HideInInspector] [Header("Title")]
private Text _labelTitle;
// 4+ 个属性 → 全部换行（原版行为）
[SerializeField]
[HideInInspector]
[Header("Title")]
[Tooltip("tip")]
private Text _labelTitle;
```

**修改文件**：
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/AttributeLists.cs` — 字段声明时根据 FormattingStyle 选择分隔符
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/BaseFieldDeclaration.cs` — 启用时用 `Doc.Group` 包裹

---

## 配置基础设施

**配置项 `formattingStyle`**（`"default"` / `"resharper"`）统一控制所有修改行为。

**修改文件**：
- `Src/CSharpier.Core/PrinterOptions.cs` — 新增 `FormattingStyle` 属性和 `public enum FormattingStyle`
- `Src/CSharpier.Core/CodeFormatterOptions.cs` — 公共 API 新增 `FormattingStyle` 属性
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/PrintingContext.cs` — `PrintingContextOptions` 新增 `FormattingStyle`
- `Src/CSharpier.Core/CSharp/CSharpFormatter.cs` — 传递配置到 `PrintingContext`
- `Src/CSharpier.Cli/Options/ConfigurationFileOptions.cs` — 配置文件解析新增字段
- `Src/CSharpier.Core/PublicAPI.Unshipped.txt` — 更新公共 API 声明
- `Src/CSharpier.Tests/AllowFieldAttributeOnSameLineTests.cs` — 测试适配
