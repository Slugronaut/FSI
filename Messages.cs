using System;


namespace Toolbox
{
    /// <summary>
    /// A simple message that can be posted by triggers when something touches them.
    /// </summary>
    public class TriggerEvent : IMessage { }


    /// <summary>
    /// Base class for deriving messages that supply a single target of an event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TargetedEvent<T> : IMessage
	{
        /// <summary>
        /// The target that the event is happening to.
        /// </summary>
        public T Target { get; protected set; }
		public TargetedEvent(T target)
        {
            Target = target;
        }

        protected TargetedEvent(){}

        /// <summary>
        /// Used to internally change the target without requiring a recreation of this message object.
        /// When using this method be absolutely sure to cast to the correct message type or your message will be mistranslated as type of IMessage!
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual TargetedEvent<T> Change(T target)
        {
            Target = target;
            return this;
        }
	}


    /// <summary>
    /// Base class for deriving messages that supply two interrelated targets of an event.
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="T"></typeparam>
    public abstract class AgentTargetEvent<A,T> : IMessage
	{
        /// <summary>
        /// The agent that caused the the event.
        /// </summary>
        public A Agent { get; protected set; }
        /// <summary>
        /// The target that the event is happening to.
        /// </summary>
        public T Target { get; protected set; }
		public AgentTargetEvent(A agent, T target)
        {
            Agent = agent;
            Target = target;
        }

        protected AgentTargetEvent(){}
        
        /// <summary>
        /// Used to internally change the target and agent without requiring a recreation of this message object.
        /// WARNING: !! When using this method be absolutely sure to cast to the correct message type or your message will be mistranslated as type of IMessage!
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual AgentTargetEvent<A,T> Change(A agent, T target)
        {
            Agent = agent;
            Target = target;
            return this;
        }
	}


    /// <summary>
    /// base demand message. This version does not accept a callback as a parameter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SimpleDemand<T> : IDemand<T> where T : class
    {
        public T Desired { get; protected set; }
        bool Responded;

        public SimpleDemand() { }

        public void Respond(T desired)
        {
            //only one responder allowed
            if (Responded) return;

            Desired = desired;
            Responded = true;
        }

        /// <summary>
        /// Resets the internal response values to null.
        /// If you intend to cache a Demand, you must call this
        /// before each new call.
        /// </summary>
        public SimpleDemand<T> Reset()
        {
            Desired = null;
            Responded = false;
            return this;
        }
    }


    /// <summary>
    /// Base demand message.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Demand<T> : IDemand<T> where T : class
    {
        Action<T> Response;
        public T Desired { get; protected set; }
        bool Responded;

        private Demand() { }

        public Demand(Action<T> callback)
        {
            Response = callback;
        }

        public Demand<T> ChangeCallback(Action<T> callback)
        {
            Response = callback;
            return this;
        }

        public void Respond(T desired)
        {
            //only one responder allowed
            if (Responded) return;

            Desired = desired;
            Responded = true;
            if (Response != null) Response(Desired);
        }

        /// <summary>
        /// Resets the internal response values to null.
        /// If you intend to cache a Demand, you must call this
        /// before each new call.
        /// </summary>
        public Demand<T> Reset()
        {
            Desired = null;
            Responded = false;
            return this;
        }

    }
}