using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Parser
{
    class Program
    {
        private const string TestClass = @"
public class ClassName
{
    void LongMethod(string first, string second, string third, string fourth, string fifth) {
        // do stuff
    }
}
";
        
        static void Main(string[] args)
        {
            var testToParse = args.Length == 0 ? TestClass : args[0];
            
            System.Text.StringBuilder
            
            var rootNode = CSharpSyntaxTree.ParseText(testToParse).GetRoot();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new TypeInsertionResolver(),
                
            };
            
            Console.WriteLine(JsonConvert.SerializeObject(rootNode, jsonSerializerSettings));
        }
    }

    public class TypeInsertionResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);
            for (var x = props.Count - 1; x >= 0; x--)
            {
                var propertyName = props[x].PropertyName ?? "";
                if (propertyName == "syntaxTree"
                    || propertyName.Contains("Trivia")
                    || propertyName == "semicolonToken"
                    || propertyName == "openBraceToken"
                    || propertyName == "closeBraceToken"
                    || propertyName == "endOfFileToken"
                    )
                {
                    props.RemoveAt(x);
                }
            }
            
            if (type.GetProperty("RawKind") != null)
            {
                props.Insert(0, new JsonProperty
                {
                    DeclaringType = type,
                    PropertyType = typeof(string),
                    PropertyName = "nodeType",
                    ValueProvider = new RawKindTypeProvider(),
                    Readable = true,
                    Writable = false
                });
            }

            return props;
        }

        class RawKindTypeProvider : IValueProvider
        {
            public object GetValue(object target)
            {
                if (target is SyntaxNodeOrToken nodeOrToken)
                {
                    return nodeOrToken.Kind().ToString();
                }

                if (target is SyntaxToken token)
                {
                    return token.Kind().ToString();
                }
                if (target is SyntaxTrivia trivia)
                {
                    return trivia.Kind().ToString();
                }
                if (target is SyntaxNode node)
                {
                    return node.Kind().ToString();
                }

                throw new Exception("Did not handle RawKind on type " + target.GetType());
            }
            
            public void SetValue(object target, object value)
            {
                throw new NotImplementedException();
            }
        }
    }
}