using System;
using System.Runtime.CompilerServices;

namespace Sztorm.Collections
{
    /// <summary>
    ///     Represents an action which uses another action to define specific operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal readonly struct BoxedAction<T> : IAction<T>
    {
        private readonly Action<T> action;

        /// <summary>
        ///     The action which defines specific operation.
        /// </summary>
        public Action<T> InnerAction
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => action;
        }

        /// <summary>
        ///     Constructs an action that takes another action to define specific operation.
        /// </summary>
        /// <param name="Action">The Action defining specific operation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoxedAction(Action<T> action) => this.action = action;

        /// <summary>
        ///     Invokes an action defined by <see cref="InnerAction"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T obj) => action(obj);
    }
}
