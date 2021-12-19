using System;
using System.Collections.Generic;

namespace LanDataTransmitter {

    enum ApplicationBehavior {
        AS_SERVER,
        AS_CLIENT,
    }

    static class Global {

        public static ApplicationBehavior Behavior;
        public static GrpcService GrpcService;

        // for server only
        public static List<string> BindClients { get; } = new List<string>();

        // public static Dictionary<string, string> ClientNames { get; } = new Dictionary<string, string>();
        public static Dictionary<string, BidirectionalChannel<PullTextReply, Exception>> PullChannels { get; } =
            new Dictionary<string, BidirectionalChannel<PullTextReply, Exception>>();

        // for client only
        public static string SelfClientId;

    }
}
