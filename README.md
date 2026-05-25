# CSharpier-Flex

基于 [CSharpier](https://github.com/belav/csharpier) 的 Fork，在保留原版全部功能的基础上，新增 `formattingStyle` 配置项，提供更贴近 ReSharper / Rider 风格的 C# 代码格式化选项。

> 原版 CSharpier 遵循 Prettier 的 [Option Philosophy](https://prettier.io/docs/en/option-philosophy.html)，仅提供极少量格式化选项。本 Fork 在此基础上扩展，通过一个配置项切换整套风格预设，不破坏原版行为。

---

## Quick Start

### 安装

```bash
# 从源码构建（需要 .NET 10 SDK）
git clone https://github.com/AnyAnq/csharpier-flex.git
cd csharpier-flex
dotnet build Src/CSharpier.Cli/CSharpier.Cli.csproj
```

### 使用

```bash
# 格式化当前目录
dotnet run --project Src/CSharpier.Cli/CSharpier.Cli.csproj -- format .

# 格式化单个文件并输出到标准输出
dotnet run --project Src/CSharpier.Cli/CSharpier.Cli.csproj -- format --write-stdout "MyFile.cs"
```

---

## 配置

在项目根目录创建 `.csharpierrc` 文件：

```json
{
  "formattingStyle": "resharper",
  "printWidth": 120
}
```

### `formattingStyle`

| 值 | 说明 |
|---|---|
| `"default"` | 原版 CSharpier 行为（不写此项时的默认值） |
| `"resharper"` | 启用下列所有扩展格式化规则 |

### 其他配置项

原版 CSharpier 的所有配置项均可正常使用：

| 配置项 | 默认值 | 说明 |
|---|---|---|
| `printWidth` | `100` | 行宽限制 |
| `indentSize` | `4` | 缩进空格数 |
| `useTabs` | `false` | 是否使用 Tab 缩进 |
| `endOfLine` | `"auto"` | 换行符类型（`auto` / `lf` / `crlf`） |

---

## 扩展格式化规则

以下规则仅在 `"formattingStyle": "resharper"` 时生效。

### 1. 字段属性同行

属性（Attribute）与字段声明保持在同一行，适用于 Unity 项目中常见的 `[SerializeField]` 风格。

```csharp
// 1-2 个属性 → 全部同行
[SerializeField] private Button _buttonQuit;
[SerializeField] [HideInInspector] private Text _label;

// 3 个属性 → 属性同行，字段换行
[SerializeField] [HideInInspector] [Header("Title")]
private Text _labelTitle;

// 4+ 个属性 → 保持原版行为（各自一行）
[SerializeField]
[HideInInspector]
[Header("Title")]
[Tooltip("tip")]
private Text _labelTitle;
```

### 2. 二元表达式操作符留在行尾

换行时操作符（`||`、`&&`、`??` 等）保留在当前行末尾，而非移到下一行开头。同时 `if` / `while` / `do-while` / `switch` 的条件紧贴括号。

```csharp
// default 风格
if (
    condition1
    || condition2
)

// resharper 风格
if (condition1 ||
    condition2)
```

### 3. 方法调用尾部 Lambda 参数内联

当方法调用的最后一个参数是带代码块的 lambda 时，非 lambda 参数保持在方法名同一行，lambda 体自然换行。

```csharp
// default 风格
BindButton(
    _buttonChallenge,
    () =>
    {
        DoSomething();
    }
);

// resharper 风格
BindButton(_buttonChallenge, () =>
{
    DoSomething();
});
```

---

## 与原版 CSharpier 的关系

- **完全兼容**：不设置 `formattingStyle` 或设为 `"default"` 时，行为与原版 CSharpier 完全一致
- **非破坏性**：所有扩展规则仅在显式配置 `"resharper"` 时生效
- **持续同步**：定期从上游 [belav/csharpier](https://github.com/belav/csharpier) 合并更新

详细修改记录见 [MODIFICATIONS.md](MODIFICATIONS.md)。

---

## 致谢

本项目基于 [CSharpier](https://github.com/belav/csharpier)，感谢原作者及所有贡献者的工作。
