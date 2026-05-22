# CSharpier-Flex 魔修改记录

本文档记录对 CSharpier 原版所做的所有定制化修改。

---

## 修改列表

<!-- 按时间倒序排列，最新的在最上面 -->

### 2. 二元表达式操作符换行位置修改

**日期**：2026-05-22

**目的**：将二元表达式（`||`、`&&`、`+`、`==`、`??` 等）换行时操作符的位置从"下一行开头"改为"当前行末尾"，使代码风格更符合 C# 社区常见习惯。

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

**影响范围**：
- 所有使用二元操作符（`||`、`&&`、`+`、`-`、`==`、`!=`、`??` 等）且需要换行的表达式
- 包括 `if` / `while` / `do-while` / `switch` 条件、变量赋值、`return` 语句、Lambda 表达式体等

**修改文件**：
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/BinaryExpression.cs` — 调整操作符与换行符的 Doc 顺序（操作符移到 `Doc.Line` 之前），同时同步修改 `??` 操作符的处理逻辑
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/IfStatement.cs` — 去掉条件前后的 `Doc.IfBreak(SoftLine)`，将 `)` 纳入同一 `Doc.Group`，使条件紧跟 `if (`
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/WhileStatement.cs` — 同步修改，去掉内层 `Doc.Group` + `SoftLine`，与 `if` 保持一致
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/DoStatement.cs` — 同步修改 `do-while` 的条件括号风格
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/SwitchStatement.cs` — 同步修改，将 `(` `)` 移入 `Doc.GroupWithId` 内
- 31 个测试快照文件 — 更新所有涉及二元表达式换行和括号风格变化的快照

---

### 1. 新增配置项 `allowFieldAttributeOnSameLine`

**日期**：2026-05-22

**目的**：允许字段声明的属性（Attribute）与字段保持在同一行，而不是强制换行。适用于 Unity 项目中常见的 `[SerializeField] private Button _buttonQuit;` 风格。

**配置方式**（`.csharpierrc`）：
```json
{
  "allowFieldAttributeOnSameLine": true
}
```

**行为**：
- `false`（默认）：属性与字段之间强制换行（原版行为）
  ```csharp
  [SerializeField]
  private Button _buttonQuit;
  ```
- `true`：属性与字段允许在同一行，按数量分级处理：
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
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/AttributeLists.cs` — 字段声明时根据配置选择 `Doc.Line`（可同行）或 `Doc.HardLine`（强制换行）
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/SyntaxNodePrinters/BaseFieldDeclaration.cs` — 启用时用 `Doc.Group` 包裹整个字段声明，使 `Doc.Line` 能折叠为空格
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/PrintingContext.cs` — `PrintingContextOptions` 新增属性
- `Src/CSharpier.Core/PrinterOptions.cs` — 新增属性
- `Src/CSharpier.Core/CodeFormatterOptions.cs` — 公共 API 新增属性
- `Src/CSharpier.Core/CSharp/CSharpFormatter.cs` — 传递配置到 `PrintingContext`
- `Src/CSharpier.Cli/Options/ConfigurationFileOptions.cs` — 配置文件解析新增字段
- `Src/CSharpier.Tests/AllowFieldAttributeOnSameLineTests.cs` — 测试用例


---

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

**触发条件**：
- 方法调用有 2 个及以上参数
- 最后一个参数是 `ParenthesizedLambdaExpression`（`() => { ... }`）或 `SimpleLambdaExpression`（`x => { ... }`）且包含非空代码块
- 不影响：单参数 lambda（已有专门处理）、表达式体 lambda（`() => expr`）、空 block lambda（`() => { }`）

**修改文件**：
- `Src/CSharpier.Core/CSharp/SyntaxPrinter/ArgumentListLikeSyntax.cs` — 在默认多参数分支前插入新分支，非 lambda 参数内联打印，lambda 的 Body 用 `Doc.Indent` 包裹实现缩进