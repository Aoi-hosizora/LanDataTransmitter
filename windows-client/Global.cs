using System;

namespace LanDataTransmitter {

    enum ApplicationBehavior {
        AS_SERVER,
        AS_CLIENT,
    }

    static class Global {

        public static ApplicationBehavior Behavior;
        public static GrpcService GrpcService;

        // for server only
        public static bool IsBindingClient;
        public static string BindClientId;
        public static BidirectionalChannel<PullTextReply, Exception> PullChannel;

        // for client only
        public static string SelfClientId;

    }
}
