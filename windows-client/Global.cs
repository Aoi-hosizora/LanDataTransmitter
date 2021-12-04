using System;
using Grpc.Core;
using Channels = System.Threading.Channels;

namespace LanDataTransmitter {

    enum ApplicationBehavior {
        AS_SERVER,
        AS_CLIENT,
    }

    static class Global {

        public static ApplicationBehavior behavior;
        public static GrpcService grpcService;

        // for server only
        public static bool isBindClient;
        public static string bindClientId;
        public static Channels.Channel<PullTextReply> pullTextChannel;
        public static Channels.Channel<Exception> pullTextExceptionChannel;

        // for client only
        public static string clientId;

    }
}
