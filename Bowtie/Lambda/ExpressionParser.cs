using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        internal bool HasNullValue(BinaryExpression exp)
        {
            return exp.Left is ConstantExpression leftConst && leftConst.Value == null || exp.Right is ConstantExpression rightConst && rightConst == null;
        }

        internal void ParseCondition(Expression exp, StringBuilder sb, DatabaseType dbType)
        {
            switch (exp.NodeType)
            {
                // not
                case ExpressionType.Not:
                    {
                        var unaryExp = (UnaryExpression)exp;
                        switch (unaryExp.Operand.NodeType)
                        {
                            // bool false
                            case ExpressionType.MemberAccess:
                                {
                                    ParseMemberAccess((MemberExpression)unaryExp.Operand, sb, dbType);
                                    sb.Append(" = 0");
                                    break;
                                }
                            // not like
                            case ExpressionType.Call:
                                {
                                    var callExp = (MethodCallExpression)unaryExp.Operand;
                                    switch (callExp.Method.Name)
                                    {
                                        case "Contains":
                                            {
                                                ParseLike(callExp, true, true, true, sb, dbType);
                                                break;
                                            }
                                        case "StartsWith":
                                            {
                                                ParseLike(callExp, true, false, true, sb, dbType);
                                                break;
                                            }
                                        case "EndsWith":
                                            {
                                                ParseLike(callExp, true, true, false, sb, dbType);
                                                break;
                                            }
                                        default:
                                            throw new Exception("Invalid lambda query expression");
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                // and conjunction
                case ExpressionType.AndAlso:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        sb.Append("(");
                        ParseCondition(binaryExp.Left, sb, dbType);
                        sb.Append(") AND (");
                        ParseCondition(binaryExp.Right, sb, dbType);
                        sb.Append(")");
                        break;
                    }
                // or conjunction
                case ExpressionType.OrElse:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        sb.Append("(");
                        ParseCondition(binaryExp.Left, sb, dbType);
                        sb.Append(") OR (");
                        ParseCondition(binaryExp.Right, sb, dbType);
                        sb.Append(")");
                        break;
                    }
                // bool true
                case ExpressionType.MemberAccess:
                    {
                        ParseMemberAccess((MemberExpression)exp, sb, dbType);
                        sb.Append(" = 1");
                        break;
                    }
                // eq
                case ExpressionType.Equal:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        ParseOperand(binaryExp.Left, sb, dbType);
                        sb.Append(HasNullValue(binaryExp) ? " IS " : " = ");
                        ParseOperand(binaryExp.Right, sb, dbType);
                        break;
                    }
                // neq
                case ExpressionType.NotEqual:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        ParseOperand(binaryExp.Left, sb, dbType);
                        sb.Append(HasNullValue(binaryExp) ? " IS NOT " : " <> ");
                        ParseOperand(binaryExp.Right, sb, dbType);
                        break;
                    }
                // gt
                case ExpressionType.GreaterThan:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        ParseOperand(binaryExp.Left, sb, dbType);
                        sb.Append(" > ");
                        ParseOperand(binaryExp.Right, sb, dbType);
                        break;
                    }
                // geq
                case ExpressionType.GreaterThanOrEqual:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        ParseOperand(binaryExp.Left, sb, dbType);
                        sb.Append(" >= ");
                        ParseOperand(binaryExp.Right, sb, dbType);
                        break;
                    }
                // lt
                case ExpressionType.LessThan:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        ParseOperand(binaryExp.Left, sb, dbType);
                        sb.Append(" < ");
                        ParseOperand(binaryExp.Right, sb, dbType);
                        break;
                    }
                // leq
                case ExpressionType.LessThanOrEqual:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        ParseOperand(binaryExp.Left, sb, dbType);
                        sb.Append(" <= ");
                        ParseOperand(binaryExp.Right, sb, dbType);
                        break;
                    }
                // like
                case ExpressionType.Call:
                    {
                        var callExp = (MethodCallExpression)exp;
                        switch (callExp.Method.Name)
                        {
                            case "Contains":
                                {
                                    ParseLike(callExp, false, true, true, sb, dbType);
                                    break;
                                }
                            case "StartsWith":
                                {
                                    ParseLike(callExp, false, false, true, sb, dbType);
                                    break;
                                }
                            case "EndsWith":
                                {
                                    ParseLike(callExp, false, true, false, sb, dbType);
                                    break;
                                }
                            default:
                                throw new Exception("Invalid lambda query expression");
                        }
                        break;
                    }
                default:
                    throw new Exception("Invalid lambda query expression");
            }
        }

        internal void ParseOperand(Expression exp, StringBuilder sb, DatabaseType dbType)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Convert:
                    {
                        var unaryExp = (UnaryExpression)exp;
                        ParseMemberAccess((MemberExpression)unaryExp.Operand, sb, dbType);
                        break;
                    }
                case ExpressionType.MemberAccess:
                    {
                        ParseMemberAccess((MemberExpression)exp, sb, dbType);
                        break;
                    }
                case ExpressionType.Constant:
                    {
                        var constantExp = (ConstantExpression)exp;
                        sb.Append(constantExp.Value == null ? "NULL" : Parameterize(constantExp.Value));
                        break;
                    }
                default:
                    throw new Exception("Invalid lambda query expression");
            }
        }

        internal void ParsePredicate(Expression exp, string @operator, StringBuilder sb, DatabaseType dbType)
        {
            var binaryExp = (BinaryExpression)exp;
            MemberExpression memberExp;
            // enum
            if (binaryExp.Left is UnaryExpression)
            {
                var unaryExp = (UnaryExpression)binaryExp.Left;
                memberExp = (MemberExpression)unaryExp.Operand;
            }
            // others
            else
            {
                memberExp = (MemberExpression)binaryExp.Left;
            }
            var prop = (PropertyInfo)memberExp.Member;
            var paramExp = (ParameterExpression)memberExp.Expression;
            var alias = paramExp.Name;
            // if alias is in table refs, append alias
            if (TableRefs.Any(t => t.Alias == alias))
            {
                sb.Append(alias + ".");
            }
            sb.Append(SyntaxAdapter.ApplyColumnModifier(SyntaxConstructor.GetColumnName(prop), dbType));

            if (binaryExp.Right.NodeType == ExpressionType.Constant)
            {
                var constantExp = (ConstantExpression)binaryExp.Right;
                if (constantExp.Value == null)
                {
                    switch (@operator)
                    {
                        case "=":
                            @operator = "IS";
                            break;
                        case "<>":
                            @operator = "IS NOT";
                            break;
                        case "SET":
                            @operator = "=";
                            break;
                        default:
                            throw new Exception("Invalid lambda query expression");
                    }
                    sb.Append($" {@operator} NULL");
                }
                else
                {
                    if (@operator == "SET")
                    {
                        @operator = "=";
                    }

                    sb.Append($" {@operator} ");
                    sb.Append(Parameterize(constantExp.Value));
                }
            }
            else
            {
                if (@operator == "SET")
                {
                    @operator = "=";
                }

                switch (prop.PropertyType.Name)
                {
                    case "Int16":
                        {
                            sb.Append($" {@operator} ");
                            var valExp = Expression.Lambda<Func<short>>(binaryExp.Right);
                            var val = valExp.Compile();
                            sb.Append(Parameterize(val()));
                            break;
                        }
                    case "Int32":
                        {
                            sb.Append($" {@operator} ");
                            var valExp = Expression.Lambda<Func<int>>(binaryExp.Right);
                            var val = valExp.Compile();
                            sb.Append(Parameterize(val()));
                            break;
                        }
                    case "Int64":
                        {
                            sb.Append($" {@operator} ");
                            var valExp = Expression.Lambda<Func<long>>(binaryExp.Right);
                            var val = valExp.Compile();
                            sb.Append(Parameterize(val()));
                            break;
                        }
                    case "Float":
                        {
                            sb.Append($" {@operator} ");
                            var valExp = Expression.Lambda<Func<float>>(binaryExp.Right);
                            var val = valExp.Compile();
                            sb.Append(Parameterize(val()));
                            break;
                        }
                    case "Double":
                        {
                            sb.Append($" {@operator} ");
                            var valExp = Expression.Lambda<Func<double>>(binaryExp.Right);
                            var val = valExp.Compile();
                            sb.Append(Parameterize(val()));
                            break;
                        }
                    case "Decimal":
                        {
                            sb.Append($" {@operator} ");
                            var valExp = Expression.Lambda<Func<decimal>>(binaryExp.Right);
                            var val = valExp.Compile();
                            sb.Append(Parameterize(val()));
                            break;
                        }
                    case "Boolean":
                        {
                            sb.Append($" {@operator} ");
                            var valExp = Expression.Lambda<Func<bool>>(binaryExp.Right);
                            var val = valExp.Compile();
                            sb.Append(Parameterize(val()));
                            break;
                        }
                    case "String":
                        {
                            sb.Append($" {@operator} ");
                            var valExp = Expression.Lambda<Func<string>>(binaryExp.Right);
                            var val = valExp.Compile();
                            sb.Append(Parameterize(val()));
                            break;
                        }
                    case "DateTime":
                        {
                            sb.Append($" {@operator} ");
                            var valExp = Expression.Lambda<Func<DateTime>>(binaryExp.Right);
                            var val = valExp.Compile();
                            sb.Append(Parameterize(val()));
                            break;
                        }
                    case "Nullable`1":
                        {
                            switch (prop.PropertyType.GenericTypeArguments[0].Name)
                            {
                                case "Int16":
                                    {
                                        sb.Append($" {@operator} ");
                                        var valExp = Expression.Lambda<Func<short?>>(binaryExp.Right);
                                        var val = valExp.Compile();
                                        sb.Append(Parameterize(val()));
                                        break;
                                    }
                                case "Int32":
                                    {
                                        sb.Append($" {@operator} ");
                                        var valExp = Expression.Lambda<Func<int?>>(binaryExp.Right);
                                        var val = valExp.Compile();
                                        sb.Append(Parameterize(val()));
                                        break;
                                    }
                                case "Int64":
                                    {
                                        sb.Append($" {@operator} ");
                                        var valExp = Expression.Lambda<Func<long?>>(binaryExp.Right);
                                        var val = valExp.Compile();
                                        sb.Append(Parameterize(val()));
                                        break;
                                    }
                                case "Float":
                                    {
                                        sb.Append($" {@operator} ");
                                        var valExp = Expression.Lambda<Func<float?>>(binaryExp.Right);
                                        var val = valExp.Compile();
                                        sb.Append(Parameterize(val()));
                                        break;
                                    }
                                case "Double":
                                    {
                                        sb.Append($" {@operator} ");
                                        var valExp = Expression.Lambda<Func<double?>>(binaryExp.Right);
                                        var val = valExp.Compile();
                                        sb.Append(Parameterize(val()));
                                        break;
                                    }
                                case "Decimal":
                                    {
                                        sb.Append($" {@operator} ");
                                        var valExp = Expression.Lambda<Func<decimal?>>(binaryExp.Right);
                                        var val = valExp.Compile();
                                        sb.Append(Parameterize(val()));
                                        break;
                                    }
                                case "Boolean":
                                    {
                                        sb.Append($" {@operator} ");
                                        var valExp = Expression.Lambda<Func<bool?>>(binaryExp.Right);
                                        var val = valExp.Compile();
                                        var ret = val();
                                        if (!ret.HasValue)
                                        {
                                            throw new Exception("Null value in boolean comparison");
                                        }
                                        sb.Append(Parameterize(val()));
                                        break;
                                    }
                                case "DateTime":
                                    {
                                        sb.Append($" {@operator} ");
                                        var valExp = Expression.Lambda<Func<DateTime?>>(binaryExp.Right);
                                        var val = valExp.Compile();
                                        sb.Append(Parameterize(val()));
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
        }

        internal void ParseMemberAccess(MemberExpression exp, StringBuilder sb, DatabaseType dbType)
        {
            // get table alias
            var param = (ParameterExpression)exp.Expression;
            var alias = param.Name;
            // if alias is in table refs, append alias
            if (TableRefs.Any(t => t.Alias == alias))
            {
                sb.Append(alias + ".");
            }
            // get column name
            var prop = (PropertyInfo)exp.Member;
            sb.Append(SyntaxAdapter.ApplyColumnModifier(SyntaxConstructor.GetColumnName(prop), dbType));
        }

        //internal void ParseExpBoolean(Expression exp, bool rightVal, StringBuilder sb, DatabaseType dbType)
        //{
        //    var memberExp = (MemberExpression)exp;
        //    var prop = (PropertyInfo)memberExp.Member;
        //    if (prop.PropertyType.Name == "Boolean")
        //    {
        //        sb.Append($"{SyntaxAdapter.ApplyColumnModifier(SyntaxConstructor.GetColumnName(prop), dbType)} = {(rightVal ? 1 : 0)}");
        //    }
        //    else
        //    {
        //        throw new Exception("Invalid lambda query expression");
        //    }
        //}

        internal void ParseLike(MethodCallExpression exp, bool not, bool preWildcard, bool postWildcard, StringBuilder sb, DatabaseType dbType)
        {
            // member
            if (exp.Object.NodeType != ExpressionType.MemberAccess)
            {
                throw new Exception("Invalid lambda query expression");
            }
            ParseMemberAccess((MemberExpression)exp.Object, sb, dbType);
            
            var arg = exp.Arguments[0];
            var argVal = string.Empty;
            if (arg.NodeType == ExpressionType.Constant)
            {
                argVal = ((ConstantExpression)arg).Value.ToString();
            }
            else
            {
                var valExp = Expression.Lambda<Func<string>>(arg);
                var val = valExp.Compile();
                argVal = val();
            }
            if (preWildcard)
            {
                argVal = "%" + argVal;
            }
            if (postWildcard)
            {
                argVal = argVal + "%";
            }
            sb.Append($" {(not ? "NOT " : string.Empty)}LIKE {Parameterize(argVal)}");
        }

        internal string ParseSortSpec(Expression exp, bool asc, DatabaseType dbType)
        {
            var sb = new StringBuilder();
            if (exp is UnaryExpression unaryExp)
            {
                ParseMemberAccess((MemberExpression)unaryExp.Operand, sb, dbType);
            }
            else if (exp is MemberExpression memberExp)
            {
                ParseMemberAccess(memberExp, sb, dbType);
            }
            
            return $"{sb} {(asc ? "ASC" : "DESC")}";
        }

        internal void ParseAssignment(Expression exp, List<string> assignments, DatabaseType dbType)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Equal:
                    {
                        var sb = new StringBuilder();
                        var binaryExp = (BinaryExpression)exp;
                        ParseOperand(binaryExp.Left, sb, dbType);
                        sb.Append(" = ");
                        ParseOperand(binaryExp.Right, sb, dbType);
                        assignments.Add(sb.ToString());
                        break;
                    }
                case ExpressionType.AndAlso:
                    {
                        var binaryExp = (BinaryExpression)exp;
                        ParseAssignment(binaryExp.Left, assignments, dbType);
                        ParseAssignment(binaryExp.Right, assignments, dbType);
                        break;
                    }
                default:
                    throw new Exception("Invalid lambda query expression");
            }
        }
    }
}