using System;
using System.ServiceModel.Channels;

namespace FunctionToDelete
{
    internal class MessageEncodingBindingElementTest : MessageEncodingBindingElement
    {
        public MessageEncodingBindingElementTest() : base()
        {

        }

        public override MessageVersion MessageVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override BindingElement Clone()
        {
            throw new NotImplementedException();
        }

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            throw new NotImplementedException();
        }
    }
}
