using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    public class TestMessageInspector : IDispatchMessageInspector, IClientMessageInspector
    {
        public static string AfterReceiveRequestMessage;
        public static string BeforeSendReplyMessage;
        public static string AfterReceiveReplyMessage;
        public static string BeforeSendRequestMessage;
        public static string Prefix;

        public TestMessageInspector()
        {            
        }

        public static void Reset()
        {
            AfterReceiveRequestMessage      =
                BeforeSendReplyMessage      = 
                AfterReceiveReplyMessage    = 
                BeforeSendRequestMessage    = string.Empty;
        }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            AfterReceiveRequestMessage = string.Format("{0} After Receive Request", Prefix);
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            BeforeSendReplyMessage = string.Format("{0} Before Send Reply", Prefix);
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            AfterReceiveReplyMessage = string.Format("{0} After Receive Reply", Prefix);
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            BeforeSendRequestMessage = string.Format("{0} Before Send Request", Prefix);
            return null;
        }
    }
}
