using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using ExsysmaAPI.Models;

namespace ExsysmaAPI
{
    public static class Utils
    {
        public static readonly string newline = Environment.NewLine;
        public static string ConvertToReadableRule(Rule rule) {
            if (rule.Conditions.Count == 0 || rule.Conclusion is null) return string.Empty;
            var readableRule = "Se";

            var isFirstCondition = true;
            foreach (var condition in rule.Conditions) {
                var readableCondition = ConvertToReadableRuleItem(condition);
                if (isFirstCondition) {
                    readableRule += $"{newline}{readableCondition}";
                    isFirstCondition = false;
                    continue;
                }
                readableRule += $"{newline}E {readableCondition}";
            }

            readableRule += $"{newline}Então";
            readableRule += $"{newline}{ConvertToReadableRuleItem(rule.Conclusion)}";
            return readableRule;

        }

        public static string ConvertToReadableRuleItem(RuleItem ruleItem) =>
            $"{ruleItem.Variable.Name} {GetOperatorSymbol(ruleItem.Operator)} {ruleItem.Value}";

        public static string GetOperatorSymbol(Operator op) {
            return op switch
            {
                Operator.IsDifferent => "!=",
                Operator.Equals => "==",
                _ => "=="
            };
        }
    }
}