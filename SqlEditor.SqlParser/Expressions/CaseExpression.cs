#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace SqlEditor.SqlParser.Expressions
{
    public class CaseSwitch
    {
        public CaseSwitch(Expression parent)
        {
            When = new Expression(parent);
            Then = new Expression(parent);
        }

        public Expression When { get; set; }
        public Expression Then { get; set; }
    }

    public abstract class CaseExpression : Expression, IInlineFormattable
    {
        private const int InlineCaseCount = 4;

        /// <summary>
        ///   Initializes a new instance of the CaseExpression class.
        /// </summary>
        public CaseExpression(Expression parent) : base(parent)
        {
            Cases = new List<CaseSwitch>();
        }

        public List<CaseSwitch> Cases { get; set; }
        public Expression Else { get; set; }

        #region IInlineFormattable Members

        public override bool CanInline
        {
            get
            {
                return
                    Cases.Count < InlineCaseCount &&
                    Cases.All(e => e.When.CanInline && e.Then.CanInline) &&
                    (Else == null || Else is IdentifierExpression || Else is StringExpression);
            }
        }

        #endregion

        protected string GetElseToString()
        {
            return Else != null ? " ELSE " + Else.Value : "";
        }

        protected string GetCasesToString()
        {
            StringBuilder cases = new StringBuilder();
            foreach (var caseSwitch in Cases)
                cases.AppendFormat(
                    " WHEN {0} THEN {1}",
                    caseSwitch.When.Value, caseSwitch.Then.Value
                    );

            return cases.ToString();
        }
    }

    public class CaseSwitchExpression : CaseExpression
    {
        public CaseSwitchExpression(Expression parent) : base(parent)
        {
        }

        public Expression Switch { get; set; }

        public override string Value
        {
            get
            {
                return String.Format(
                    "CASE {0}{1}{2} END",
                    Switch.Value, GetCasesToString(), GetElseToString()
                    );
            }
        }

        public override bool CanInline
        {
            get { return base.CanInline && (Switch is IdentifierExpression || Switch is StringExpression); }
        }
    }

    public class CaseWhenExpression : CaseExpression
    {
        public CaseWhenExpression(Expression parent) : base(parent)
        {
        }

        public override string Value
        {
            get
            {
                return String.Format(
                    "CASE{0}{1} END", GetCasesToString(), GetElseToString()
                    );
            }
        }
    }
}