namespace Ra.Common.Wpf
{
    #region Namespace Using

    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    #endregion

    public abstract class CheckedViewModel
    {
        #region Properties

        public virtual string TextKey { get; }

        public virtual bool Value { get; set; }

        #endregion

        #region

        public static CheckedViewModel Create<T>(T owner, Expression<Func<T, bool>> outExpr, string text)
        {
            return new CheckedViewModel<T>(owner, outExpr, text);
        }

        #endregion
    }

    public class CheckedViewModel<T> : CheckedViewModel
    {
        #region  Fields

        private readonly object _owner;

//        private string _name;

        private readonly PropertyInfo _prop;

        #endregion

        #region  Constructors

        /// <summary>
        /// </summary>
        /// <param name="outExpr"></param>
        public CheckedViewModel(T owner, Expression<Func<T, bool>> outExpr, string textKey)
        {
            _owner = owner;
            var expr = (MemberExpression) outExpr.Body;
            _prop = (PropertyInfo) expr.Member;
            TextKey = textKey;
        }

        #endregion

        #region Properties

        public override string TextKey { get; }

        public override bool Value
        {
            get => (bool) _prop.GetValue(_owner);
            set => _prop.SetValue(_owner, value);
        }

        #endregion
    }
}