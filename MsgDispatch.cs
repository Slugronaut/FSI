using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox;

namespace SAOT
{
    /// <summary>
    /// Global singleton (yuck) that acts as a centralized hub for transmitting messages.
    /// It can be used to post global messages as well as forwarding targeted ones.
    /// 
    /// Messages can be instantaeous or delayed so that they process at the beginning
    /// of the next frame. They can also be buffered so that all listeners that come to
    /// the party late will still receive past messages.
    /// 
    /// This should not be placed on an Object. Instead, it will automatically instantiate
    /// a hidden GameObject for the lifetime of the application.
    /// 
    /// This object will not work outside of play-mode. Any calls to it from edit-mode will be ignored.
    /// </summary>
    /// 
    /// <remarks>
    /// This behaviour attempts to force itself to be the first object in the script update execution order.
    /// You should occasionally verify that this remains true to ensure proper and consistent functionality.
    /// </remarks>
    /// 
    ///
    public sealed class MsgDispatch
    {
        static AllPurposeMessageDispatcher Dispatcher = new AllPurposeMessageDispatcher();


        #region Static Methods
        /// <summary>
        /// Processes all delayed and pending message events.
        /// </summary>
        public static void Pump()
        {
            Dispatcher.ProcessAllPendingMessages();
        }

        /// <summary>
        /// Adds a listener of an event type to this message pump.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public static void AddListener<T>(MessageHandler<T> handler) where T : IMessage
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.AddListener(handler);
        }

        /// <summary>
        /// Adds a listener of an event type to this message pump.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public static void AddListener(Type msgType, MessageHandler handler)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.AddListener(msgType, handler);
        }

        /// <summary>
        /// Removes a listener of an event type from this message pump.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public static void RemoveListener<T>(MessageHandler<T> handler) where T : IMessage
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.RemoveListener(handler);
        }

        /// <summary>
        /// Removes a listener of an event type from this message pump.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public static void RemoveListener(Type msgType, MessageHandler handler)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.RemoveListener(msgType, handler);
        }

        /// <summary>
        /// Forwards the given message to the local message dispatch associated with the GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="localDispatch"></param>
        /// <param name="msg"></param>
        public static void ForwardDispatch<T>(object localDispatch, T msg) where T : IMessage
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.ForwardDispatch(localDispatch, msg);
        }

        /// <summary>
        /// Forwards the given message to the local message dispatch associated with the GameObject.
        /// </summary>
        /// <param name="localDispatch"></param>
        /// <param name="msg"></param>
        public static void ForwardDispatch(object localDispatch, Type msgType, IMessage msg)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.ForwardDispatch(localDispatch, msgType, msg);
        }

        /// <summary>
        /// Posts a message using a strategy based on what interfaces the message implements. Messages that implement
        /// IDeferedMessage will be processed on the next frame. IBufferedMessages will be buffered for all future listeners.
        /// Messages that implement both will be defered to the next frame and *then* buffered. Messages that implement neither
        /// will be dispatched immediately.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void PostMessage<T>(T msg) where T : IMessage
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            var dm = msg as IDeferredMessage;
            var bm = msg as IBufferedMessage;

            //Strategizing the type of message to post 
            //based on what interface(s) it implements.
            if (dm != null)
            {
                if (bm != null) Dispatcher.PostDelayedBufferedMessage(dm);
                else Dispatcher.PostDelayedMessage(dm);
            }
            else if (bm != null) Dispatcher.PostBufferedMessage(bm);
            else Dispatcher.PostMessage(msg);
        }


        /// <summary>
        /// Posts a message using a strategy based on what interfaces the message implements. Messages that implement
        /// IDeferedMessage will be processed on the next frame. IBufferedMessages will be buffered for all future listeners.
        /// Messages that implement both will be defered to the next frame and *then* buffered. Messages that implement neither
        /// will be dispatched immediately.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void PostMessage(Type msgType, IMessage msg)
        {
            //throw new UnityException("Not yet implmented.");

#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            var dm = msg as IDeferredMessage;
            var bm = msg as IBufferedMessage;

            //Strategizing the type of message to post 
            //based on what interface(s) it implements.
            if (dm != null)
            {
                if (bm != null) Dispatcher.PostDelayedBufferedMessage(msgType, dm);
                else Dispatcher.PostDelayedMessage(msgType, dm);
            }
            else if (bm != null) Dispatcher.PostBufferedMessage(msgType, bm);
            else Dispatcher.PostMessage(msgType, msg);
        }

        /// <summary>
        /// Registers an object a being associated to a local dispatcher. This allows multiple
        /// different GameObjects to be able to be used as forward targets that will redirect
        /// the message to the same dispatcher.
        /// </summary>
        /// <param name="dispatchOwner"></param>
        /// <param name="dispatcher"></param>
        public static void RegisterLocalDispatch(object dispatchOwner, IMessageDispatcher dispatcher)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.RegisterLocalDispatch(dispatchOwner, dispatcher);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchOwner"></param>
        public static void UnregisterLocalDispatch(object dispatchOwner)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.UnregisterLocalDispatch(dispatchOwner);
        }

        /// <summary>
        /// Removes all internally pending, buffered, or pending-buffered messages.
        /// Use with caution.
        /// </summary>
        public static void ClearAllMessages()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif
            Dispatcher.ClearAllMessages();
        }

        /// <summary>
        /// Rmoves all messages of the given type.
        /// </summary>
        /// <param name="msgType"></param>
        public static void ClearMessagesOfType(Type msgType)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif
            Dispatcher.ClearMessagesOfType(msgType);
        }

        /// <summary>
        /// Removes all internally pending messages. Often used between scene switches.
        /// </summary>
        public static void ClearPendingMessages()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.ClearPendingMessages();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void RemoveBufferedMessage<T>(T msg) where T : IBufferedMessage
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || AppIsQuitting) return;
#endif

            Dispatcher.RemoveBufferedMessage(msg);
        }
        #endregion

    }
}
